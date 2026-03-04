using Enemy.Traps;
using Player;
using UnityEngine;

namespace GameMech
{
    public class FireTrapLever : MonoBehaviour
    {
        [Header("Lever")]
        [SerializeField] private Sprite leverOnSprite;
        [SerializeField] private Sprite leverOffSprite;
        [SerializeField, Tooltip("Put Fire Traps in this that you want to be affected by this lever.")] private FireTrap[] fireTraps;
        [SerializeField] private bool trapIsActive;

        private SpriteRenderer _spriteRenderer;
        private bool _playerCanInteract;
        private PlayerMovementController _playerMovement;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _playerMovement = FindFirstObjectByType<PlayerMovementController>();
            
            if (fireTraps != null) trapIsActive = fireTraps[0].IsActive;

            _spriteRenderer.sprite = trapIsActive ? leverOnSprite : leverOffSprite;
        }

        private void Update()
        {
            if (!_playerCanInteract || !_playerMovement.PlayerInteracts()) return;
            trapIsActive = !trapIsActive;
            UpdateTraps();
        }

        private void UpdateTraps()
        {
            foreach (FireTrap fireTrap in fireTraps) fireTrap.IsActive = trapIsActive;
            
            _spriteRenderer.sprite = trapIsActive ? leverOnSprite : leverOffSprite;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerCanInteract = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerCanInteract = false;
        }
    }
}