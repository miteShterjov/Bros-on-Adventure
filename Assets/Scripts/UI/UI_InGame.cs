using System;
using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame Instance;
    public UI_FadeInOutVFX FadeEffect { get; private set; }
    
    [SerializeField] private TextMeshProUGUI fruitText;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        Instance = this;
        
        FadeEffect = GetComponentInChildren<UI_FadeInOutVFX>();
    }

    public void UpdateFruitsUI()
    {
        fruitText.text = GameManager.Instance.FruitsInfo();
    }

    public void UpdateTImerUI(float timer)
    {
        timerText.text = timer.ToString("00") + " s";
    }
}
