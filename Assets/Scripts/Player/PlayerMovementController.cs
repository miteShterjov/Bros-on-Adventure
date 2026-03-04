using System.Collections;
using GameEffects;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        public enum State
        {
            Idle, Moving, Jumping, Falling, WallSliding
        }

        public State CurrentState => currentState;
        
        public Vector2 InputMovement => _moveInput;
        
        public bool PlayerIsKnocked => _knockback.IsKnocked;

        public bool ConsumeDoubleJump()
        {
            if (!isDoubleJumping) return false;
            isDoubleJumping = false;
            return true;
        }

        [Header("Locomotion")] 
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 7.5f;
        [SerializeField] private int jumpCap = 2;
        [SerializeField] private int jumpCounter;
        [SerializeField] private bool isDoubleJumping;

        [Space][Header("Air Control")] 
        [SerializeField] private float airMoveSpeedMultiplier = 1.0f;

        [Space][Header("Wall Jump")] 
        [SerializeField] private float wallJumpHorizontalSpeed = 5f;
        [SerializeField] private float wallJumpLockDuration = 0.12f;

        [Space][Header("Jump Buffer")] 
        [SerializeField] private float jumpBufferTime = 0.12f;
        [SerializeField] private float jumpBufferTimer;

        [Space][Header("Debug")] [SerializeField] private State currentState;

        private Rigidbody2D _rb;
        private InputSystem_Actions _inputActions;
        private Vector2 _moveInput;
        private PlayerCollisionController _playerCollision;
        private PlayerAnimationController _playerAnim;
        private KnockbackVFX _knockback;
        private float _wallJumpLockTimer;

        private bool _inputsEnabled;
        private System.Action<InputAction.CallbackContext> _onTestPerformed;

        private void Awake()
        {
            currentState = State.Idle;
            _rb = GetComponent<Rigidbody2D>();
            _playerCollision = GetComponent<PlayerCollisionController>();
            _playerAnim = GetComponent<PlayerAnimationController>();
            _knockback = GetComponent<KnockbackVFX>();
            _inputActions = new InputSystem_Actions();

            _inputActions.Player.Move.performed += OnMove;
            _inputActions.Player.Move.canceled += OnMoveCanceled;
            _inputActions.Player.Jump.performed += OnJump;

            _onTestPerformed = _ => TestingDebugPlayerDies();
            _inputActions.Player.Test.performed += _onTestPerformed;
        }

        private void Update()
        {
            if (PlayerIsKnocked) return;

            bool isRespawning = GameManager.Instance.IsRespawning;

            if (isRespawning)
            {
                if (_inputsEnabled)
                {
                    _inputActions.Player.Disable();
                    _inputsEnabled = false;
                }

                _moveInput = Vector2.zero;
                SetLinearVelocity(0, _rb.linearVelocity.y);
                return;
            }

            if (!_inputsEnabled)
            {
                _inputActions.Player.Enable();
                _inputsEnabled = true;
            }

            if (jumpBufferTimer > 0f) jumpBufferTimer -= Time.deltaTime;
            if (_wallJumpLockTimer > 0f) _wallJumpLockTimer -= Time.deltaTime;

            TryConsumeBufferedJump();

            HandlePlayerStateTransition();
        }

        private void FixedUpdate()
        {
            if (_playerCollision.IsGrounded)
            {
                float vy = _rb.linearVelocity.y;
                if (Mathf.Abs(vy) < 0.01f && jumpCounter > 0)
                    jumpCounter = 0;
            }

            if (currentState != State.WallSliding)
                _rb.gravityScale = 1f;

            HandlePlayerState();
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();
            _inputsEnabled = true;
        }

        private void OnDisable()
        {
            _inputActions.Player.Disable();
            _inputsEnabled = false;
        }

        private void OnDestroy()
        {
            if (_inputActions != null)
            {
                _inputActions.Player.Move.performed -= OnMove;
                _inputActions.Player.Move.canceled -= OnMoveCanceled;
                _inputActions.Player.Jump.performed -= OnJump;

                if (_onTestPerformed != null)
                    _inputActions.Player.Test.performed -= _onTestPerformed;

                _inputActions.Dispose();
            }
        }

        private void HandlePlayerStateTransition()
        {
            bool grounded = _playerCollision.IsGrounded;
            bool wallDetected = _playerCollision.IsWallDetected;

            float vy = _rb.linearVelocity.y;

            bool isFalling = vy < -0.01f;
            bool isRising = vy > 0.01f;

            // 1) Grounded resolves everything else
            if (grounded)
            {
                currentState = Mathf.Abs(_moveInput.x) > 0.01f ? State.Moving : State.Idle;
                return;
            }

            // IMPORTANT: after a wall jump, don't immediately re-enter WallSliding.
            if (_wallJumpLockTimer > 0f)
            {
                if (isFalling)
                {
                    currentState = State.Falling;
                    return;
                }

                if (isRising) currentState = State.Jumping;
                return;
            }

            // 2) Airborne: WallSlide has priority over Falling
            if (isFalling && wallDetected)
            {
                currentState = State.WallSliding;
                return;
            }

            if (isFalling)
            {
                currentState = State.Falling;
                return;
            }

            // 3) Optional: if you want "upward airtime" to always be Jumping
            if (isRising) currentState = State.Jumping;

            // Otherwise: keep the currentState (prevents state flicker when vy ~ 0 midair)
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
                    HandleWallSlidingState();
                    break;
                default:
                    HandleIdleState();
                    break;
            }
        }

        private void HandleWallSlidingState()
        {
            _rb.gravityScale = 0.1f;

            // Allow wall jump even if we previously used up jumps in the air.
            // This is classic behavior: touching a wall "refreshes" your jump options.
            if (jumpCounter != 0) jumpCounter = 0;

            if (isDoubleJumping) isDoubleJumping = false;

            if ((int)_moveInput.x != 0 && (int)_moveInput.x != _playerAnim.FacingDirection)
                currentState = State.Idle;

            // Wall jump is handled in OnJump() so it can't double-trigger.
            // if (_inputActions.Player.Jump.WasPressedThisFrame()) OnWallJump();
        }

        private void HandleFallingState()
        {
            float desiredX = _moveInput.x * moveSpeed * airMoveSpeedMultiplier;

            bool pressingIntoWall =
                _wallJumpLockTimer <= 0f &&
                _playerCollision.IsWallDetected &&
                Mathf.Abs(_moveInput.x) > 0.01f &&
                (int)Mathf.Sign(_moveInput.x) == _playerCollision.WallDirection;

            if (pressingIntoWall)
                desiredX = 0f;

            SetLinearVelocity(desiredX, _rb.linearVelocity.y);
        }

        private void HandleJumpingState()
        {
            float desiredX = _moveInput.x * moveSpeed * airMoveSpeedMultiplier;

            bool pressingIntoWall =
                _wallJumpLockTimer <= 0f &&
                _playerCollision.IsWallDetected &&
                Mathf.Abs(_moveInput.x) > 0.01f &&
                (int)Mathf.Sign(_moveInput.x) == _playerCollision.WallDirection;

            if (pressingIntoWall)
                desiredX = 0f;

            SetLinearVelocity(desiredX, _rb.linearVelocity.y);

            if (_rb.linearVelocity.y <= 0f) currentState = State.Falling;
        }

        private void HandleMovingState()
        {
            float desiredX = _moveInput.x * moveSpeed;

            // If a wall is detected and the player is pressing INTO it, stop horizontal movement.
            // This keeps input active (so your animation can stay "moving") while the velocity is 0.
            bool pressingIntoWall =
                _playerCollision.IsWallDetected &&
                Mathf.Abs(_moveInput.x) > 0.01f &&
                (int)Mathf.Sign(_moveInput.x) == _playerCollision.WallDirection;

            if (pressingIntoWall)
                desiredX = 0f;

            SetLinearVelocity(desiredX, _rb.linearVelocity.y);
        }

        private void HandleIdleState() => SetLinearVelocity(0f, _rb.linearVelocity.y);

        private void OnMove(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();

        private void OnMoveCanceled(InputAction.CallbackContext ctx) => _moveInput = Vector2.zero;

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (PlayerIsKnocked) return;

            // Buffer the jump input, then try to consume it immediately (feels responsive),
            // or later (when you land) within jumpBufferTime.
            jumpBufferTimer = jumpBufferTime;
            TryConsumeBufferedJump();
        }

        private void TryConsumeBufferedJump()
        {
            if (jumpBufferTimer <= 0f) return;

            // If we're wall sliding, a buffered jump should become a wall jump.
            if (currentState == State.WallSliding)
            {
                if (jumpCounter == jumpCap) return;
                jumpBufferTimer = 0f;
                OnWallJump();
                return;
            }

            if (jumpCounter == jumpCap) return;

            // If we're in the air and still have jumps left, allow the buffered press
            // to act immediately as a (double) jump too.
            // If you ONLY want the buffering for "press before landing", change this to require grounded.
            jumpBufferTimer = 0f;

            jumpCounter++;

            if (jumpCounter == 2)
                isDoubleJumping = true;

            SetLinearVelocity(_rb.linearVelocity.x, jumpForce);
            currentState = State.Jumping;
        }

        private void OnWallJump()
        {
            if (jumpCounter == jumpCap) return;

            // Push away from the wall you're sliding on.
            // FacingDirection points into the wall while sliding in your setup, so negate it.
            float jumpX = -_playerAnim.FacingDirection * wallJumpHorizontalSpeed;

            // Start lockout so you don't immediately re-stick/re-enter at wall slide or get X zeroed.
            _wallJumpLockTimer = wallJumpLockDuration;

            SetLinearVelocity(jumpX, jumpForce);
            currentState = State.Jumping;

            jumpCounter++;
        }

        private void SetLinearVelocity(float x, float y) => _rb.linearVelocity = new Vector2(x, y);
        // private void SetLinearVelocity(Vector2 velocity) => _rb.linearVelocity = velocity;

        public Vector2 GetLinearVelocity() => _rb.linearVelocity;

        private void TestingDebugPlayerDies()
        {
            _playerAnim.DestroyPlayerAnimEvent();
        }

        public void PushPlayer(Vector2 direction, float duration = 0) =>
            StartCoroutine(PlayerPushCo(direction, duration));

        private IEnumerator PlayerPushCo(Vector2 direction, float duration)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(direction, ForceMode2D.Impulse);

            yield return new WaitForSeconds(duration);
        }

        public bool PlayerInteracts() => _inputActions.Player.Interact.IsPressed();
    }
}