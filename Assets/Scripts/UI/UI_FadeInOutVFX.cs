using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFadeInOutVFX : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private Image faderImage;

        public void ScreenFade(float targetAlpha, float duration, System.Action onCompleted = null)
        {
            StartCoroutine(FadeCoroutine(targetAlpha, duration, onCompleted));
        }

        private IEnumerator FadeCoroutine(float targetAlpha, float duration, System.Action onCompleted)
        {
            float time = 0;
            Color currentColor = faderImage.color;
        
            float startAlpha = currentColor.a;

            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                faderImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                yield return null;
            }
        
            faderImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        
            onCompleted?.Invoke();
        }
    }
}
