using System;
using UnityEngine;

namespace Enemy
{
    public class BulletEnemy : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private Sprite bulletSprite0;
        [SerializeField] private Sprite bulletSprite1;

        public int Direction { get; set; }

        private void Update()
        {
            transform.Translate(Vector2.right * (moveSpeed * Direction * Time.deltaTime));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ShatterBulletOnCollision();
        }

        private void ShatterBulletOnCollision()
        {
            // need work done,
            // when bullet hits a target is shaters in 2 piesces.
            // they fall to the ground and 
            // after x time is gone they are destroyed
            
        }
    }
}
