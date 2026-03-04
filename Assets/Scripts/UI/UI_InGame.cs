using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIInGame : MonoBehaviour
    {
        public static UIInGame Instance;
        // public UIFadeInOutVFX FadeEffect { get; private set; }
    
        [SerializeField] private TextMeshProUGUI fruitText;
        [SerializeField] private TextMeshProUGUI timerText;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
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
}
