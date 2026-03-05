using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UISkinSelection : MonoBehaviour
    {
        [Header("Skins")]
        [SerializeField] private Skin[] skins;
        [Space][Header("Config")]
        [SerializeField] private int currentIndex;
        [SerializeField] private int maxIndex;
        [SerializeField] private Animator skinDisplayed;

        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI bankText;
        [SerializeField] private TextMeshProUGUI buySelectText;

        private void Start()
        {
            LoadSkinUnlocks();
            UpdateSkinDisplayed();
        }
        
        public void NextSkin()
        {
            currentIndex++;
            if (currentIndex > maxIndex) currentIndex = 0;
            UpdateSkinDisplayed();
        }
    
        public void PreviousSkin()
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = maxIndex;
            UpdateSkinDisplayed();
        }

        public void SelectSkin()
        {
            if(skins[currentIndex].unlocked == false) BuySkin(currentIndex);
            else SkinManager.Instance.SetSkinID(currentIndex);
            
            UpdateSkinDisplayed();
        }

        private void LoadSkinUnlocks()
        {
            for (int i = 0; i < skins.Length; i++)
            {
                string skinName = skins[i].skinName;
                bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;
                
                if (skinUnlocked || i ==0) skins[i].unlocked = true;
            }
        }

        private void UpdateSkinDisplayed()
        {
            bankText.text = "Bank: " + FruitsInBank();
            
            for (int i = 0; i < skinDisplayed.layerCount; i++) skinDisplayed.SetLayerWeight(i, 0);
            skinDisplayed.SetLayerWeight(currentIndex, 1);

            if (skins[currentIndex].unlocked)
            {
                priceText.transform.parent.gameObject.SetActive(false);
                buySelectText.text = "Select";
            }
            else
            {
                priceText.transform.parent.gameObject.SetActive(true);
                priceText.text = "Price: " + skins[currentIndex].skinPrice.ToString();
                buySelectText.text = "Buy";
            }
        }

        private void BuySkin(int index)
        {
            if (!HaveEnoughFruits(skins[index].skinPrice)) return;
            
            string skinName = skins[index].skinName;
            skins[index].unlocked = true;
            
            PlayerPrefs.SetInt(skinName + "Unlocked", 1);
        }

        private bool HaveEnoughFruits(int price)
        {
            if (FruitsInBank() < price) return false;
            PlayerPrefs.SetInt("Total Fruits Amount", FruitsInBank() - price);
            return true;
        }
        
        private int FruitsInBank() => PlayerPrefs.GetInt("Total Fruits Amount", 0);
    }
}

[System.Serializable]
public struct Skin
{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}
