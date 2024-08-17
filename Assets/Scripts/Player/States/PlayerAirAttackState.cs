using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.States
{
    public class PlayerAirAttackState : PlayerState
    {
        private GameObject _hitbox;
        private PlayerHitbox _hitboxScript;
        private bool _isCharging;
        private Vector2 _attackDirection;
        
        public PlayerAirAttackState(PlayerController pController) : base(pController)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            _attackDirection = PController.PlayerInputActions.Player.Move.ReadValue<Vector2>();
            
            PController.anim.Play("Attacking");
            PController.anim.speed = 1;
            CreateHitBox();
        }
        
        public override void Update()
        {
            base.Update();
            PController.rb.velocity = new Vector2(
                PController.rb.velocity.x,
                PController.rb.velocity.y
            );
            
            if (IsAttackAnimationFinished())
            {
                PController.StateMachine.ChangeState(PController.StateContainer.PlayerAirState);
            }
        }

        private bool IsAttackAnimationFinished()
        {
            var animState = PController.anim.GetCurrentAnimatorStateInfo(0);
            return animState.normalizedTime >= 1.0f;
        }
        
        private void CreateHitBox()
        {
            _hitbox = new GameObject("HitBox");
            _hitbox.transform.parent = PController.transform;
            _hitbox.transform.localPosition = new Vector3(0, 0, 0);
            var hitboxCollider = _hitbox.AddComponent<BoxCollider2D>();
            hitboxCollider.isTrigger = true;
            
            hitboxCollider.size = new Vector2(2.5f, 1.5f);
            hitboxCollider.offset = new Vector2(0.5f, 0);

            _hitboxScript = _hitbox.AddComponent<PlayerHitbox>();
            _hitboxScript.damage = 4 * PController.attackCharge;
            _hitboxScript.hitBoxDirection = _attackDirection;
        }
        
        public override void Exit()
        {
            base.Exit();
            Object.Destroy(_hitbox);
            Object.Destroy(_hitboxScript);
        }
       
    }
}