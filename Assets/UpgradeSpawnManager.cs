using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSpawnManager : MonoBehaviour
{
    public static UpgradeSpawnManager instance;

    [Header("UI")]
    [SerializeField] GameObject upgradeScreen;

    [SerializeField] TMP_Text upgradeTierText1;
    [SerializeField] TMP_Text upgradeTierText2;

    [SerializeField] TMP_Text positiveUIText;
    [SerializeField] TMP_Text positiveUIValueText;
    [SerializeField] TMP_Text negativeUIText;
    [SerializeField] TMP_Text negativeUIValueText;

    [SerializeField] public bool upgradeScreenOpen;

    [Header("Upgrades")]
    // GameObjects to spawn that contain the Upgrade scripts to pickup
    /* [SerializeField] List<GameObject> positiveUpgrades1;
     [SerializeField] List<GameObject> positiveUpgrades2;
     [SerializeField] List<GameObject> positiveUpgrades3;

     [SerializeField] List<GameObject> negativeUpgrades1;
     [SerializeField] List<GameObject> negativeUpgrades2;
     [SerializeField] List<GameObject> negativeUpgrades3;*/
    // TODO: There should maybe be tier 1 2 and 3 lists instead. Then those contain the data that is negative and positive in those tiers (in Upgrade script)

    [SerializeField] List<GameObject> tierUpgrades1;
    [SerializeField] List<GameObject> tierUpgrades2;
    [SerializeField] List<GameObject> tierUpgrades3;

    [Tooltip("As time goes up, more varieties of enemies are spawned")]
    [SerializeField] int enemyDifficultyIndex;



    [SerializeField] public Upgrade currentUpgrade;

    [Header("Spawning")]
    [SerializeField] int difficultyIndex;
    [SerializeField] float timeSurvived;
    [SerializeField] float timeSurvivedExtreme;
    [SerializeField] public bool gameStarted;
    [SerializeField] public bool gameEnded;
    [SerializeField] float spawnTimer;
    [SerializeField] float spawnTimerMax;
    [Tooltip("The longer the game goes on, the more frequently upgrades are spawned")]
    [SerializeField] float spawnTimerDecreaseMultiplier;
    [Tooltip("The longer an upgrade does not spawn, the more higher the chance for one to spawn is. This resets everytime one spawns")]
    [SerializeField] float spawnTimerDecreaseTimer;
    [SerializeField] float tierImprovementTimer;
    [SerializeField] float upgradeValueMultiplier;
    [SerializeField] float upgradeValueImprovementTimer;
    [SerializeField] float chanceCeiling;
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
        if (gameStarted && !gameEnded)
        {
            spawnTimer -= Time.deltaTime;
            timeSurvived += Time.deltaTime;

            if (difficultyIndex <= 5)
                HandleDifficulty();
            else
                HandleDifficultyExtreme();

            chanceCeiling = (spawnTimerDecreaseMultiplier * timeSurvived / 500000f);
            spawnTimerDecreaseTimer += Time.deltaTime * 0.01f;
            tierImprovementTimer += Time.deltaTime * 0.01f;
            upgradeValueImprovementTimer += Time.deltaTime * 0.01f;
            /*if (spawnTimer < 0)
            {
                spawnTimer = spawnTimerMax;
                SpawnUpgrade();
            }*/
        }
    }

    private void HandleDifficulty()
    {
        // Difficulty increases two values, that multiply enemy spawn rate and reward values, as well as up the amount of unique enemies
        // First difficulty increases with specific steps, after that, it increases after every 4 minutes but with no new enemies

        if(timeSurvived >= 1200)
        {
            if (difficultyIndex == 5)
            {
                difficultyIndex = 6;
                spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
                upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
                enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
            }

        }
        else if(timeSurvived < 1200 && timeSurvived >= 960) // After 16 minutes
        {
            if (difficultyIndex == 4)
            {
                difficultyIndex = 5;
                spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
                upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
                enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
            }

        }
        else if (timeSurvived < 960 && timeSurvived >= 720) // After 12 minutes
        {
            if (difficultyIndex == 3)
            {
                difficultyIndex = 4;
                spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
                upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
                enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
            }

        }
        else if (timeSurvived < 720 && timeSurvived >= 480) // After 8 minutes
        {
            if (difficultyIndex == 2)
            {
                Debug.Log("Increase! 8");
                difficultyIndex = 3;
                spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
                upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
                enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
            }

        }
        else if (timeSurvived < 480 && timeSurvived >= 240) // After 4 minutes
        {
            if (difficultyIndex == 1)
            {
                Debug.Log("Increase! 4");
                difficultyIndex = 2;
                spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
                upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
                enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
            }
        }
        else if (timeSurvived < 240 && timeSurvived >= 60) // After 1 minute
        {
            // This is performed only once
            if(difficultyIndex == 0)
            {
                Debug.Log("Increase! 1");
                difficultyIndex = 1;
                spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
                upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
                enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.enemyDifficultyIndex += 1;
                EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
            }


        }
        else
            difficultyIndex = 0;
    }

    private void HandleDifficultyExtreme()
    {
        timeSurvivedExtreme += Time.deltaTime;

        if(timeSurvivedExtreme >= 240) // 4 minutes have passed
        {
            spawnTimerDecreaseMultiplier = spawnTimerDecreaseMultiplier * 1.25f;
            upgradeValueMultiplier = upgradeValueMultiplier * 1.25f;
            timeSurvivedExtreme = 0f;
            EnemySpawnManager.instance.UpdateEnemyStatMultiplier();
        }
    }

    public void SpawnUpgrade(Vector2 position)
    {
        // TODO: Add this
        // ENemies call this when they die. If upgrade can be spawned, spawn it.
        float upgradeSpawnChance = 0;
        // 500 000f is about 5 - 6 days. Probably a better way to calculate this so timeSurvived matters, but this works for now.
        // If someone survives for 5 days, all enemies always drop upgrades basically
        upgradeSpawnChance = Random.Range(0f, 1f);
        //Debug.Log("SpawnChance is now: " + (chanceCeiling + spawnTimerDecreaseTimer));
        if (upgradeSpawnChance <= (chanceCeiling + spawnTimerDecreaseTimer))
        {
            // Spawn upgrade
        //    Debug.Log("Chance ceiling was:" + chanceCeiling);
            spawnTimerDecreaseTimer = 0f;
            Instantiate(ChooseUpgradeGameobject(), position, Quaternion.identity);
        }
        else
        {
            // Don't
        }
    }

    private GameObject ChooseUpgradeGameobject()
    {
        // Randomize the item tier again here
        GameObject upgradeObject = null;
        float positiveValue = 0;
        float negativeValue = 0;

        float tierChance = 0;
        tierChance = Random.Range(0f, 1f);

        if(tierChance + tierImprovementTimer > 0.95f)
        {
            Debug.Log("Tier 3");
            // Tier 3 reward
            // Needs to be above 0.95 %
            int index = Random.Range(0, tierUpgrades3.Count);
            upgradeObject = tierUpgrades3[index];

            if (tierImprovementTimer - 0.5f >= 0f)
                tierImprovementTimer -= 0.5f;
            else
                tierImprovementTimer = 0f;

            // Randomize the positive and negative values
            positiveValue = Random.Range(0, upgradeValueMultiplier * upgradeValueImprovementTimer);
            negativeValue = -1 * Random.Range(0, upgradeValueMultiplier * upgradeValueImprovementTimer);
        }
        else if((tierChance + tierImprovementTimer > 0.75f) && (tierChance + tierImprovementTimer <= 0.95f))
        {
            Debug.Log("Tier 2");
            // Tier 2 reward
            // Needs to be between 0.75 % - 0.95 %
            int index = Random.Range(0, tierUpgrades2.Count);
            upgradeObject = tierUpgrades2[index];

            if (tierImprovementTimer - 0.25f >= 0f)
                tierImprovementTimer -= 0.25f;
            else
                tierImprovementTimer = 0f;

            // Randomize the positive and negative values
            positiveValue = Random.Range(0, upgradeValueMultiplier * upgradeValueImprovementTimer);
            negativeValue = -1 * Random.Range(0, upgradeValueMultiplier * upgradeValueImprovementTimer);
        }
        else
        {
            Debug.Log("Tier 1");
            // Tier 1 reward
            // Anything less than 0.75 %
            int index = Random.Range(0, tierUpgrades1.Count);
            upgradeObject = tierUpgrades1[index];

            // Randomize the positive and negative values
            positiveValue = Random.Range(0, upgradeValueMultiplier * upgradeValueImprovementTimer);
            negativeValue = -1 * Random.Range(0, upgradeValueMultiplier * upgradeValueImprovementTimer);
        }


        upgradeObject.GetComponent<Upgrade>().InitializeUpgrade(positiveValue, negativeValue);

        return upgradeObject;
    }

    public void AcceptUpgrade()
    {
        Debug.Log("Upgrade accepted");
        currentUpgrade.PickupUpgradeObject();
        currentUpgrade = null;
        CloseUpgradeScreen();
    }

    public void DeclineUpgrade()
    {
        Debug.Log("Upgrade declined");
        currentUpgrade.DestroyUpgradeObject();
        currentUpgrade = null;
        CloseUpgradeScreen();
    }

    public void OpenUpgradeScreen(Upgrade upgradeScript)
    {
        upgradeScreenOpen = true;
        currentUpgrade = upgradeScript;

        // TODO: Make sure to handle situations with multiple upgrades one by one, not all at once!
        upgradeScreen.SetActive(true);

        // Tier is basically just stronger upgrades, such as lifesteal etc.
        upgradeTierText1.text = "Tier " + upgradeScript.upgradeTier.ToString();
        upgradeTierText2.text = "Tier " + upgradeScript.upgradeTier.ToString();

        positiveUIText.text = upgradeScript.positiveString;
        positiveUIValueText.text = upgradeScript.positiveValue.ToString("F2");
        negativeUIText.text = upgradeScript.negativeString;
        negativeUIValueText.text = upgradeScript.negativeValue.ToString("F2");

        Time.timeScale = 0f;
    }

    public void CloseUpgradeScreen()
    {
        if (!PlayerController.instance.menuOpen)
            Time.timeScale = 1f;

        upgradeTierText1.text = "Tier ";
        upgradeTierText2.text = "Tier ";

        positiveUIText.text = "";
        positiveUIValueText.text = "";
        negativeUIText.text = "";
        negativeUIValueText.text = "";
        upgradeScreen.SetActive(false);

        upgradeScreenOpen = false;
    }
}
