using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UILevelButton : MonoBehaviour
    {
        public string sceneName;
        public int levelIndex;
        
        [Header("Level Info Texts")]
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI bestTimeText;
        [SerializeField] private TextMeshProUGUI fruitsText;
    
        public void SetupButton(int newLevelIndex)
        {
            levelIndex = newLevelIndex;
            levelNumberText.text = "Level " + levelIndex;
            sceneName = "Level_" + levelIndex;
            bestTimeText.text = TimerInfoText();
            fruitsText.text = FruitsInfoText();
        }
    
        public void LoadLevel() => SceneManager.LoadScene(sceneName);

        private string FruitsInfoText()
        {
            string totalFruitsKey = $"Level {levelIndex} : TotalFruits";
            string fruitsCollectedKey = $"Level {levelIndex} : Fruits Collected";

            int totalFruits = PlayerPrefs.GetInt(totalFruitsKey, 0);
            int fruitsCollected = PlayerPrefs.GetInt(fruitsCollectedKey, 0);

            return $"Fruits: {fruitsCollected}/{totalFruits}";
        }

        private string TimerInfoText()
        {
            string bestTimeKey = $"Level {levelIndex} : BestTime";

            float timerValue = PlayerPrefs.GetFloat(bestTimeKey, 99f);
            return "Best Time: " + timerValue.ToString("00") + " s";
        }
    }
}
