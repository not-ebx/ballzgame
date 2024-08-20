namespace Player.States
{
    public class StateContainer
    {
        public PlayerGroundState PlayerGroundState;
        public PlayerAirState PlayerAirState;
        public PlayerGroundAttackState PlayerGroundAttackState;
        public PlayerAirAttackState PlayerAirAttackState;
        public PlayerJumpState PlayerJumpState;
        public PlayerLandingState PlayerLandingState;
        public PlayerState PlayerStunnedState { get; private set; }

        public StateContainer(PlayerController playerController)
        {
            PlayerGroundState = new PlayerGroundState(playerController);
            PlayerAirState = new PlayerAirState(playerController);
            PlayerGroundAttackState = new PlayerGroundAttackState(playerController);
            PlayerAirAttackState = new PlayerAirAttackState(playerController);
            PlayerJumpState = new PlayerJumpState(playerController);
            PlayerLandingState = new PlayerLandingState(playerController);
            PlayerStunnedState = new PlayerStunnedState(playerController);
        }
    }
}