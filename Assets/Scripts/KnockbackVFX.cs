using System;
using System.Collections;
using Player;
using UnityEngine;

public class KnockbackVFX : MonoBehaviour
{
    public bool IsKnocked => isKnocked;
    public bool CanBeKnocked => canBeKnocked;
    
    [Header("Config")]
    [SerializeField] private float knockbackDuration = 1f;
    [SerializeField] private Vector2 knockbackPower;
    [SerializeField] private bool isKnocked;
    [SerializeField] private bool canBeKnocked = true;
    
    private Rigidbody2D _rigidbody;
    private PlayerAnimationController _playerAnim;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(_rigidbody != null, "Rigidbody2D is null");
        _playerAnim = GetComponent<PlayerAnimationController>();
        Debug.Assert(_playerAnim != null, "PlayerAnimationController is null");
    }

    public void Knockback()
    {
        if (!canBeKnocked) return;
        StartCoroutine(KnockbackCo());
        _rigidbody.linearVelocity = new Vector2(knockbackPower.x * -_playerAnim.FacingDirection, knockbackPower.y);
    }
    
    private IEnumerator KnockbackCo()
    {
        isKnocked = true;
        canBeKnocked = false;
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        canBeKnocked = true;
    }
}
