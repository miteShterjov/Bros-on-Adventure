using Managers;
using UnityEngine;

namespace UI
{
    public class UISkinSelection : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int currentIndex;
        [SerializeField] private int maxIndex;
        [SerializeField] private Animator skinDisplayed;

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
            SkinManager.Instance.SetSkinID(currentIndex);
        }

        private void UpdateSkinDisplayed()
        {
            for (int i = 0; i < skinDisplayed.layerCount; i++) skinDisplayed.SetLayerWeight(i, 0);
        skinDisplayed.SetLayerWeight(currentIndex, 1);
        }
    }
}
