using Player;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsRespawning => _isRespawning;
    public static readonly string EndCreditsSceneName = "EndCredits";
    public static readonly string MainMenuSceneName = "MainMenu";
    
    [Header("Scene Managment")]
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private int nextLevelIndex;
    [SerializeField] private float levelTimer;
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
        
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        nextLevelIndex = currentLevelIndex + 1;

        SaveTotalFruitsForThisLevel();
    }

    private void SaveTotalFruitsForThisLevel()
    {
        string totalFruitsKey = $"Level {currentLevelIndex} : TotalFruits";

        // Save it (and keep it updated if your scene changes later)
        PlayerPrefs.SetInt(totalFruitsKey, totalFruits);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        // Check if the player object is missing and we aren't already respawning
        if (playerHealth == null && !_isRespawning)
        {
            _isRespawning = true;
            StartCoroutine(SpawnPlayerCoroutine());
        }
        
        UI_InGame.Instance.UpdateFruitsUI();
        
        levelTimer += Time.deltaTime;
        UI_InGame.Instance.UpdateTImerUI(levelTimer);
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
        UI_InGame.Instance.UpdateFruitsUI();
    }

    public string FruitsInfo() => fruitCollected + " / " + totalFruits;
    
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

    private void SaveFruitInfo()
    {
        // Use ONE consistent key format (no accidental missing/extra spaces)
        string levelFruitKey = $"Level {currentLevelIndex} : Fruits Collected";

        int fruitsCollectedBefore = PlayerPrefs.GetInt(levelFruitKey, 0);
        if (fruitCollected > fruitsCollectedBefore)
        {
            PlayerPrefs.SetInt(levelFruitKey, fruitCollected);
        }

        // If you intend this to be a lifetime bank, consider whether you want to add
        // only the NEW fruits (difference) rather than adding fruitCollected every finish.
        int totalFruitsInBank = PlayerPrefs.GetInt("Total Fruits Amount", 0);
        PlayerPrefs.SetInt("Total Fruits Amount", totalFruitsInBank + fruitCollected);
    }

    private void SaveBestTime()
    {
        // Remove trailing spaces and make the key consistent
        string bestTimeKey = $"Level {currentLevelIndex} : BestTime";

        float previousBest = PlayerPrefs.GetFloat(bestTimeKey, float.MaxValue);
        if (levelTimer < previousBest)
        {
            PlayerPrefs.SetFloat(bestTimeKey, levelTimer);
        }
    }

    public void LevelFinished()
    {
        UnlockNextLevel();
        SaveFruitInfo();
        SaveBestTime();

        // Force PlayerPrefs to flush to disk before scene transition
        PlayerPrefs.Save();

        LoadNextScene();
    }

    private void UnlockNextLevel()
    {
        PlayerPrefs.SetInt("Level " + nextLevelIndex + "is Unlocked.", 1);
        if (NoMoreLevels()) PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
    }

    private void LoadNextLevel() => SceneManager.LoadScene("Level_" + nextLevelIndex);

    private void LoadEndCreditsScene() => SceneManager.LoadScene(EndCreditsSceneName);
    
    private void LoadNextScene()
    {
        UI_FadeInOutVFX fadeEffect = UI_InGame.Instance.GetComponentInChildren<UI_FadeInOutVFX>();
        if (fadeEffect == null) Debug.LogError("No UI_FadeInOutVFX component found in UI_InGame!");
        
        // 2 cuz of scenes: Main Menu and End Credits
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;
        bool noMoreLevels = currentLevelIndex == lastLevelIndex;
        
        if (noMoreLevels) fadeEffect.ScreenFade(1, 1, LoadEndCreditsScene);
        else fadeEffect.ScreenFade(1, 1, LoadNextLevel);
    }

    private bool NoMoreLevels()
    {
        // 2 cuz of scenes: Main Menu and End Credits
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;
        bool noMoreLevels = currentLevelIndex == lastLevelIndex;
        
        return noMoreLevels;
    }
    
    
}