
namespace Enemy
{
    public class MushroomEnemy : EnemyController
    {
        protected override void Update()
        {
            base.Update();

            if (!isGroundDetected || isWallDetected) DoPatrolSequence();
        }
    }
}