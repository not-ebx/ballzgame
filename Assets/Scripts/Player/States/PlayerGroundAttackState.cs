using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerGroundAttackState : PlayerState
    {
        private GameObject _hitbox;
        private bool _isCharging;
        private float _attackChargeTime;
        private Vector2 _attackDirection;

        private AudioClip _strongSwingSound;
        private AudioClip _chargeSound;
        private AudioSource _audioSource;

        // Volume and pitch settings
        public float chargeVolume = 0.1f;
        public float strongSwingVolume = 0.1f;
        public float chargePitch = 0.75f;
        public float strongSwingPitch = 1.1f;
        
        // Charge particle prefab
        private GameObject _chargeParticlePrefab = Resources.Load<GameObject>("Particles/Charge/ChargeParticle");
        private GameObject _instancedParticle;

        // Nueva variable para controlar el sonido de carga
        private bool _isChargeSoundPlaying = false;
        private float _chargeThreshold = 0.2f; // Ajusta este valor según sea necesario

        public PlayerGroundAttackState(PlayerController pController) : base(pController)
        {
            _strongSwingSound = Resources.Load<AudioClip>("strong");
            _chargeSound = Resources.Load<AudioClip>("charging");
            _audioSource = PController.gameObject.AddComponent<AudioSource>();
        }

        public override void Enter()
        {
            base.Enter();
            _attackChargeTime = 0f;
            PController.PlayerInputActions.Player.Attack.performed += OnGroundAttack;
            PController.PlayerInputActions.Player.Attack.canceled += OnGroundAttackCanceled;

            _attackChargeTime = Time.time;
            _isCharging = true;

            _attackDirection = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
            PController.anim.Play("GroundAttackCharge");
        }

        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(0, 0);
            
            if (_isCharging)
            {
                float chargeTime = CalculateTimeDifference(_attackChargeTime);
                PController.attackCharge = Mathf.Min(chargeTime, 1.0f);
                
                // Inicia el sonido de carga solo si superamos el umbral
                if (chargeTime > _chargeThreshold && !_isChargeSoundPlaying)
                {
                    PlaySwingSound(_chargeSound, chargeVolume, chargePitch, true);
                    _isChargeSoundPlaying = true;
                }

                _attackDirection = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
                if (IsChargeAnimationFinished() && PController.attackCharge > 0)
                {
                    if (_instancedParticle == null)
                    {
                        _instancedParticle = Object.Instantiate(_chargeParticlePrefab, PController.transform);
                    }
                }
            }
            else
            {
                if (_instancedParticle != null)
                {
                    Object.Destroy(_instancedParticle);
                }
                if (_isChargeSoundPlaying)
                {
                    _audioSource.Stop();
                    _isChargeSoundPlaying = false;
                }
                
                if (IsAttackAnimationFinished())
                {
                    PController.sprite.color = Color.white;
                    PController.StateMachine.ChangeState(PController.StateContainer.PlayerGroundState);
                }
            }
        }

        private bool IsAttackAnimationFinished()
        {
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.IsName("GroundAttackDischarge") && IsCurrentAnimationFinished();
        }

        private bool IsChargeAnimationFinished()
        {
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.IsName("GroundAttackCharge") && IsCurrentAnimationFinished();
        }

        private void OnGroundAttack(InputAction.CallbackContext context)
        {
            return;
        }

        private void OnGroundAttackCanceled(InputAction.CallbackContext context)
        {
            if (!_isCharging)
                return;

            if (_isChargeSoundPlaying)
            {
                _audioSource.Stop();
                _isChargeSoundPlaying = false;
            }

            PController.anim.Play("GroundAttackDischarge");

            var animationLength = PController.GetAnimationLength();
            _hitbox = BatHitboxManager.CreateHitBox(
                PController.gameObject,
                _attackDirection,
                Mathf.Pow(6f, PController.attackCharge) + 0.5f,
                animationLength,
                AttackType.GroundAttack
            );

            // Reproduce el sonido de swing apropiado
            if (PController.attackCharge > _chargeThreshold)
            {
                PlaySwingSound(_strongSwingSound, strongSwingVolume, strongSwingPitch);
                
            }
            else
            {
                PlaySwingSound(_strongSwingSound, strongSwingVolume, strongSwingPitch);
            }

            _attackChargeTime = 0.0f;
            _attackDirection = Vector2.zero;
            _isCharging = false;
        }

        public override void Exit()
        {
            base.Exit();
            Object.Destroy(_hitbox);
            Object.Destroy(_instancedParticle);
            PController.attackCharge = 0;
            PController.PlayerInputActions.Player.Attack.performed -= OnGroundAttack;
            PController.PlayerInputActions.Player.Attack.canceled -= OnGroundAttackCanceled;

            if (_isChargeSoundPlaying)
            {
                _audioSource.Stop();
                _isChargeSoundPlaying = false;
            }
        }

        private float CalculateTimeDifference(float a)
        {
            return Time.time - a;
        }

        public void PlaySwingSound(AudioClip clip, float volume, float pitch, bool loop = false)
        {
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.volume = volume;
                _audioSource.pitch = pitch;
                _audioSource.loop = loop;
                _audioSource.Play();
            }
        }
    }
}