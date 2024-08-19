using UnityEngine;

namespace Player.States
{
    public class PlayerLandingState : PlayerState
    {
        public PlayerLandingState(PlayerController pController) : base(pController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PController.anim.Play("JumpLanding");
        }

        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(
                0,
                PController.rb.velocity.y
            );

            PController.StateMachine.ChangeState(PController.StateContainer.PlayerGroundState);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}