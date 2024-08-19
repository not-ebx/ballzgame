using System;
using System.Collections.Generic;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Public but not serializable
        
        [NonSerialized] public Rigidbody2D rb;
        [NonSerialized] public BoxCollider2D coll;
        [NonSerialized] public Animator anim;
        [NonSerialized] public SpriteRenderer sprite;
        [NonSerialized] public Camera MainCamera;
        private LayerMask _groundLayer;
        private float _raycastDistance = 0.1f;
        
        
        public StateMachine.StateMachine StateMachine;
        public StateContainer StateContainer;
        
        private PlayerInput _playerInput;
        public PlayerInputActions PlayerInputActions;
        
        public float moveSpeed = 8f;
        public int remainingJumps = 1; // Doesn't count the ground one.
        public int remainingAerial = 1;
        public float maxJumpHeight = 4f;
        public float minJumpHeight = 1f;
        public float timeToMaxJump = 0.7f;
        public float jumpVelocity;
        public float gravityScale;
        public Vector2 movementInput;
        private bool _isGrounded;
        
        // Attack Stuff
        public float attackCharge = 0.0f;
        
        // Score
        [NonSerialized] public int Score = 0;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();

            PlayerInputActions = new PlayerInputActions();
            StateContainer = new StateContainer(this);
            
            // Find the main camera in the scene, but make it writable
            MainCamera = Camera.main;
            if (MainCamera == null)
            {
                Debug.LogError("No main camera found in the scene");
            }
            
            _groundLayer = LayerMask.GetMask("Default");
        }

        public void RestartRemainingJumps()
        {
            remainingJumps = 1;
        }
        
        public void RestartRemainingAerial()
        {
            remainingAerial = 1;
        }
        
        private void Start()
        {
            StateMachine = new StateMachine.StateMachine();
            StateMachine.ChangeState(StateContainer.PlayerGroundState);
            
            // Set up jump physics
            gravityScale = -(2 * maxJumpHeight) / (timeToMaxJump*timeToMaxJump);
            jumpVelocity = Mathf.Abs(gravityScale) * timeToMaxJump;
            
            //gravityScale = (2 * maxJumpHeight) / Mathf.Pow(timeToMaxJump, 2);
            rb.gravityScale = gravityScale / Physics2D.gravity.y;

            //jumpVelocity = Mathf.Sqrt(2 * gravityScale * Mathf.Abs(Physics2D.gravity.y) * maxJumpHeight);
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
            var bounds = coll.bounds;
            Vector2 raycastOrigin = new Vector2(bounds.center.x, bounds.min.y);

            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, _raycastDistance, _groundLayer);

            Debug.DrawRay(raycastOrigin, Vector2.down * _raycastDistance, Color.red);

            // Return true if the ray hit something
            return hit.collider != null;
        }

        private void Update()
        {
            StateMachine.Update();
            // Depending on the x velocity, flip the sprite, but preserve the scale
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
            }
            else if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(
                    Mathf.Abs(transform.localScale.x) * -1,
                    transform.localScale.y,
                    transform.localScale.z
                );
            }

            if (false && MainCamera)
            {
                MainCamera.transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    MainCamera.transform.position.z
                );
            }
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
        
        /*
         * Animation Helper Tools
         */
        public float GetAnimationLength()
        {
            var clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            var currentClip = clipInfo[0].clip;
            // Print the name of animation
            Debug.Log("Current animation: " + currentClip.name);
            
            // Get the length of the animation in seconds
            return currentClip.length;
        }
        
        public float GetAnimationLengthByName(string animationName)
        {
            var clip = anim.runtimeAnimatorController.animationClips;
            foreach (var animationClip in clip)
            {
                if (animationClip.name == animationName)
                {
                    return animationClip.length;
                }
            }
            return 0;
        }

    }
}