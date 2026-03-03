using GameEffects;
using UnityEngine;

namespace Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        [Header("Player Health")]
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float currentHealth;
    
        private KnockbackVFX _knockback;
    
        private void Awake()
        {
            currentHealth = maxHealth;
            _knockback = GetComponent<KnockbackVFX>();
        }
    
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
    
        public void PlayerTakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth); // Keep health at 0 or above
            
            if (currentHealth > 0)
            {
                _knockback.Knockback();
            }
            else
            {
                // Trigger death animation or destroy here
                // If using DestroyTrigger/Animation, ensure it eventually calls Destroy(gameObject)
                Destroy(gameObject); 
            }
        }
    }
}
