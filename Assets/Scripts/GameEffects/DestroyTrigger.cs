using UnityEngine;

namespace GameEffects
{
    public class DestroyTrigger : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField][Range(0, 10)] private float destroyDelay = 0.2f;
        
        private void OnTriggerEnter2D(Collider2D other) => Destroy(other.gameObject, destroyDelay);
    }
}