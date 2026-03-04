using UnityEngine;

namespace Enemy
{
    public class PlantEnemy : EnemyController
    {
        [SerializeField] private Transform shotSpawnPoint;
        
        private static readonly int ShotAnimParamm = Animator.StringToHash("Attack");
     
        protected override void Update()
        {
            base.Update();
            
            if (isShooter)
            {
                if (timer > attackSpeed) canShoot = true;
                else if (timer < attackSpeed) canShoot = false;
            }
        }

        protected override void HandleAttackState()
        {
            if (canShoot)
            {
                animator.SetTrigger(ShotAnimParamm);
                Shoot();
            }
        }

        protected override void HandleAnimationEvents()
        {
        }
    }
}
