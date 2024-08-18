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
            
            PController.anim.Play("GroundAttackCharge");
        }
        
        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(0,0);
            if (!_isCharging && IsAttackAnimationFinished())
            {
                // Restore sprite color
                PController.sprite.color = Color.white;
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerGroundState);
            }
            else if (_isCharging)
            {
                PController.attackCharge = Mathf.Min(CalculateTimeDifference(_attackChargeTime), 1.0f);
                // If the charge is higher than 0.4, slowly start coloring the sprite to yellow
                if (IsChargeAnimationFinished() && PController.attackCharge > 0)
                {
                    PController.sprite.color = Color.Lerp(Color.white, Color.yellow, PController.attackCharge);
                }
            }
        }

        private bool IsAttackAnimationFinished()
        {
            // Check if the current animation statename is "GroundAttackDischarge"
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.IsName("GroundAttackDischarge") && IsCurrentAnimationFinished();
        }
        
        private bool IsChargeAnimationFinished()
        {
            // Check if the current animation statename is "GroundAttackDischarge"
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.IsName("GroundAttackCharge") && IsCurrentAnimationFinished();
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
            
            PController.anim.Play("GroundAttackDischarge");
            var animationLength = PController.GetAnimationLength();
            Debug.Log("Ground attack length: " + animationLength);
            _attackDirection = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
            _hitbox = BatHitboxManager.CreateHitBox(
                PController.gameObject,
                _attackDirection,
                4 * PController.attackCharge,
                animationLength,
                AttackType.GroundAttack
            );
            
            _attackChargeTime = 0.0f;
            _attackDirection = Vector2.zero;
            _isCharging = false;
        }

        public override void Exit()
        {
            base.Exit();
            Object.Destroy(_hitbox);
            PController.attackCharge = 0;
            PController.PlayerInputActions.Player.Attack.performed -= OnGroundAttack;
            PController.PlayerInputActions.Player.Attack.canceled -= OnGroundAttackCanceled;
        }
        
        private float CalculateTimeDifference(float a)
        {
            return Time.time - a;
        }
    }
}