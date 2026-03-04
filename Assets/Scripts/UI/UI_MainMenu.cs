using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private string firstLevelName;
        [SerializeField] private float fadeDuration = 2;
        [SerializeField] private GameObject[] uiElements;
        [SerializeField] private GameObject continueButton;
    
        private UIFadeInOutVFX _fadeEffect;

        private void Awake()
        {
            _fadeEffect = GetComponentInChildren<UIFadeInOutVFX>();
        }

        private void Start()
        {
            if (HasLevelProgress()) continueButton.SetActive(true);
            _fadeEffect.ScreenFade(0, fadeDuration);
        }

        public void NewGame() => _fadeEffect.ScreenFade(1, fadeDuration, LoadNextScene);
        
        public void ContinueGame()
        {
            int levelToContinue = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
            SceneManager.LoadScene("Level_" + levelToContinue);
        }
    
        private void LoadNextScene() => SceneManager.LoadScene(firstLevelName);

        public void SwitchUI(GameObject uiToEnable)
        {
            foreach (GameObject ui in uiElements) ui.SetActive(false);
            uiToEnable.SetActive(true);
        }

        private static bool HasLevelProgress()
        {
            bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
            return hasLevelProgression;
        }
    }
}
