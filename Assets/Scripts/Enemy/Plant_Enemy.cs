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
                if (_timer > attackSpeed) canShoot = true;
                else if (_timer < attackSpeed) canShoot = false;
            }
        }

        protected override void HandleAttackState()
        {
            if (canShoot)
            {
                Animator.SetTrigger(ShotAnimParamm);
                Shoot();
            }
        }

        protected override void HandleAnimationEvents()
        {
        }
    }
}
