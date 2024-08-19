using UnityEngine;

namespace Player.States
{
    public class PlayerAirAttackState : PlayerState
    {
        private GameObject _hitbox;
        private bool _isCharging;
        private Vector2 _attackDirection;

        private AudioClip _weakSwingSound;

        private AudioSource _audioSource;

        // Volume and pitch settings
        public float weakSwingVolume = 0.1f;
        public float weakSwingPitch = 1.1f;

        public PlayerAirAttackState(PlayerController pController) : base(pController)
        {
            _weakSwingSound = Resources.Load<AudioClip>("weak");
            _audioSource = PController.gameObject.AddComponent<AudioSource>();
        }

        public override void Enter()
        {
            base.Enter();
            _attackDirection = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();

            PController.anim.Play("JumpAttack");
            PlaySwingSound(_weakSwingSound, weakSwingVolume, weakSwingPitch);
            var animationLength = PController.GetAnimationLength();
            Debug.Log("JumpAttack animation length: " + animationLength);
            //CreateHitBox();
            _hitbox = BatHitboxManager.CreateHitBox(
                PController.gameObject,
                _attackDirection,
                1.5f,
                animationLength,
                AttackType.AerialAttack
            );
        }

        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(
                PController.rb.velocity.x,
                PController.rb.velocity.y
            );

            if (IsAttackAnimationFinished())
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerAirState);
            }

            if (PController.IsGrounded())
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerLandingState);
            }
        }

        private bool IsAttackAnimationFinished()
        {
            // Check if the current animation statename is "GroundAttackDischarge"
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.IsName("JumpAttack") && IsCurrentAnimationFinished();
        }


        public override void Exit()
        {
            base.Exit();
            Object.Destroy(_hitbox);
        }

        public void PlaySwingSound(AudioClip clip, float volume, float pitch)
        {
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.volume = volume;
                _audioSource.pitch = pitch;
                _audioSource.Play();
            }
        }

    }
}
