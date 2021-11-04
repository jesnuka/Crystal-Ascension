using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;

    [Header("Enemies")]
    [SerializeField] List<GameObject> enemyList;
    [Tooltip("As time goes up, more varieties of enemies are spawned")]
    [SerializeField] int enemyDifficultyIndex;

    [Header("Difficulty")]
    [SerializeField] float timeSurvived;
    // Over time, increase stats of enemies spawned as well
    [SerializeField] public float speedMultiplier;
    [SerializeField] public int extraBullets;
    [SerializeField] public float fireRateMultiplier;
    [SerializeField] public float damageMultiplier;
    [SerializeField] public float bulletSizeExtraMultiplier;

    // If player gets some upgrade, it can give all enemies lifesteal and health regen, which is added on top of their own ones.
    [SerializeField] public bool allCanLifesteal;
    [SerializeField] public float universalLifesteal;
    [SerializeField] public bool allCanHealthRegen;
    [SerializeField] public float universalHealthRegen;

    [Header("Spawning")]
    [SerializeField] bool gameStarted;
    [SerializeField] public bool gameEnded;
    [Tooltip("Spawn enemies after small delay at first, no instant spawning")]
    [SerializeField] float gameStartTimer;

    [SerializeField] float spawnTimer;
    [SerializeField] float spawnTimerMax;
    [Tooltip("The longer the game goes on, the more frequently enemies are spawned")]
    [SerializeField] float spawnTimerDecreaseMultiplier;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(gameStarted && !gameEnded)
        {
            spawnTimer -= Time.deltaTime;
            timeSurvived += Time.deltaTime;

            HandleDifficulty();

            if (spawnTimer < 0)
            {
                spawnTimer = spawnTimerMax;
                SpawnEnemy();
            }
        }
        else
        {
            gameStartTimer -= Time.deltaTime;

            if (gameStartTimer < 0)
            {
                gameStarted = true;
                UpgradeSpawnManager.instance.gameStarted = true;
            }
        }
       
    }

    private void HandleDifficulty()
    {

        // TODO: Add certain time frames when new enemies will start spawning (Can be randomized between some vlaues, such as 2 minutes etc..)
        // Need to make this into an infinite system, so game always changes even after 1 hour etc.
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyDifficultyIndex);
        GameObject enemyObject = Instantiate(enemyList[index], RandomizeSpawnPoint(), Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.InitiateEnemyStats(speedMultiplier, extraBullets, fireRateMultiplier, damageMultiplier, bulletSizeExtraMultiplier, universalLifesteal, universalHealthRegen);
        
    }


    private Vector2 RandomizeSpawnPoint()
    {
        // TODO: This is wrong, this needs to spawn always outside the screen! then, perhaps add timer before shooting is allowed (Don't shoot from outside screen at first!)
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
        return screenPos;
    }
}
