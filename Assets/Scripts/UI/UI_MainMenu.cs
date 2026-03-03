using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelName;
    [SerializeField] private float fadeDuration = 2;
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject continueButton;
    
    private UI_FadeInOutVFX fadeEffect;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeInOutVFX>();
    }

    private void Start()
    {
        if (HasLevelProgress()) continueButton.SetActive(true);
        fadeEffect.ScreenFade(0, fadeDuration);
    }

    public void NewGame()
    {
        fadeEffect.ScreenFade(1, fadeDuration, LoadNextScene);
    }

    public void ContinueGame()
    {
        int levelToContinue = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        SceneManager.LoadScene("Level_" + levelToContinue);
    }
    
    private void LoadNextScene() => SceneManager.LoadScene(firstLevelName);

    public void SwichUI(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElements) ui.SetActive(false);
        uiToEnable.SetActive(true);
    }

    private bool HasLevelProgress()
    {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
        return hasLevelProgression;
    }
}
