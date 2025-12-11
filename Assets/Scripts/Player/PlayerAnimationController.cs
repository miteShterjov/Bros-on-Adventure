using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public int FacingDirection
        {
            get => _facingDirection;
        }

        private static readonly int AnimMoveParam = Animator.StringToHash("xVelocity");
        private static readonly int AnimJumpParam = Animator.StringToHash("yVelocity");
        private static readonly int AnimIsJumping = Animator.StringToHash("isJumping");
        private static readonly int AnimIsDoubleJumping = Animator.StringToHash("doubleJump");
        private static readonly int AnimIsWallSliding = Animator.StringToHash("IsWallDetected");

        private Animator _animator;
        private PlayerMovementController _playerMovement;
        private PlayerCollisionController _playerCollision;
        private int _facingDirection = 1;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null) throw new Exception("PlayerAnimationController requires an Animator component");

            _playerMovement = GetComponent<PlayerMovementController>();
            _playerCollision = GetComponent<PlayerCollisionController>();
        }

        private void Update()
        {
            UpdateAnimEvents();
        }

        private void FixedUpdate()
        {
            FlipSprite((int)_playerMovement.MoveInput.x);
            ChangeDirection((int)_playerMovement.MoveInput.x);
        }

        private void UpdateAnimEvents()
        {
            _animator.SetFloat(AnimMoveParam, _playerMovement.MoveInput.x);
            _animator.SetFloat(AnimJumpParam, _playerMovement.GetLinearVelocity().y);

            _animator.SetBool(AnimIsJumping, _playerMovement.IsJumping());
            _animator.SetBool(AnimIsWallSliding, _playerCollision.IsWallDetected);
            if (_playerMovement.IsDoubleJumping) _animator.SetTrigger(AnimIsDoubleJumping);
        }

        public void ChangeDirection()
        {
            if (_facingDirection == 1) _facingDirection = -1;
            else if (_facingDirection == -1) _facingDirection = 1;
        }

        public void ChangeDirection(int x)
        {
            if (x == 0) return;
            _facingDirection = x;   
        }

        public void FlipSprite()
        {
            if (transform.localScale.x == 1) transform.localScale = new Vector3(-1, 1f, 1f);
            else if (transform.localScale.x == -1) transform.localScale = new Vector3(1, 1f, 1f);
        }

        public void FlipSprite(int x)
        {
            if (x == 0) return;
            transform.localScale = new Vector3(x, 1f, 1f);
        }
    }
}