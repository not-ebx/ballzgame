using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerGroundState: PlayerState 
    {
        public PlayerGroundState(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerController.RestartRemainingJumps();
            PlayerController.PlayerInputActions.Player.Move.performed += PlayerController.OnMove;
            PlayerController.PlayerInputActions.Player.Move.canceled  += PlayerController.OnMoveCanceled;
            PlayerController.PlayerInputActions.Player.Jump.performed += PlayerController.OnJump;
            PlayerController.PlayerInputActions.Player.Jump.canceled  += PlayerController.OnJumpCanceled; 
        }

        public override void Update()
        {
            base.Update();
            PlayerController.rb.velocity = new Vector2(
                PlayerController.movementInput.x * PlayerController.moveSpeed,
                PlayerController.rb.velocity.y
            );
            
            // Check if it should change state to jumping or falling
            if (!PlayerController.IsGrounded())
            {
                PlayerController.StateMachine.ChangeState(PlayerController.StateContainer.PlayerAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            PlayerController.PlayerInputActions.Player.Move.performed -= PlayerController.OnMove;
            PlayerController.PlayerInputActions.Player.Move.canceled  -= PlayerController.OnMoveCanceled;
            PlayerController.PlayerInputActions.Player.Jump.performed -= PlayerController.OnJump; 
            PlayerController.PlayerInputActions.Player.Jump.canceled  -= PlayerController.OnJumpCanceled; 
        }
        
    }
}