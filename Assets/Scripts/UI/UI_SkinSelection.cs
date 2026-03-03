using UnityEngine;

public class UI_SkinSelection : MonoBehaviour
{
    [SerializeField] private int currentIndex;
    [SerializeField] private int maxIndex;
    [SerializeField] private Animator skindDisplayed;

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
        for (int i = 0; i < skindDisplayed.layerCount; i++) skindDisplayed.SetLayerWeight(i, 0);
        
        skindDisplayed.SetLayerWeight(currentIndex, 1);
    }
}
