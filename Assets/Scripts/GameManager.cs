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
    [SerializeField] public PlayerHealthController playerHealth;
    [SerializeField] private GameObject currentCheckpoint;
    [Space][Header("Fruits Collection")]
    [SerializeField] private int fruitCollected;
    [SerializeField] private int totalFruits;
    [SerializeField] private Fruit[] allFruits;
    [Header("Traps")] 
    [SerializeField] public GameObject arrowPrefab;
    
    private bool _isRespawning;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        playerHealth = FindFirstObjectByType<PlayerHealthController>();
    }

    private void Start()
    {
        playerSpawnPoint = FindAnyObjectByType<StartCheckPoint>().transform;
        
        fruitCollected = 0;
        allFruits = FindObjectsByType<Fruit>(sortMode: FindObjectsSortMode.None);
        totalFruits = allFruits.Length;
    }

    private void Update()
    {
        // Check if the player object is missing and we aren't already respawning
        if (playerHealth == null && !_isRespawning)
        {
            _isRespawning = true;
            StartCoroutine(SpawnPlayerCoroutine());
        }
    }

    public void SpawnObject(GameObject prefab, Transform spawnPoint, float delay = 0) => 
        StartCoroutine(SpawnObjectCoroutine(prefab, spawnPoint, delay));

    private IEnumerator SpawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(playerRespawnDelay);

        Vector3 spawnPos = currentCheckpoint == null ? playerSpawnPoint.position : currentCheckpoint.transform.position;

        GameObject newPlayer = Instantiate(
            playerPrefab, 
            spawnPos, 
            Quaternion.identity
            );

        // Update the reference so the Update loop knows the player is back
        playerHealth = newPlayer.GetComponent<PlayerHealthController>();

        // Short buffer to prevent immediate double-respawn if something goes wrong
        yield return new WaitForSeconds(0.1f); 
        _isRespawning = false;
    }

    private IEnumerator SpawnObjectCoroutine(GameObject obj, Transform spawnPoint, float delay)
    {
        Vector3 spawnPosition = spawnPoint.position;
        yield return new WaitForSeconds(delay);
        GameObject spawnedObject = Instantiate(obj, spawnPosition, Quaternion.identity);
    }

    public void AddFruit()
    {
        fruitCollected++;
        print(fruitCollected + " of " + totalFruits + " fruits collected!");
    }
    
    public void UpdateActiveCheckPoint(GameObject checkPoint)
    {
        if (currentCheckpoint == checkPoint) return;

        if (currentCheckpoint != null)
        {
            Checkpoint oldCheckpoint = currentCheckpoint.GetComponent<Checkpoint>();
            if (oldCheckpoint != null)
            {
                oldCheckpoint.ResetActiveCheckpoint();
            }
        }
        
        currentCheckpoint = checkPoint;
    }
}