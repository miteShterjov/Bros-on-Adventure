using System;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionController : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        public bool IsWallDetected { get; private set; }
        
        [Header("Ground Detection")] 
        [SerializeField] private float groundCheckDistance = 0.9f;
        [SerializeField] private float wallCheckDistance = 1f;
        [SerializeField] private LayerMask groundLayer;
        

        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private PlayerAnimationController _playerAnimationController;
        private int _facingDirection = 1;
        
        private void Awake()
        {
            _playerAnimationController = GetComponent<PlayerAnimationController>();
        }

        private void Update()
        {
            _facingDirection = _playerAnimationController.FacingDirection;
            
            HandleGroundCollision();
            HandleWallCollision();
        }

        private void HandleGroundCollision()
        {
            int count = Physics2D.RaycastNonAlloc(
                transform.position,
                Vector2.down,
                _hits,
                groundCheckDistance,
                groundLayer
            );

            IsGrounded = count > 0;
        }
        
        private void HandleWallCollision()
        {
            IsWallDetected = Physics2D.Raycast(
                transform.position,
                Vector2.right * _playerAnimationController.FacingDirection,
                wallCheckDistance,
                groundLayer);
        }

        private void OnDrawGizmos()
        {
            float sphereRadious = 0.04f;
            
            Gizmos.color = IsGrounded ? Color.green: Color.red;
            Gizmos.DrawLine(
                transform.position,
                transform.position + Vector3.down * groundCheckDistance);
            Gizmos.DrawWireSphere(
                transform.position + Vector3.down * groundCheckDistance, 
                sphereRadious);
            
            Gizmos.color = IsWallDetected ? Color.green: Color.red;
            Gizmos.DrawLine(
                transform.position,
                transform.position + Vector3.right * _facingDirection * wallCheckDistance);
            Gizmos.DrawWireSphere(
                transform.position + Vector3.right * _facingDirection * wallCheckDistance, 
                sphereRadious);
        }
    }
}