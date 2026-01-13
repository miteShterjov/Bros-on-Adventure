using UnityEngine;

namespace Enemy
{
    public class MushroomEnemy : EnemyController
    {
        private static readonly int MoveAnimParam = Animator.StringToHash("xVelocity");

        protected override void HandleObjectMovement()
        {
            if (_isIdle) return;
            rigidbody.linearVelocity = new Vector2(moveSpeed * facingDirection, rigidbody.linearVelocity.y);
        }

        protected override void UpdateAnimEvents()
        {
            animator.SetFloat(MoveAnimParam, rigidbody.linearVelocity.x);
        }

        protected override void HandleEnemyDies()
        {
            base.HandleEnemyDies();
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponentInChildren<DamageDealer>().enabled = false;
        }
    }
}