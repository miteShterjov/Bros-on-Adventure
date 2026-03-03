using UnityEngine;

namespace GameEffects
{
    public class DestroySelf : MonoBehaviour
    {
        [SerializeField] private float destroyDelay = 0f;

        public void DestroyGameObject()
        {
            Destroy(transform.parent != null ? transform.parent.gameObject : gameObject, destroyDelay);
        }
    }
}