using UnityEngine;

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
        if (randomFruit) _animator.SetFloat(_randomIndexParam, GetRandomIndex());
        else _animator.SetFloat(_randomIndexParam, ReturnFruitTypeIndex(fruitType));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.AddFruit();
        _animator.SetTrigger(_collectedAnimParam);
    }
    
    private int GetRandomIndex() => Random.Range(0, 8);

    private int ReturnFruitTypeIndex(FruitType fruitTypeSelected)
    {
        switch (fruitTypeSelected)
        {
            case FruitType.Apple: return 0;
            case FruitType.Banana: return 1;
            case FruitType.Cherry: return 2;
            case FruitType.Kiwi: return 3;
            case FruitType.Melon: return 4;
            case FruitType.Orange: return 5;
            case FruitType.Pineapple: return 6;
            case FruitType.Strawberry: return 7;
            default: return GetRandomIndex();
        }
    }
}
