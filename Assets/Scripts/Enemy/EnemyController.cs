using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        [Header("Movement")]
        [SerializeField] protected float moveSpeed = 5f;
        [SerializeField] protected int facingDirection;
        [SerializeField] protected bool isFacingLeft;
        [SerializeField] protected bool isPatrolling;
        [Space][Header("Collision")]
        [SerializeField] protected float groundCheckDistance;
        [SerializeField] protected float wallCheckDistance;
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected bool isGroundDetected;
        [SerializeField] protected bool isWallDetected;
        [SerializeField, Tooltip("When Enemy uses the knockback visual effect set the pushback force here.")] protected Vector2 customForce = new Vector2(2f, 3f);
        [Space][Header("Death")] 
        [SerializeField] protected float deathImpact;
        [SerializeField] protected float deathRotationSpeed;
        [SerializeField] protected bool isDead;
        [Space][Header("Attack")]
        [SerializeField] protected float aggroRange;
        [SerializeField] protected bool hasAggro;
        [SerializeField] protected bool hasBackAggro;
        [SerializeField] private LayerMask playerLayer;
        [Space][Header("Shooter")] 
        [SerializeField] protected bool isShooter;
        [SerializeField] protected BulletEnemy bulletPrefab;
        [SerializeField] protected float attackSpeed;
        
        protected new Rigidbody2D rigidbody;
        protected Animator animator;
        private Collider2D _collider;
        private Collider2D _colliderInChild;
        private Coroutine _patrolCoroutine;

        protected float timer;
        protected bool canShoot;
        
        private static readonly int AnimHitParam = Animator.StringToHash("Hit");
        private static readonly int AnimMoveParam = Animator.StringToHash("xVelocity");
        
        private void OnValidate()
        {
            facingDirection = isFacingLeft ? -1 : 1;

            // Optional: keep the sprite/object visually consistent in the editor too.
            // If you don't want OnValidate touching scale, delete these 2 lines.
            Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(scale.x) * -facingDirection, scale.y, scale.z);
            
            if (!isShooter) canShoot = false;
        }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _colliderInChild = GetComponentInChildren<Collider2D>();
            
            facingDirection = isFacingLeft ? -1 : 1;
        }
        
        protected virtual void Update()
        {
            HandleCollisionDetection();
            HandleAggroDetection();
            
            if (isDead)
            {
                HandleEnemyDies();
                return;
            }
            
            if (hasAggro) HandleAttackState();
            else if (hasBackAggro) HandleFacingDirection();
            else if (isPatrolling) HandleMovingState();
            else HandleIdleState();
            
            HandleAnimationEvents();
        }

        private void FixedUpdate()
        {
            timer += Time.deltaTime;
        }
        
        protected virtual void HandleIdleState() => SetLinearVelocity(0, rigidbody.linearVelocity.y);
        
        protected virtual void HandleMovingState() => SetLinearVelocity(moveSpeed * facingDirection, rigidbody.linearVelocity.y);

        protected virtual void HandleAttackState() => print("Attack!");

        protected virtual void HandleFacingDirection()
        {
            facingDirection = -facingDirection;
            transform.localScale = new Vector3(-facingDirection, 1, 1);
        }
        
        protected virtual void HandleEnemyDies()
        {
            const float destroyDelay = 1f;
            
            _collider.enabled = false;
            _colliderInChild.enabled = false;
            
            animator.SetTrigger(AnimHitParam);
            SetLinearVelocity(rigidbody.linearVelocity.x, deathImpact);
            transform.Rotate(Vector3.forward * (deathRotationSpeed * Time.deltaTime));
            Destroy(gameObject, destroyDelay);
        }

        protected virtual void HandleAnimationEvents()
        {
            animator.SetFloat(AnimMoveParam, rigidbody.linearVelocity.x);
        }

        protected virtual void HandleCollisionDetection()
        {
            Vector3 transformPosition = transform.position + Vector3.right * (facingDirection * wallCheckDistance);
            
            isGroundDetected = Physics2D.Raycast(
                transformPosition,
                Vector2.down, groundCheckDistance,
                groundLayer
            );
            isWallDetected = Physics2D.Raycast(
                transform.position,
                Vector2.right * facingDirection, wallCheckDistance,
                groundLayer
            );
        }

        private void HandleAggroDetection()
        {
            hasAggro = Physics2D.Raycast(
                transform.position,
                Vector2.right * facingDirection, aggroRange,
                playerLayer);
            
            hasBackAggro = Physics2D.Raycast(
                transform.position,
                Vector2.right * -facingDirection, aggroRange,
                playerLayer);
        }
        
        protected void DoPatrolSequence()
        {
            if (_patrolCoroutine != null) return;

            isPatrolling = false;
            _patrolCoroutine = StartCoroutine(PatrolCoroutine());
        }

        private IEnumerator PatrolCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            HandleFacingDirection();
            isPatrolling = true;
            _patrolCoroutine = null;
        }
        
        protected virtual void OnDrawGizmos()
        {
            float r = 0.04f;
            Vector3 transformPosition = transform.position + Vector3.right * facingDirection * wallCheckDistance;

            // isGroundDetected
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transformPosition, transformPosition + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(transformPosition + Vector3.down * groundCheckDistance, r);

            // isWallDetected
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transformPosition);
            Gizmos.DrawWireSphere(transformPosition, r);
            
            // Aggro Range
            Gizmos.color = Color.blueViolet;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDirection * aggroRange);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * facingDirection * aggroRange, 0.2f);

            // Back Aggro Range
            Gizmos.color = Color.blueViolet;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * -facingDirection * aggroRange);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * -facingDirection * aggroRange, 0.2f);
        }

        protected virtual void SetLinearVelocity(float x, float y) => rigidbody.linearVelocity = new Vector2(x, y);
        protected virtual void SetLinearVelocity(Vector2 velocity) => rigidbody.linearVelocity = velocity;
        
        protected virtual void KnockBackVisualEffect() => rigidbody.AddForce(customForce * -facingDirection, ForceMode2D.Impulse);
        
        protected void Shoot()
        {
            BulletEnemy bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Direction = facingDirection;
            timer = 0f;
        }
    }
}