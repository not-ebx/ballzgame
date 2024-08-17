using System;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 15f;
        public float jumpForce = 8f;
        public float gravityForce = -4f;
        public LayerMask groundLayer;
        
        public Rigidbody2D rb;
        public BoxCollider2D coll;
        
        public StateMachine.StateMachine StateMachine;
        public StateContainer StateContainer;
        
        private PlayerInput _playerInput;
        public PlayerInputActions PlayerInputActions;

        public int remainingJumps = 1; // Doesn't count the ground one.
        public float maxJumpHeight = 4f;
        public float minJumpHeight = 1f;
        public float timeToMaxJump = 0.7f;
        public float jumpVelocity;
        public float gravityScale;
        public Vector2 movementInput;
        private bool _isGrounded;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();

            PlayerInputActions = new PlayerInputActions();
            StateContainer = new StateContainer(this);
        }

        public void RestartRemainingJumps()
        {
            remainingJumps = 1;
        }
        
        private void Start()
        {
            StateMachine = new StateMachine.StateMachine();
            StateMachine.ChangeState(StateContainer.PlayerGroundState);
            
            // Set up jump physics
            gravityScale = (2 * maxJumpHeight) / Mathf.Pow(timeToMaxJump, 2);
            rb.gravityScale = gravityScale;

            jumpVelocity = Mathf.Sqrt(2 * gravityScale * Mathf.Abs(Physics2D.gravity.y) * maxJumpHeight);
        }

        public bool IsJumping()
        {
            return rb.velocity.y > 0;
        }

        public bool IsFalling()
        {
            return rb.velocity.y < 0;
        }
        
        private void OnEnable()
        {
            PlayerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            PlayerInputActions.Player.Disable();
        }
        
        public bool IsGrounded()
        {
            return _isGrounded;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _isGrounded = false;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            CheckCollisionSide(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CheckCollisionSide(collision);
        }

        private void CheckCollisionSide(Collision2D collision)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 contactPoint = contact.point;
                Vector2 center = coll.bounds.center;

                // Calculate the difference between the contact point and the center of the collider
                Vector2 difference = contactPoint - center;

                // Check if the collision is on the bottom side
                if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
                {
                    if (difference.y < 0)
                    {
                        _isGrounded = true;
                        break; // If one contact is on the bottom, we can safely say the player is grounded
                    }
                }
            }
        }
        

        private void Update()
        {
            StateMachine.Update();
        }
        
        /*
         * Common Movement Logic
         */
        public void OnMove(InputAction.CallbackContext context)
        {
            var movementVector = context.ReadValue<Vector2>();
            movementInput.x = movementVector.x;
        }
        
        public void OnMoveCanceled(InputAction.CallbackContext context)
        {
            movementInput.x = 0f;
        }

        private bool CanJump()
        {
            return IsGrounded() || remainingJumps > 0;
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (CanJump())
            {
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    jumpVelocity
                );
            }
        }
        
        public void OnJumpCanceled(InputAction.CallbackContext context)
        {
            if (IsJumping())
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0);
            }
        }
    }
}