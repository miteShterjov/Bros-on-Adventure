using UnityEngine;

namespace Enemy
{
    public class BulletEnemy : MonoBehaviour
    {
        public int Direction { get; set; }
        
        [Header("Config")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private Sprite bulletSprite0;
        [SerializeField] private Sprite bulletSprite1;

        private void Update()
        {
            transform.Translate(Vector2.right * (moveSpeed * Direction * Time.deltaTime));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            ShatterBulletOnCollision();
        }

        private static void ShatterBulletOnCollision()
        {
            // This method needs a lot of work done.
            // When the bullet hits the target, it shatters in 2 pieces.
            // They fall to the ground.  
            // After x time is gone they are destroyed.
            // I'm just not sure this to apply only to player or ground too?
        }
    }
}
