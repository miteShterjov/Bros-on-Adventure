using UnityEngine;

namespace GameEffects
{
    public class DestroySelf : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField][Range(0, 10)] private float destroyDelay;

        public void DestroyGameObject()
        {
            Destroy(transform.parent != null ? transform.parent.gameObject : gameObject, destroyDelay);
        }
    }
}