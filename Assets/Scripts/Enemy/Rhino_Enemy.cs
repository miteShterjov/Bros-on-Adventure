using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Enemy
{
    public class RhinoEnemy : EnemyController
    {
        [Header("Rhino"), Space] 
        [SerializeField] private float maxSpeedMultiplier = 2.0f;
        
        private static readonly int WallHitParam = Animator.StringToHash("HitWall");
        
        private bool _isCharging;

        protected override void Update()
        {
            if (_isCharging)
            {
                HandleCollisionDetection();
                SetLinearVelocity(moveSpeed * facingDirection * maxSpeedMultiplier, Rigidbody.linearVelocity.y);


                if (!isGroundDetected)
                {
                    _isCharging = false;
                    SetLinearVelocity(0f, Rigidbody.linearVelocity.y);
                    return;
                }

                if (isWallDetected)
                {
                    _isCharging = false;
                    SetLinearVelocity(0f, Rigidbody.linearVelocity.y);
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
            // Start charge once
            if (!_isCharging) _isCharging = true;
        }

        private void DoRhinoWallCollisionSequence()
        {
            Animator.SetTrigger(WallHitParam);
            KnockBackVisualEffect();    
        }
    }
}