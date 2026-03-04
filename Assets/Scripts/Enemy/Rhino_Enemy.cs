using UnityEngine;

namespace Enemy
{
    public class RhinoEnemy : EnemyController
    {
        [Space][Header("Rhino")] 
        [SerializeField] private float maxSpeedMultiplier = 2.0f;
        
        private static readonly int WallHitParam = Animator.StringToHash("HitWall");
        
        private bool _isCharging;

        protected override void Update()
        {
            if (_isCharging)
            {
                HandleCollisionDetection();
                SetLinearVelocity(moveSpeed * facingDirection * maxSpeedMultiplier, rigidbody.linearVelocity.y);

                if (!isGroundDetected)
                {
                    _isCharging = false;
                    SetLinearVelocity(0f, rigidbody.linearVelocity.y);
                    return;
                }

                if (isWallDetected)
                {
                    _isCharging = false;
                    SetLinearVelocity(0f, rigidbody.linearVelocity.y);
                    DoRhinoWallCollisionSequence();
                    return;
                }

                HandleAnimationEvents();
                return;
            }
            
            base.Update();
            //if (isKnocked) KnockBackVisualEffect();
        }

        protected override void HandleAttackState()
        {
            // Start to charge at once
            if (!_isCharging) _isCharging = true;
        }

        private void DoRhinoWallCollisionSequence()
        {
            animator.SetTrigger(WallHitParam);
            KnockBackVisualEffect();    
        }
    }
}