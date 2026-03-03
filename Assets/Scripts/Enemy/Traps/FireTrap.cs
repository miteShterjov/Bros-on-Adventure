using System;
using UnityEngine;

namespace Enemy.Traps
{
    public class FireTrap : MonoBehaviour
    {
        public bool IsActive { get => isActive; set => isActive = value;}
        
        private static readonly int IsActiveParam = Animator.StringToHash("isActive");

        [SerializeField] private bool isActive;
        private Animator _animator;
        private Collider2D _collider;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            _animator.SetBool(IsActiveParam, isActive);
            _collider.enabled = isActive;
        }
    }
}