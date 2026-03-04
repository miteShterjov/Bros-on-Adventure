using Player;
using UnityEngine;

namespace Enemy.Traps
{
    public class Trampoline : MonoBehaviour
    {
        [Header("Config")] 
        [SerializeField] private Vector2 direction;
        [SerializeField] private float duration = 0.5f;
        
        private static readonly int TrampolineParam = Animator.StringToHash("active");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerMovementController player = other.GetComponent<PlayerMovementController>();
            if (player == null) return;

            player.PushPlayer(direction, duration);
            
            _animator.SetTrigger(TrampolineParam);
        }
    }
}