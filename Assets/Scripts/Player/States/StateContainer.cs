namespace Player.States
{
    public class StateContainer
    {
        public PlayerGroundState PlayerGroundState;
        public PlayerAirState PlayerAirState;
        public PlayerGroundAttackState PlayerGroundAttackState;
        public PlayerAirAttackState PlayerAirAttackState;

        public StateContainer(PlayerController playerController)
        {
            PlayerGroundState = new PlayerGroundState(playerController);
            PlayerAirState = new PlayerAirState(playerController);
            PlayerGroundAttackState = new PlayerGroundAttackState(playerController);
            PlayerAirAttackState = new PlayerAirAttackState(playerController);
        }
    }
}