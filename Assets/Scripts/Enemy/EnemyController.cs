using System;
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

        private static readonly int MoveAnimParam = Animator.StringToHash("xVelocity");
        private static readonly int Hit = Animator.StringToHash("Hit");
        
        [Header("Movement")] 
        [SerializeField] protected float moveSpeed = 10f;
        [SerializeField] protected bool canMove = true;

        [Header("Basic collision")] 
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] protected LayerMask playerLayer;
        [SerializeField] protected float groundCheckDistance = 0.04f;
        [SerializeField] protected float wallCheckDistance = 0.04f;
        [SerializeField] protected float idleDuration = 0.5f;

        [Header("Death")]
        [SerializeField] private float deathImpact;
        [SerializeField] private float deathRotationSpeed;
        [SerializeField] protected bool isDead;

        protected int facingDirection = -1;
        protected float idleTimer = 0f;
        protected bool _isIdle;

        protected Animator animator;
        protected Rigidbody2D rigidbody;

        private Collider2D _collider;
        private Collider2D _colliderInChild;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            
            _collider = GetComponent<Collider2D>();
            _colliderInChild = GetComponentInChildren<Collider2D>();
        }
        
        protected virtual void Start() {}

        protected virtual void Update()
        {
            if (!canMove) return;
            if (isDead) HandleEnemyDies();
            
            HandleIdleStatus();
            HandleFacingDirection();
            HandleObjectMovement();
            HandleSpriteFacingSide();
            UpdateAnimEvents();
        }

        protected virtual void FixedUpdate()
        {
            idleTimer -= Time.deltaTime;
        }

        protected virtual void HandleIdleStatus()
        {
            if (!IsGroundDetected || IsWallDetected) idleTimer = idleDuration;
            
            if (idleTimer > 0) _isIdle = true;
            else if (idleTimer <= 0) _isIdle = false;

        }

        protected virtual void HandleFacingDirection()
        {
            if (!IsGroundDetected || IsWallDetected) facingDirection = -facingDirection;
        }
        
        protected virtual void HandleSpriteFacingSide() => transform.localScale = new Vector3(-facingDirection, 1, 1);

        protected virtual void HandleObjectMovement()
        {
            if (_isIdle) return;
            rigidbody.linearVelocity = new Vector2(moveSpeed * facingDirection, rigidbody.linearVelocity.y);
        }

        protected virtual void UpdateAnimEvents()
        {
            animator.SetFloat(MoveAnimParam, rigidbody.linearVelocity.x);
        }
        
        protected virtual void HandleEnemyDies()
        {
            float destroyDelay = 1f;
            
            _collider.enabled = false;
            _colliderInChild.enabled = false;
            
            animator.SetTrigger(Hit);
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, deathImpact);
            transform.Rotate(Vector3.forward * deathRotationSpeed * Time.deltaTime);
            Destroy(gameObject, destroyDelay);
        }

        protected virtual bool IsGroundDetected => Physics2D.Raycast(
            transform.position + Vector3.right * facingDirection * wallCheckDistance,
            Vector2.down, groundCheckDistance,
            groundLayer
        );

        protected virtual bool IsWallDetected => Physics2D.Raycast(
            transform.position,
            Vector2.right * facingDirection, wallCheckDistance,
            groundLayer
        );

        protected virtual void OnDrawGizmos()
        {
            float r = 0.04f;
            Gizmos.color = Color.red;

            Vector3 transformPosition = transform.position + Vector3.right * facingDirection * wallCheckDistance;

            Gizmos.DrawLine(transformPosition, transformPosition + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(Vector3.down * groundCheckDistance, r);

            Gizmos.DrawLine(transform.position, transformPosition);
            Gizmos.DrawWireSphere(Vector3.right * facingDirection * wallCheckDistance, r);
        }
    }
}