using StateMachine;
using UnityEngine;

namespace Player.States
{
    public class PlayerState : IState
    {
        protected PlayerController PlayerController;
        protected Vector2 MovementInput;
        
        public PlayerState(PlayerController playerController)
        {
            PlayerController = playerController;
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