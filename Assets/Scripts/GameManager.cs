using System;
using Player;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsRespawning => _isRespawning;
    
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private float playerRespawnDelay = 2f;
    [SerializeField] public PlayerMovementController player;
    [Header("Fruits Collection")]
    [SerializeField] private int fruitCollected;

    private bool _isRespawning;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        player = FindFirstObjectByType<PlayerMovementController>();
    }

    private void Update()
    {
        // when hp logic is implement change player===null to hp<=0
        if (player == null && !_isRespawning)
        {
            _isRespawning = true;
            StartCoroutine(SpawnPlayerCoroutine());
        }
    }

    private IEnumerator SpawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(playerRespawnDelay);
        
        GameObject newPlayer = Instantiate(
            playerPrefab, 
            playerSpawnPoint.position, 
            Quaternion.identity
        );
        
        player = newPlayer.GetComponent<PlayerMovementController>();

        yield return new WaitForSeconds(playerRespawnDelay / 2);
        
        _isRespawning = false;
    }

    

    public void AddFruit() => fruitCollected++;
}