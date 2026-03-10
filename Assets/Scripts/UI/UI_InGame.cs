using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIInGame : MonoBehaviour
    {
        public static UIInGame Instance;
        public UIFadeInOutVFX fadeEffect;
     
        [Header("Config")]
        [SerializeField] private TextMeshProUGUI fruitText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject uiPauseMenu;
        
        private bool _isPaused;

        private const string MainMenuSceneHash = ("MainMenu");

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            fadeEffect = GetComponentInChildren<UIFadeInOutVFX>();
            
            if (uiPauseMenu != null) uiPauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) PauseGameActive();
        }

        public void UpdateFruitsUI()
        {
            fruitText.text = GameManager.Instance.FruitsInfo();
        }

        public void UpdateTImerUI(float timer)
        {
            timerText.text = timer.ToString("00") + " s";
        }
        
        public void MainMenuButton() => SceneManager.LoadScene(MainMenuSceneHash);
        
        public void ResumeGame() => PauseGameActive();

        private void PauseGameActive()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
            
            if (uiPauseMenu != null) uiPauseMenu.SetActive(_isPaused);
        }
    }
}
