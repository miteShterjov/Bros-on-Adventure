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
                if (_timer > attackSpeed) canShoot = true;
                else if (_timer < attackSpeed) canShoot = false;
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
                SetLinearVelocity(0, Rigidbody.linearVelocity.y);
                Animator.SetTrigger(ShotAnimParamm);
                Shoot();
            }
        }
    }
}
