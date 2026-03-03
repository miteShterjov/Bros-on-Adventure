using UnityEngine;

namespace Enemy.Traps
{
    public class SpikedTrap : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float pushForce = 10f;

        private void Start()
        {
            rb.AddForce(
                new Vector2(pushForce, rb.linearVelocity.y),
                ForceMode2D.Impulse
            );
        }
    }
}