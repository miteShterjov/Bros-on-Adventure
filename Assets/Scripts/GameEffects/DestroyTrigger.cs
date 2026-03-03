using System;
using Player;
using UnityEngine;

namespace GameEffects
{
    public class DestroyTrigger : MonoBehaviour
    {
        [SerializeField] private float destroyDelay = 0.2f;
        private void OnTriggerEnter2D(Collider2D other) => Destroy(other.gameObject, destroyDelay);
    }
}