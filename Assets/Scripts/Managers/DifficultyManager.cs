using UnityEngine;

namespace Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance;
        public DifficultyType difficultyType;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        public void SetDifficulty(DifficultyType difficulty) => difficultyType = difficulty;
        
        public float GetDifficultyMultiplier()
        {
            const float easyMultiplier = 0.8f;
            const float normalMultiplier = 1f;
            const float hardMultiplier = 1.4f;
            const float defaultMultiplier = 1f;
            
            return difficultyType switch
            {
                DifficultyType.Easy => easyMultiplier,
                DifficultyType.Normal => normalMultiplier,
                DifficultyType.Hard => hardMultiplier,
                _ => defaultMultiplier
            };
        }
    }
}

public enum DifficultyType { Easy, Normal, Hard }