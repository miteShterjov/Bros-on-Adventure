using UnityEngine;

namespace Player
{
    public class PlayerCollisionController : MonoBehaviour
    {
        public bool IsGrounded => isGrounded;
        public bool IsWallDetected => isWallDetected;

        public int WallDirection { get; private set; }

        [Header("Ground")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 0.4f;
        [SerializeField] public bool isGrounded;

        [Header("Wall")]
        [SerializeField] private float wallCheckDistance = 0.4f;
        [SerializeField] public bool isWallDetected;

        private PlayerAnimationController _playerAnim;

        private void Awake()
        {
            _playerAnim = GetComponent<PlayerAnimationController>();
        }

        private void FixedUpdate()
        {
            WallDirection = (int)_playerAnim.FacingDirection;

            HandleGroundCheck();
            HandleWallCheck();
        }

        private void HandleGroundCheck()
        {
            isGrounded = Physics2D.Raycast(
                transform.position,
                Vector2.down,
                groundCheckDistance,
                groundLayer
            );
        }

        private void HandleWallCheck()
        {
            isWallDetected = Physics2D.Raycast(
                transform.position,
                Vector2.right * WallDirection,
                wallCheckDistance,
                groundLayer
            );
        }

        private void OnDrawGizmos()
        {
            float r = 0.04f;

            // Ground
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, r);

            // Wall
            Gizmos.color = isWallDetected ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.right * WallDirection * wallCheckDistance);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * WallDirection * wallCheckDistance, r);
        }
    }
}
