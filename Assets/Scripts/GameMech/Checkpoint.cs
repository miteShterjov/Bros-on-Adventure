using Managers;
using UnityEngine;

namespace GameMech
{
    public class Checkpoint : MonoBehaviour
    {
        [ Header("Checkpoint")]
        [SerializeField] private bool isActiveCheckpoint = false;
        
        private bool _wasActiveCheckpoint = false;
        private Animator _animator;
        
        private static readonly int ActiveCheckpointParam = Animator.StringToHash("isActive");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActiveCheckpoint || _wasActiveCheckpoint) return;
            if (other.CompareTag("Player")) SetActiveCheckpoint();
        }

        private void SetActiveCheckpoint()
        {
            isActiveCheckpoint = true;
            _animator.SetBool(ActiveCheckpointParam, isActiveCheckpoint);
            GameManager.Instance.UpdateActiveCheckPoint(gameObject);
        }

        public void ResetActiveCheckpoint()
        {
            isActiveCheckpoint = false;
            _wasActiveCheckpoint = true;
            _animator.SetBool(ActiveCheckpointParam, isActiveCheckpoint);
        }
    }
}