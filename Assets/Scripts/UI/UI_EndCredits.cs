using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_EndCredits : MonoBehaviour
{
    [SerializeField] private RectTransform creditsPanel;
    [SerializeField] private float scrollSpeed = 100f;
    [SerializeField] private float offScreenPosition = 1200;
    [SerializeField] private float fadeDuration = 2;
    private bool creditsSkiped;
    private UI_FadeInOutVFX fadeEffect;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeInOutVFX>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, fadeDuration);
    }

    private void Update()
    {
        creditsPanel.anchoredPosition += Vector2.up * Time.deltaTime * scrollSpeed;

        if (creditsPanel.anchoredPosition.y > offScreenPosition) GoToMainMenu();
    }

    public void SkipCreditsRoll()
    {
        if (!creditsSkiped)
        {
            scrollSpeed *= 10;
            creditsSkiped = true;
        }
        else GoToMainMenu();
    }

    private void GoToMainMenu() => fadeEffect.ScreenFade(1, 1, GoToMainMenuScene);
    
    private void GoToMainMenuScene() => SceneManager.LoadScene("MainMenu");
}
