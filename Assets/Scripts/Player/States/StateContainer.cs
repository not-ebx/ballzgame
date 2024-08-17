namespace Player.States
{
    public class StateContainer
    {
        public PlayerGroundState PlayerGroundState;
        public PlayerAirState PlayerAirState;

        public StateContainer(PlayerController playerController)
        {
            PlayerGroundState = new PlayerGroundState(playerController);
            PlayerAirState = new PlayerAirState(playerController);
        }
    }
}