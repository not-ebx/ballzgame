using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerGroundState: PlayerState
    {

        
        public PlayerGroundState(PlayerController pController) : base(pController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PController.movementInput = Vector2.zero;
            PController.RestartRemainingJumps();
            PController.RestartRemainingAerial();
            
            PController.PlayerInputActions.Player.Move.performed += PController.OnMove;
            PController.PlayerInputActions.Player.Move.canceled  += PController.OnMoveCanceled;
            PController.PlayerInputActions.Player.Jump.performed += OnGroundJump;
            
            PController.PlayerInputActions.Player.Attack.performed += OnGroundAttack;
            
            // Set the movement input to the current one
            PController.movementInput = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
            
        }

        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(
                PController.movementInput.x * PController.moveSpeed,
                PController.rb.velocity.y
            );
            PController.anim.Play(PController.movementInput.x != 0 ? "Running" : "Idle");
            
            // Check if it should change state to jumping or falling
            if (!PController.IsGrounded())
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerAirState);
            }
            
        }

        public override void Exit()
        {
            base.Exit();
            PController.PlayerInputActions.Player.Move.performed   -= PController.OnMove;
            PController.PlayerInputActions.Player.Move.canceled    -= PController.OnMoveCanceled;
            PController.PlayerInputActions.Player.Jump.performed   -= OnGroundJump; 
            PController.PlayerInputActions.Player.Attack.performed -= OnGroundAttack;
        }
        
        private void OnGroundJump(InputAction.CallbackContext context)
        {
            PController.StateMachine.ChangeState(PController.StateContainer.PlayerJumpState);
        }
        
        public void OnGroundAttack(InputAction.CallbackContext context)
        {
            PController.StateMachine.ChangeState(PController.StateContainer.PlayerGroundAttackState);
        }


        
    }
}