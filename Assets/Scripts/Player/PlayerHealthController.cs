using GameEffects;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        [Header("Player Health")]
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private float currentHealth;
        
        private DifficultyType _gameDifficulty;
        private GameManager _gameManager;
    
        private KnockbackVFX _knockback;
    
        private void Awake()
        {
            currentHealth = maxHealth;
            _knockback = GetComponent<KnockbackVFX>();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            if (DifficultyManager.Instance == null) Debug.LogError("DifficultyManager is null");
            else _gameDifficulty = DifficultyManager.Instance.difficultyType;
        }

        public float CurrentHealth => currentHealth;
        
        public float MaxHealth => maxHealth;
    
        public void PlayerTakeDamage(float damage)
        {
            float GetDamageMultiplier() => DifficultyManager.Instance.GetDifficultyMultiplier();
            switch (_gameDifficulty)
            {
                // !!! Fix the magic numbers in due time!!!
                case DifficultyType.Easy:
                    currentHealth -= damage * GetDamageMultiplier();
                    break;
                case DifficultyType.Normal:
                    currentHealth -= damage * GetDamageMultiplier();
                    _gameManager.RemoveFruits(1);
                    break;
                case DifficultyType.Hard:
                    GameManager.RestartCurrentLevel();
                    break;
                default:
                    currentHealth -= damage * GetDamageMultiplier();
                    break; 
            }
            
            if (_gameManager.FruitsCollected() <= 0) GameManager.RestartCurrentLevel();
            
            currentHealth = Mathf.Max(0, currentHealth);
            
            
            if (currentHealth > 0) _knockback.Knockback();
            else Destroy(gameObject); 
        }
    }
}
