using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public float FacingDirection => facingDirection;
        
        private static readonly int MoveAnimParam = Animator.StringToHash("xVelocity");
        private static readonly int JumpAnimParam = Animator.StringToHash("yVelocity");
        private static readonly int JumpingAnimParam = Animator.StringToHash("isJumping");
        private static readonly int DoubleJumpAnimParam = Animator.StringToHash("doubleJump");
        
        [Header("Sprite Direction")]
        [SerializeField] private float facingDirection = 1;
        
        private Animator _animator;
        private PlayerMovementController _playerMovement;
        private PlayerCollisionController _playerCollision;
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerMovement = GetComponent<PlayerMovementController>();
            _playerCollision = GetComponent<PlayerCollisionController>();
        }

        private void Update()
        {
            HandlePlayerSpriteDirection((int)_playerMovement.InputMovement.x);
            HandlePlayerAnimEvents();
        }

        private void HandlePlayerAnimEvents()
        {
            _animator.SetFloat(MoveAnimParam, _playerMovement.InputMovement.x);
            _animator.SetFloat(JumpAnimParam, _playerMovement.GetLinearVelocity().y);
            
            _animator.SetBool(JumpingAnimParam, !_playerCollision.IsGrounded);
            if (_playerMovement.IsDoubleJumping) _animator.SetTrigger(DoubleJumpAnimParam);
        }

        private void HandlePlayerSpriteDirection(int x)
        {
            if (x == 0) return;
            facingDirection = x;
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }
}
