using Managers;
using UnityEngine;

namespace UI
{
    public class UIDifficulty : MonoBehaviour
    {
        private DifficultyManager _difficultyManager;
        private void Start()
        {
            _difficultyManager = DifficultyManager.Instance;
        }

        public void SetEasyMode() => _difficultyManager.SetDifficulty(DifficultyType.Easy);
        public void SetNormalMode() => _difficultyManager.SetDifficulty(DifficultyType.Normal);
        public void SetHardMode() => _difficultyManager.SetDifficulty(DifficultyType.Hard);
    }
}
