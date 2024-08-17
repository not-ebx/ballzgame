using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerAirState: PlayerState 
    {
        public PlayerAirState(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerController.PlayerInputActions.Player.Move.performed += OnAirStateMove;
            PlayerController.PlayerInputActions.Player.Move.canceled  += PlayerController.OnMoveCanceled;
            PlayerController.PlayerInputActions.Player.Jump.performed += OnAirStateJump;
            PlayerController.PlayerInputActions.Player.Jump.canceled  += PlayerController.OnJumpCanceled; 
        }

        public override void Update()
        {
            base.Update();
            PlayerController.rb.velocity = new Vector2(
                PlayerController.movementInput.x * PlayerController.moveSpeed,
                PlayerController.rb.velocity.y
            );
            
            if (PlayerController.IsGrounded())
            {
                PlayerController.StateMachine.ChangeState(PlayerController.StateContainer.PlayerGroundState);
            }
        }

        private void OnAirStateMove(InputAction.CallbackContext context)
        {
            var movementVector = context.ReadValue<Vector2>();
            PlayerController.movementInput.x = movementVector.x * 0.5f;
        }
        
        private void OnAirStateJump(InputAction.CallbackContext context)
        {
            PlayerController.OnJump(context);
            PlayerController.remainingJumps--;
        }

        public override void Exit()
        {
            base.Exit();
            PlayerController.PlayerInputActions.Player.Move.performed -= OnAirStateMove;
            PlayerController.PlayerInputActions.Player.Move.canceled  -= PlayerController.OnMoveCanceled;
            PlayerController.PlayerInputActions.Player.Jump.performed -= OnAirStateJump; 
            PlayerController.PlayerInputActions.Player.Jump.canceled  -= PlayerController.OnJumpCanceled; 
        }
        
    }
}