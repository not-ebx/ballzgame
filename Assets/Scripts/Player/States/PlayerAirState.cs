using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerAirState: PlayerState 
    {
        private Vector2 _jumpDirection;
        private float _smoothFactor = 0.01f;
        
        public PlayerAirState(PlayerController pController) : base(pController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PController.PlayerInputActions.Player.Move.performed += OnAirStateMove;
            PController.PlayerInputActions.Player.Move.canceled  += PController.OnMoveCanceled;
            PController.PlayerInputActions.Player.Jump.performed += OnAirStateJump;
            PController.PlayerInputActions.Player.Jump.canceled  += PController.OnJumpCanceled; 
            
            PController.PlayerInputActions.Player.Attack.performed += OnAirAttack;
            
            // Set the movement input to the current one
            PController.movementInput = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
        }

        public override void Update()
        {
            base.Update();
            var targetVelocityX = PController.movementInput.x * PController.moveSpeed;
            
            PController.rb.velocity = new Vector2(
                Mathf.Lerp(PController.rb.velocity.x, targetVelocityX, _smoothFactor),
                PController.rb.velocity.y
            );
            _smoothFactor = 0.01f;

            if (PController.IsJumping())
            {
                PController.anim.Play("JumpUp");
            }
            else
            {
                PController.anim.Play("JumpFalling");
            }

            if (PController.IsGrounded() && PController.rb.velocity.y <= 0)
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerLandingState);
            }
        }

        private void OnAirStateMove(InputAction.CallbackContext context)
        {
            var movementVector = context.ReadValue<Vector2>();
            PController.movementInput.x = movementVector.x;
        }
        
        private void OnAirStateJump(InputAction.CallbackContext context)
        {
            PController.movementInput.x = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>().x;
            _smoothFactor = 1;
            
            // Set the direction of the new jump with OnMove
            PController.OnJump(context);
            PController.remainingJumps--;
        }

        public override void Exit()
        {
            base.Exit();
            PController.PlayerInputActions.Player.Move.performed -= OnAirStateMove;
            PController.PlayerInputActions.Player.Move.canceled  -= PController.OnMoveCanceled;
            PController.PlayerInputActions.Player.Jump.performed -= OnAirStateJump; 
            PController.PlayerInputActions.Player.Jump.canceled  -= PController.OnJumpCanceled; 
            PController.PlayerInputActions.Player.Attack.performed -= OnAirAttack;
        }

        private void OnAirAttack(InputAction.CallbackContext context)
        {
            if (PController.remainingAerial <= 0)
                return;
            PController.remainingAerial--;
            PController.StateMachine.ChangeState(PController.StateContainer.PlayerAirAttackState);
        }
        
    }
}