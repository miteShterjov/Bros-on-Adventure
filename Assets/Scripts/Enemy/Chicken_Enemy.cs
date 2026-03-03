using UnityEngine;

namespace Enemy
{
    public class ChickenEnemy : EnemyController
    {
        [Header("Chicken"), Space] [SerializeField]
        protected float speedMultiplier = 1.5f; // max multiplier after ramp-up

        [SerializeField] private float aggroRampUpTime = 2.0f; // seconds to reach max multiplier

        private float _aggroTime;
        private float _baseSpeed;

        private void Start()
        {
            _baseSpeed = moveSpeed;
        }

        protected override void Update()
        {
            base.Update();
            if (!hasAggro && !Mathf.Approximately(_baseSpeed, moveSpeed)) moveSpeed = _baseSpeed;
        }

        protected override void HandleIdleState()
        {
            _aggroTime = 0f;
            base.HandleIdleState();
        }

        protected override void HandleMovingState()
        {
            base.HandleMovingState();
            if (!isGroundDetected || isWallDetected) DoPatrolSequence();
        }

        protected override void HandleAttackState()
        {
            if (!isGroundDetected || isWallDetected)
            {
                SetLinearVelocity(0f, Rigidbody.linearVelocity.y);
                return;
            }

            _aggroTime += Time.deltaTime;

            float t = aggroRampUpTime <= 0f ? 1f : Mathf.Clamp01(_aggroTime / aggroRampUpTime);
            float currentMultiplier = Mathf.Lerp(1f, speedMultiplier, t);

            SetLinearVelocity(moveSpeed * currentMultiplier * facingDirection, Rigidbody.linearVelocity.y);
        }
    }
}