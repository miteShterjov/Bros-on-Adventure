using System;
using UnityEngine;

public class EndCheckPoint : MonoBehaviour
{
    private Animator _animator;
    private static readonly int LevelFinishedAnimTEvent = Animator.StringToHash("pressed");
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) RunLevelFinished();
    }

    private void RunLevelFinished()
    {
        _animator.SetTrigger(LevelFinishedAnimTEvent);
        print("Level finished!");
        // when levels redo should load next scene!
        GameManager.Instance.LevelFinished();
    }
}
