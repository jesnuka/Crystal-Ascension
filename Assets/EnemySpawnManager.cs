using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] List<GameObject> enemyList;
    [Tooltip("As time goes up, more varieties of enemies are spawned")]
    [SerializeField] int enemyDifficultyIndex;

    [Header("Difficulty")]
    [SerializeField] float timeSurvived;
    // Over time, increase stats of enemies spawned as well
    [SerializeField] float speedMultiplier;
    [SerializeField] int extraBullets;
    [SerializeField] float fireRateMultiplier;
    [SerializeField] float damageMultiplier;

    // If player gets some upgrade, it can give all enemies lifesteal and health regen, which is added on top of their own ones.
    [SerializeField] bool allCanLifesteal;
    [SerializeField] float universalLifesteal;
    [SerializeField] bool allCanHealthRegen;
    [SerializeField] float universalHealthRegen;

    [Header("Spawning")]
    [SerializeField] bool gameStarted;
    [Tooltip("Spawn enemies after small delay at first, no instant spawning")]
    [SerializeField] float gameStartTimer;

    [SerializeField] float spawnTimer;
    [SerializeField] float spawnTimerMax;
    [Tooltip("The longer the game goes on, the more frequently enemies are spawned")]
    [SerializeField] float spawnTimerDecreaseMultiplier;

    // Update is called once per frame
    void Update()
    {
        if(gameStarted)
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
                gameStarted = true;
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
        enemy.InitiateEnemyStats(speedMultiplier, extraBullets, fireRateMultiplier, damageMultiplier, universalLifesteal, universalHealthRegen);
        
    }


    private Vector2 RandomizeSpawnPoint()
    {
        // TODO: This is wrong, this needs to spawn always outside the screen! then, perhaps add timer before shooting is allowed (Don't shoot from outside screen at first!)
        Vector2 screenPos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
        return screenPos;
    }
}
