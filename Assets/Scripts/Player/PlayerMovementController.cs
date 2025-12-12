using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Moving,
            Jumping,
            Falling,
            WallSliding
        }

        public State CurrentState => currentState;
        public Vector2 InputMovement => _moveInput;
        public bool IsDoubleJumping => isDoubleJumping;
        
        [Header("Locomotion")] 
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 7.5f;
        [SerializeField] private int jumpCap = 2;
        [SerializeField] private int jumpCounter = 0;
        [SerializeField] private bool isDoubleJumping;

        [Header("Debug")] 
        [SerializeField] private State currentState;

        private Rigidbody2D _rb;
        private InputSystem_Actions _inputActions;
        private Vector2 _moveInput;
        private PlayerCollisionController _playerCollision;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputActions = new InputSystem_Actions();
            _playerCollision = GetComponent<PlayerCollisionController>();

            _inputActions.Player.Move.performed += OnMove;
            _inputActions.Player.Move.canceled += OnMoveCanceled;
            _inputActions.Player.Jump.performed += OnJump;
        }

        private void Update()
        {
            HandlePlayerState();
            HandlePlayerStateTransition();
        }

        private void FixedUpdate()
        {
            if (_playerCollision.IsGrounded && _rb.linearVelocity.y == 0 && jumpCounter > 0) jumpCounter = 0;
            
            if (jumpCounter == 1)
            {
                if (_inputActions.Player.Jump.IsPressed()) isDoubleJumping = true;
            }
            
            if(jumpCounter != 1 && currentState == State.Falling && isDoubleJumping) isDoubleJumping = false;
            
        }

        private void OnEnable() => _inputActions.Player.Enable();
        private void OnDisable() => _inputActions.Player.Disable();

        private void OnDestroy()
        {
            if (_inputActions != null)
            {
                _inputActions.Player.Move.performed -= OnMove;
                _inputActions.Player.Move.canceled -= OnMove;
                _inputActions.Player.Jump.performed -= OnJump;

                _inputActions.Dispose();
            }
        }

        private void HandlePlayerStateTransition()
        {
            if (!_playerCollision.isGrounded)
            {
                if (_rb.linearVelocity.y < 0) currentState = State.Falling;
                return;
            }

            currentState = Mathf.Abs(_moveInput.x) > 0.01f ? State.Moving : State.Idle;
        }

        private void HandlePlayerState()
        {
            switch (currentState)
            {
                case State.Idle:
                    HandleIdleState();
                    break;
                case State.Moving:
                    HandleMovingState();
                    break;
                case State.Jumping:
                    HandleJumpingState();
                    break;
                case State.Falling:
                    HandleFallingState();
                    break;
                case State.WallSliding:
                    break;
            }
        }

        private void HandleFallingState()
        {
            print("Falling state");
        }

        private void HandleJumpingState()
        {
            // Important: do NOT keep forcing y velocity every frame.
            // Jump is applied once in OnJump().
            if (_rb.linearVelocity.y <= 0f) currentState = State.Falling;
        }

        private void HandleMovingState()
        {
            SetLinearVelocity(_moveInput.x * moveSpeed, _rb.linearVelocity.y);
        }

        private void HandleIdleState() => SetLinearVelocity(0f, _rb.linearVelocity.y);

        private void OnMove(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();

        private void OnMoveCanceled(InputAction.CallbackContext ctx) => _moveInput = Vector2.zero;

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (jumpCounter == jumpCap) return;
            
            jumpCounter++;
            SetLinearVelocity(_rb.linearVelocity.x, jumpForce);
            currentState = State.Jumping;
        }

        private void SetLinearVelocity(float x, float y) => _rb.linearVelocity = new Vector2(x, y);
        // private void SetLinearVelocity(Vector2 velocity) => _rb.linearVelocity = velocity;

        public Vector2 GetLinearVelocity() => _rb.linearVelocity;
    }
}
