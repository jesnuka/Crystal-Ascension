using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public PlayerController player;
    [SerializeField] public Rigidbody2D rgb;
    [SerializeField] public Material enemyColorMaterial;
    [SerializeField] public ParticleSystem deathParticles;

    [SerializeField] public GameObject enemySprite;

    [Header("Enemy Stats")]
    [SerializeField] public Color enemyColorDefault;
    [SerializeField] public Color enemyColor;

    [SerializeField] public int enemyScore;

    [SerializeField] public float healthMax;
    [SerializeField] public float health;
                     
    [SerializeField] public float healthRegenAmount;
    [SerializeField] public bool canHealthRegen;
                     
    [SerializeField] public float lifeStealAmount;
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
            CountdownTimers();
            MoveEnemy();
            if(canShoot)
                ShootingBehavior();
        }

    }

    #region Spawning
    public void InitiateEnemyStats(float speedMult, int extraBullets, float fireRateMult, float damageMult, float bulletSizeExtraMult, float universalLifesteal, float universalHealthRegen)
    {
        if (universalLifesteal > 0)
            canLifeSteal = true;
        if (universalHealthRegen > 0)
            canHealthRegen = true;

        // health = health * difficultyMultiplier;
        // health = health * difficultyMultiplier;

        speedMultiplier = speedMultiplier + speedMultiplier * speedMult;
        bulletAmount = bulletAmount + extraBullets;
        shootCooldownMax = Mathf.Abs(shootCooldownMax - shootCooldownMax * fireRateMult);
        bulletDamage = bulletDamage + bulletDamage * damageMult;
        bulletSizeMultiplier = bulletSizeMultiplier + bulletSizeExtraMult;

        lifeStealAmount = universalLifesteal;
        healthRegenAmount = universalHealthRegen;

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
            health -= damage;
        }
    }

    private void EnemyDeath()
    {
        Instantiate(deathParticles, this.transform.position, Quaternion.identity);
        enemySprite.SetActive(false);
        CalculateScore();
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
