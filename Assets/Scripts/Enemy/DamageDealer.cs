using Player;
using UnityEngine;

namespace Enemy
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerHealthController>().PlayerTakeDamage(damage);
            }
        }
    }
}
