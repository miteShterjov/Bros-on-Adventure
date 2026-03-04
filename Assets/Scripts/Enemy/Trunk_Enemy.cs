using System.Numerics;

namespace Enemy
{
    public class TrunkEnemy : EnemyController
    {
        private static readonly int ShotAnimParamm = UnityEngine.Animator.StringToHash("Attack");

        protected override void Update()
        {
            base.Update();
            
            if (isShooter)
            {
                if (timer > attackSpeed) canShoot = true;
                else if (timer < attackSpeed) canShoot = false;
            }
        }
        
        protected override void HandleMovingState()
        {
            base.HandleMovingState();
            if (!isGroundDetected || isWallDetected) DoPatrolSequence();
        }

        protected override void HandleAttackState()
        {
            base.HandleAttackState();
            if (canShoot)
            {
                SetLinearVelocity(0, rigidbody.linearVelocity.y);
                animator.SetTrigger(ShotAnimParamm);
                Shoot();
            }
        }
    }
}
