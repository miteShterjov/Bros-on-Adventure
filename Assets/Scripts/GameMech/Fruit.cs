using Managers;
using UnityEngine;

namespace GameMech
{
    public class Fruit : MonoBehaviour
    {
        private enum FruitType
        {
            Apple,
            Banana,
            Cherry,
            Kiwi,
            Melon,
            Orange,
            Pineapple,
            Strawberry
        }

        [Header("Config")]
        [SerializeField, Tooltip("The type of fruit to spawn.")] private FruitType fruitType;
        [SerializeField, Tooltip("If true, randomizes the fruit sprite.")] private bool randomFruit;
    
        private readonly int _collectedAnimParam = Animator.StringToHash("collected");
        private readonly int _randomIndexParam = Animator.StringToHash("index");
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
    
        private void Start()
        {
            _animator.SetFloat(_randomIndexParam, randomFruit ? GetRandomIndex() : ReturnFruitTypeIndex(fruitType));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) GameManager.Instance.AddFruit();
            _animator.SetTrigger(_collectedAnimParam);
        }
    
        private int GetRandomIndex() => Random.Range(0, 8);

        private int ReturnFruitTypeIndex(FruitType fruitTypeSelected)
        {
            return fruitTypeSelected switch
            {
                FruitType.Apple => 0,
                FruitType.Banana => 1,
                FruitType.Cherry => 2,
                FruitType.Kiwi => 3,
                FruitType.Melon => 4,
                FruitType.Orange => 5,
                FruitType.Pineapple => 6,
                FruitType.Strawberry => 7,
                _ => GetRandomIndex()
            };
        }
    }
}
