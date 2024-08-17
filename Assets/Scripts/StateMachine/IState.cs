namespace StateMachine
{
    public interface IState
    {
        public string GetName();
        public void Enter();
        public void Exit();
        public void Update();
    }
}