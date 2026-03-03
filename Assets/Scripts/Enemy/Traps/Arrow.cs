using System;
using Enemy.Traps;
using Player;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private float pushPower;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private bool rotationRight;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] private float respawnCooldown = 5f;

    private bool _isDestroying = false;
    private Animator _animator;
    private int _direction = 1;
    private static readonly int AnimParam = Animator.StringToHash("active");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RotateArrow();
    }

    public void DestroySelf()
    {
        if (_isDestroying) return;
        _isDestroying = true;

        GameObject arrowPrefab = GameManager.Instance.arrowPrefab;
        GameManager.Instance.SpawnObject(arrowPrefab, transform, respawnCooldown);
        
        Destroy(gameObject, destroyDelay);
    }

    private void RotateArrow()
    {
        _direction = rotationRight ? -1 : 1;
        transform.Rotate(Vector3.forward * (_direction * rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        other.GetComponent<PlayerMovementController>().PushPlayer(transform.up * pushPower, duration);
        _animator.SetTrigger(AnimParam);
    }

}