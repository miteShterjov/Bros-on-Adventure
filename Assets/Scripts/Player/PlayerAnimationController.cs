using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public int FacingDirection => facingDirection;
        
        private static readonly int MoveAnimParam = Animator.StringToHash("xVelocity");
        private static readonly int JumpAnimParam = Animator.StringToHash("yVelocity");
        private static readonly int JumpingAnimParam = Animator.StringToHash("isJumping");
        private static readonly int DoubleJumpAnimParam = Animator.StringToHash("doubleJump");
        private static readonly int WallSlideAnimParam = Animator.StringToHash("isWallSliding");
        private static readonly int PlayerHurtAnimParam = Animator.StringToHash("isHurt");
        private static readonly int RevivedAnimParam = Animator.StringToHash("revived");
        private static readonly int DeadAnimParam = Animator.StringToHash("destroyed");
        
        public bool IsKnocked { get; set; }
        
        [Header("Sprite Direction")]
        [SerializeField] private int facingDirection = 1;
        
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
            if (!_animator) return;
            HandlePlayerSpriteDirection((int)_playerMovement.InputMovement.x);
            HandlePlayerAnimEvents();
        }
        
        public void RevivePlayerAnimEvent() => _animator.SetTrigger(RevivedAnimParam);
        public void DestroyPlayerAnimEvent() => _animator.SetTrigger(DeadAnimParam);

        private void HandlePlayerAnimEvents()
        {
            // player moves or is idle
            _animator.SetFloat(MoveAnimParam, _playerMovement.InputMovement.x);
            
            // player jumps
            _animator.SetFloat(JumpAnimParam, _playerMovement.GetLinearVelocity().y);
            
            // player is jumping or not
            _animator.SetBool(JumpingAnimParam, !_playerCollision.IsGrounded);

            // player does a second jump mid air
            if (_playerMovement.ConsumeDoubleJump())
                _animator.SetTrigger(DoubleJumpAnimParam);
            
            // player is sliding on a wall
            _animator.SetBool(WallSlideAnimParam, _playerMovement.CurrentState == PlayerMovementController.State.WallSliding);
            
            // player is hurt, takes damage
            _animator.SetBool(PlayerHurtAnimParam, _playerMovement.PlayerIsKnocked);
        }

        private void HandlePlayerSpriteDirection(int x)
        {
            if (x == 0) return;
            facingDirection = x;
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }
}
