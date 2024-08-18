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
        
        protected bool IsCurrentAnimationFinished()
        {
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.normalizedTime >= 1.0f;
        }
        
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {}
        
    }
}