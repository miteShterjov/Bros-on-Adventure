using Enemy;
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

        [Header("Enemy")] 
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float enemyCheckOffset;
        [SerializeField] private float enemyCheckDistance;
        [SerializeField] private float bounceForce = 10f;

        private PlayerAnimationController _playerAnim;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _playerAnim = GetComponent<PlayerAnimationController>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            WallDirection = _playerAnim.FacingDirection;

            HandleGroundCheck();
            HandleWallCheck();
            HandleEnemyCheck();
        }

        private void HandleEnemyCheck()
        {
            if (_rb.linearVelocity.y >= 0) return;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position + Vector3.down * enemyCheckOffset,
                enemyCheckDistance,
                enemyLayer);

            foreach (var enemy in colliders)
            {
                if (enemy != null)
                {
                    enemy.GetComponent<EnemyController>().IsDead = true;
                    PlayerBounceOfTarget();
                }
            }
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

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * enemyCheckOffset, enemyCheckDistance);
        }

        private void PlayerBounceOfTarget()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, bounceForce);
        }
    }
}
