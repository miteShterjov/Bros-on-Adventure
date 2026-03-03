using System;
using UnityEngine;

public class StartCheckPoint : MonoBehaviour
{
    private static readonly int PlayerPassAnimTrigger = Animator.StringToHash("playerPassed");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _animator.SetTrigger(PlayerPassAnimTrigger);
    }
}
