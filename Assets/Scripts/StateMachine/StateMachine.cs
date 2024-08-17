namespace StateMachine
{
    public class StateMachine
    {
        private IState _currentState;

        public string GetCurrentStateName()
        {
            return _currentState.GetName() ?? "No Valid State";
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }
        
        public void ChangeState(IState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState.Enter();
        }
        
        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.Update();
            }
        }
    }
}