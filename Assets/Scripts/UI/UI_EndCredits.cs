using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIEndCredits : MonoBehaviour
    {
        [Header("Credits Panel")]
        [SerializeField] private RectTransform creditsPanel;
        [SerializeField] private float scrollSpeed = 100f;
        [SerializeField] private float offScreenPosition = 1200;
        [SerializeField] private float fadeDuration = 2;
        
        private bool _creditsSkip;
        private UIFadeInOutVFX _fadeEffect;

        private void Awake()
        {
            _fadeEffect = GetComponentInChildren<UIFadeInOutVFX>();
        }

        private void Start()
        {
            _fadeEffect.ScreenFade(0, fadeDuration);
        }

        private void Update()
        {
            creditsPanel.anchoredPosition += Vector2.up * (Time.deltaTime * scrollSpeed);

            if (creditsPanel.anchoredPosition.y > offScreenPosition) GoToMainMenu();
        }

        public void SkipCreditsRoll()
        {
            if (!_creditsSkip)
            {
                scrollSpeed *= 10;
                _creditsSkip = true;
            }
            else GoToMainMenu();
        }

        private void GoToMainMenu() => _fadeEffect.ScreenFade(1, 1, GoToMainMenuScene);
    
        private void GoToMainMenuScene() => SceneManager.LoadScene("MainMenu");
    }
}
