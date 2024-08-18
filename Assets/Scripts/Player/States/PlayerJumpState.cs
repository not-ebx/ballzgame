using UnityEngine;

namespace Player.States
{
    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerController pController) : base(pController)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            // Set the movement input to the current one
            PController.movementInput = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
            PController.anim.Play("JumpStart");
        }

        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(
                PController.rb.velocity.x,
                PController.jumpVelocity
            );
            if (IsCurrentAnimationFinished())
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}