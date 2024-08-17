using StateMachine;
using UnityEngine;

namespace Player.States
{
    public class PlayerState : IState
    {
        protected PlayerController PController;
        protected Vector2 MovementInput;
        
        public PlayerState(PlayerController pController)
        {
            PController = pController;
        }

        public string GetName()
        {
            return GetType().Name;
        }
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {}
        
    }
}