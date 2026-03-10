using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIDifficultyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI difficultyInfo;
        [TextArea][SerializeField] private string description;
        
        public void OnPointerEnter(PointerEventData eventData) => difficultyInfo.text = description;
        public void OnPointerExit(PointerEventData eventData) => difficultyInfo.text = "";
    }
}
