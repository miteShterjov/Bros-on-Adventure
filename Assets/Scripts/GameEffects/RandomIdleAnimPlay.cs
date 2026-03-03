using UnityEngine;

namespace GameEffects
{
    public class RandomIdleAnimPlay : MonoBehaviour
    {
        private Animator _animator;

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            _animator.Play(stateInfo.fullPathHash, -1, Random.Range(0f, 1f));
        }
    }
}