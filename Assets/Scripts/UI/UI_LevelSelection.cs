using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UILevelSelection : MonoBehaviour
    {
        [Header("Level Buttons")]
        [SerializeField] private UILevelButton buttonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private bool[] levelsUnlocked;

        private void Start()
        {
            LoadLevelsInfo();
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            int levelAmounts = SceneManager.sceneCountInBuildSettings - 1;

            for (int i = 1; i < levelAmounts; i++)
            {
                if (!IsLevelUnlocked(i)) return;
                UILevelButton newButton = Instantiate(buttonPrefab, buttonParent);
                newButton.SetupButton(i);
            }
        }

        private void LoadLevelsInfo()
        {
            int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;
        
            levelsUnlocked = new bool[levelsAmount];
        
            for (int i = 1; i < levelsAmount; i++)
            {
                bool levelIsUnlocked = PlayerPrefs.GetInt("Level " + i + "is Unlocked.", 0) == 1;
                if (levelIsUnlocked) levelsUnlocked[i] = true;
            }
            levelsUnlocked[1] = true;
        }
    
        private bool IsLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];
    }
}
