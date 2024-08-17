using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerGroundAttackState : PlayerState
    {
        private GameObject _hitbox;
        private PlayerHitbox _hitboxScript;
        private bool _isCharging;
        private float _attackChargeTime;
        private Vector2 _attackDirection;
        
        public PlayerGroundAttackState(PlayerController pController) : base(pController)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            _attackChargeTime = 0f;
            PController.PlayerInputActions.Player.Attack.performed += OnGroundAttack;
            PController.PlayerInputActions.Player.Attack.canceled += OnGroundAttackCanceled;
            
            // Get the charge time start, to compare on Canceled for the total time
            _attackChargeTime = Time.time;
            _isCharging = true;
            
            PController.anim.Play("Attacking");
            PController.anim.speed = 0;
        }
        
        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(0,0);
            if (!_isCharging && IsAttackAnimationFinished())
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerGroundState);
            }
            else if (_isCharging)
            {
                PController.attackCharge = Mathf.Min(CalculateTimeDifference(_attackChargeTime), 1.0f);
            }
        }

        private bool IsAttackAnimationFinished()
        {
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.normalizedTime >= 1.0f;
        }
        
        // Custom Movements
        private void OnGroundAttack(InputAction.CallbackContext context)
        {
            return;
        }
        
        private void OnGroundAttackCanceled(InputAction.CallbackContext context)
        {
            if (!_isCharging)
                return;
            
            _attackDirection = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
            CreateHitBox();
            _attackChargeTime = 0.0f;
            _attackDirection = Vector2.zero;
            _isCharging = false;
            PController.anim.Play("Attacking");
            PController.anim.speed = 1;
        }

        private void CreateHitBox()
        {
            _hitbox = new GameObject("HitBox");
            _hitbox.transform.parent = PController.transform;
            _hitbox.transform.localPosition = new Vector3(0, 0, 0);
            var hitboxCollider = _hitbox.AddComponent<BoxCollider2D>();
            hitboxCollider.isTrigger = true;
            
            hitboxCollider.size = new Vector2(2.5f, 1.5f);
            hitboxCollider.offset = new Vector2(0.5f, 0);

            _hitboxScript = _hitbox.AddComponent<PlayerHitbox>();
            _hitboxScript.damage = 4 * PController.attackCharge;
            _hitboxScript.hitBoxDirection = _attackDirection;
        }
        
        public override void Exit()
        {
            base.Exit();
            Object.Destroy(_hitbox);
            Object.Destroy(_hitboxScript);
            PController.PlayerInputActions.Player.Attack.performed -= OnGroundAttack;
            PController.PlayerInputActions.Player.Attack.canceled -= OnGroundAttackCanceled;
        }
        
        private float CalculateTimeDifference(float a)
        {
            return Time.time - a;
        }
    }
}