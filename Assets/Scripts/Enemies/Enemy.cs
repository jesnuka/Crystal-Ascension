using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public PlayerController player;
    [SerializeField] public Material enemyColorMaterial;
    [SerializeField] public ParticleSystem deathParticles;

    [SerializeField] public GameObject enemySprite;
    [SerializeField] public GameObject enemySpriteColored;

    [Header("Enemy Stats")]
    [SerializeField] public Color enemyColorDefault;
    [SerializeField] public Color enemyColor;

    [SerializeField] public int enemyScore;

    [SerializeField] public float healthMax;
    [SerializeField] public float health;
                     

    // Health regen is not even used currently for enemies
    [SerializeField] public float healthRegenAmount;
    [SerializeField] public float healthRegenAmountTrue; // This is actually used, other is used to check if it is enough for healthRegen (over 1.0f)
    [SerializeField] public bool canHealthRegen;
                     
    [SerializeField] public float lifeStealAmount;
    [SerializeField] public float lifeStealAmountTrue; // This is actually used, other is used to check if it is enough for lifesteal (over 1.0f)
    [SerializeField] public bool canLifeSteal;
                      
    [SerializeField] public float speedMultiplier;
    [SerializeField] public float enemyMaxVelocity;
                    
                     
    [SerializeField] public bool isDead;
    [SerializeField] public bool isInGameSpace;

    [Header("Death Related")]
    [Tooltip("How long to perform actions until gameObject is destroyed after death")]
    [SerializeField] public float deathTimer;
    [SerializeField] public bool startDeathTimer;

    [Header("Bullets")]
    [Tooltip("Enemy can't shoot until it is inside the screen")]
    [SerializeField] public bool canShoot;
    [SerializeField] public float distanceToPlayer;

    [SerializeField] public GameObject bulletObject;
    [SerializeField] public Color bulletColor;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float bulletAmount;
    [SerializeField] public float bulletSpread;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletLifetime;
    [SerializeField] public float bulletBounces;
    [SerializeField] public float bulletSizeMultiplier;

    [SerializeField] public float shootCooldown;
    [SerializeField] public float shootCooldownMax;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        // TODO: Add ENEMY SPAWNER, that creates the enemy health etc based on the original values, but also based on time survived!
        ChildUpdate();
        if(!isDead)
        {
            if(!canShoot)
            {
                // Don't allow shooting until close enough to the player, meaning enemy is visible
               // distanceToPlayer = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
              //  if (distanceToPlayer < Screen.width)
                if (enemySpriteColored.GetComponent<Renderer>().isVisible)
                {
                    Debug.Log("Can shoot now!");
                    canShoot = true;
                }
            }
            CountdownTimers();
            MoveEnemy();
            if(canShoot)
                ShootingBehavior();
        }

    }

    #region Spawning
    public void InitiateEnemyStats(float speedMult, int extraBullets, float fireRateMult, float damageMult, float bulletSizeExtraMult, float universalLifesteal, float universalHealthRegen, float healthMultiplier)
    {
        if (universalLifesteal > 0)
            canLifeSteal = true;
        if (universalHealthRegen > 0)
            canHealthRegen = true;

        // health = health * difficultyMultiplier;
        // health = health * difficultyMultiplier;

        healthMax = Mathf.Abs(healthMax + healthMax * (healthMultiplier/10f));
        health += Mathf.Abs(healthMax * (healthMultiplier / 10f));

        speedMultiplier = Mathf.Abs(speedMultiplier + speedMultiplier * (speedMult/10f));
        //bulletAmount = bulletAmount + extraBullets; Not used for now

        if ((shootCooldownMax - shootCooldownMax * (fireRateMult / 100f)) <= 0)
            shootCooldownMax = 0.05f;
        else
            shootCooldownMax = Mathf.Abs(shootCooldownMax - shootCooldownMax * (fireRateMult/100f)); // Just incase to avoid insane fire rate, this is done.

        bulletDamage = Mathf.Abs(bulletDamage + bulletDamage * (damageMult/10f));
        bulletSizeMultiplier = Mathf.Abs(bulletSizeMultiplier + bulletSizeMultiplier * (bulletSizeExtraMult/10f));

        lifeStealAmount = lifeStealAmount + lifeStealAmount * universalLifesteal;
        healthRegenAmount = lifeStealAmount + lifeStealAmount * universalHealthRegen;
        if (lifeStealAmount > 1.0f)
        {
            canLifeSteal = true;
            lifeStealAmountTrue = lifeStealAmount;
        }
        if (healthRegenAmount > 1.0f)
        {
            canHealthRegen = true;
            healthRegenAmountTrue = healthRegenAmount;
        }

        // TODO: All stats should be randomized from starting value to the difficultymultiplier values maybe?
        // DOES NOT WORK, too random!!
        // INSTEAD, maybe enemies should be "Generated", and those varieties have different colors, and never change in stats (Unless specified by an upgrade).
        // So noob-enemies will still spawn later on as well, for more score.
    }
    #endregion

    #region Health

    public void healEnemy(float amount)
    {
        if (health < healthMax)
        {
            if ((health + amount) < healthMax)
            {
                health += amount;
            }
            else if ((health + amount) >= healthMax)
            {
                health = healthMax;
            }

        }
    }

    public void TakeDamage(float damage)
    {
        if (health - damage<= 0)
        {
            isDead = true;
            EnemyDeath();
        }
        else
        {
            SoundManager.instance.PlaySoundOnce("enemyTakeDamage", Vector3.zero, this.gameObject, true);
            health -= damage;
        }
    }

    private void EnemyDeath()
    {
        Debug.Log("Play enemy death");
        SoundManager.instance.PlaySoundOnce("enemyDeath", Vector3.zero, null, true);
        Instantiate(deathParticles, this.transform.position, Quaternion.identity);
        enemySprite.SetActive(false);
        CalculateScore();
        UpgradeSpawnManager.instance.SpawnUpgrade(transform.position);
        EnemySpawnManager.instance.enemyAmountCurrent -= 1;
        EnemyDeathExtra();
    }
    #endregion

    #region Score
    private void CalculateScore()
    {
        player.AddScore(enemyScore);
    }
    #endregion

    #region Timers
    private void CountdownTimers()
    {
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
    }
    #endregion

    abstract public void EnemyDeathExtra();
    abstract public void MoveEnemy();
    abstract public void ShootingBehavior();

    // Separate update for the child classes, to allow for unique behavior
    abstract public void ChildUpdate();
}
