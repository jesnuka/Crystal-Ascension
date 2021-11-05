using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("References")]
    [SerializeField] Rigidbody2D rgb;
    [SerializeField] SpriteRenderer playerSpriteRenderer_Color;
    [SerializeField] SpriteRenderer playerSpriteRenderer_White;
    [SerializeField] Material playerColorMaterial;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] GameObject deathParticles;
    [SerializeField] GameObject playerSprite;

    [Header("Menus")]
    [Tooltip("Each round randomizes a text from here to display at the end")]
    [SerializeField] List<String> deathDescriptions = new List<string>();
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseMenuConfirmBox;
    [SerializeField] public bool menuOpen;

    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject deathCounterSkipButton;
    [SerializeField] GameObject deathScreenButtons;
    [SerializeField] TMP_Text deathDescription;
    [SerializeField] TMP_Text deathSurvivedText;
    [SerializeField] TMP_Text numTime;
    [SerializeField] TMP_Text deathValourText;
    [SerializeField] TMP_Text numValour;
    [SerializeField] TMP_Text deathHeroismText;
    [SerializeField] TMP_Text deathHeroismGained;

    [SerializeField] int totalHeroism;
    [SerializeField] int deathMenuCounter;
    [SerializeField] bool deathMenuTimerOn;
    [SerializeField] float deathHeroismSpeedupTimer;
    [SerializeField] float deathMenuTimer;
    [SerializeField] float deathMenuTimerMax;

    [SerializeField] float deathMenuWait;

    [Header("Buttons")]
    [SerializeField] KeyCode pauseKey1;
    [SerializeField] KeyCode pauseKey2;
    [SerializeField] KeyCode pauseKey3;
    [SerializeField] public KeyCode pickupKey;

    [Header("Movement")]
    [SerializeField] Vector2 movement;
    [SerializeField] float zRotation;

    [Header("Player Values")]
    [SerializeField] public int upgradesTaken;
    [SerializeField] float timeSurvived;
    [ColorUsage(true, true)] [SerializeField] Color playerSpriteColorDefault;
    [ColorUsage(true, true)] [SerializeField] Color playerSpriteColorInvis;
    [ColorUsage(true, true)] [SerializeField] Color playerSpriteColor;
    [ColorUsage(true, true)] [SerializeField] Color playerColorDefault;
    [ColorUsage(true, true)] [SerializeField] Color playerColorHurt;
    [ColorUsage(true, true)] [SerializeField] Color playerColor;
  //  [SerializeField] float colorLerpTimer;
  //  [SerializeField] float colorLerpTimerMax;

    [SerializeField] int score;
    [SerializeField] public int scoreMultiplier;
    [Tooltip("How much more damage the player takes")]
    [SerializeField] public float frailnessMultiplier;
    [SerializeField] public float invincibilityLengthMax;
    [SerializeField] public float invincibilityLengthCurrent;
    [SerializeField] public float luckiness;
    [SerializeField] public float healthMax;
    [SerializeField] public float health;
    [SerializeField] public float healthRegenAmount;

    // To counter how OP health regen is, there is always a delay, which is consistent 
    [SerializeField] public float healthRegenDelay;
    [SerializeField] public float healthRegenDelayMax;

    [SerializeField] public float lifeStealAmount;


                      
    [SerializeField] public bool canHealthRegen;
    [SerializeField] public bool canLifeSteal;
                      
    [SerializeField] public float speedMultiplier;
    [SerializeField] public float playerMaxVelocity;


    [SerializeField] public bool isDead;
    [SerializeField] bool isInGameSpace;

    [Header("Bullets")]
    [SerializeField] GameObject bulletObject;
    [SerializeField] Color bulletColor;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float bulletAmount;
    [SerializeField] public float bulletSpread;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletLifetime;
    [SerializeField] public float bulletBounces;
    [SerializeField] public float bulletSizeMultiplier;
                     
    [SerializeField] public float shootCooldown;
    [SerializeField] public float shootCooldownMax;

    [Header("UI References")]

    // TODO: Add tooltips to pause menu for these!

    // Upper UI
    [SerializeField] TMP_Text text_playerHealthUI;
    [SerializeField] TMP_Text text_playerScoreUI;

    // Player values
    [SerializeField] TMP_Text text_playerMaxHealth;
    [SerializeField] TMP_Text text_playerHealth;
    [SerializeField] TMP_Text text_playerSpeed;
    [SerializeField] TMP_Text text_playerRegen;
    [SerializeField] TMP_Text text_playerLifesteal;
    [SerializeField] TMP_Text text_playerTimeSurvived;
    [SerializeField] TMP_Text text_playerFrailness;
    [SerializeField] TMP_Text text_playerInvincibilityFrames;
    [SerializeField] Image bar_playerInvincibilityFrames;
    [SerializeField] TMP_Text text_playerLuckiness;
    [SerializeField] TMP_Text text_playerValourMultiplier;

    // Bullet values
    [SerializeField] TMP_Text text_bulletDamage;
    [SerializeField] TMP_Text text_bulletAmount;
    [SerializeField] TMP_Text text_bulletSpeed;
    [SerializeField] TMP_Text text_bulletCooldown;
    [SerializeField] Image bar_bulletCooldown;

    [SerializeField] TMP_Text text_bulletSpread;
    [SerializeField] TMP_Text text_bulletLifetime;
    [SerializeField] TMP_Text text_bulletBounces;
    [SerializeField] TMP_Text text_bulletSizeMultiplier;




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

        ChangePlayerColor(playerColorDefault, playerSpriteColorDefault);
    }

    private void Start()
    {
        UpdatePlayerStatsUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0 && !isDead)
        {
            timeSurvived += Time.deltaTime;

            UpdatePlayerUI();
            GetInput();
            if(isInGameSpace)
                MovePlayer();

            CheckPlayerInGameSpace();

           // if (colorLerpTimer > 0) // This is also invicibility timer
            if (invincibilityLengthCurrent > 0) // This is also invicibility timer
            {
                //  playerCollider.enabled = false;


                // Instead, ignore collision betwen player and enemy + enemyBullets
                Physics2D.IgnoreLayerCollision(8, 11, true);
                Physics2D.IgnoreLayerCollision(8, 12, true);
                LerpPlayerColor();
            }
            else
            {
                ChangePlayerColor(playerColorDefault, playerSpriteColorDefault);
                // Dont disable, doesn't allow picking up items
                // playerCollider.enabled = true;

                Physics2D.IgnoreLayerCollision(8, 11, false);
                Physics2D.IgnoreLayerCollision(8, 12, false);
            }
            if(healthRegenDelay > 0)
                healthRegenDelay -= Time.deltaTime;

            if (healthRegenDelay <= 0) // Once delay is ready, can heal
            {
                if (healthRegenAmount > 0)
                    canHealthRegen = true;
                else
                    canHealthRegen = false;

                if (lifeStealAmount > 0)
                    canLifeSteal = true;
                else
                    canLifeSteal = false;

                if (canHealthRegen)
                    healPlayer(healthRegenAmount);
            }
        }
        else if(health <= 0 && !isDead)
        {
            TakeDamage(-5000);
        }

        if(deathMenuTimerOn)
        {
            deathHeroismSpeedupTimer += Time.unscaledDeltaTime;

            if (deathMenuTimer >= 0)
                deathMenuTimer -= Time.unscaledDeltaTime;
            else
            {
                if (deathHeroismSpeedupTimer > 25)
                    HeroismCounter(1000);
                else if(deathHeroismSpeedupTimer > 20)
                    HeroismCounter(500);
                else if(deathHeroismSpeedupTimer > 10)
                    HeroismCounter(100);
                else if (deathHeroismSpeedupTimer > 5)
                    HeroismCounter(10);
                else if (deathHeroismSpeedupTimer > 2)
                    HeroismCounter(2);
                else
                    HeroismCounter(1);
            }
        }
    }


    public void ChangePlayerColor(Color newColor, Color spriteColor)
    {
        playerSpriteColor = spriteColor;
        playerColor = newColor;
        playerColorMaterial.SetColor("Color_C13AA74B", playerColor);
        playerSpriteRenderer_Color.color = spriteColor;
        playerSpriteRenderer_White.color = spriteColor;
    }
    #region Score
    public void AddScore(int newScore)
    {
        Debug.Log("Score added!");
        score += newScore + newScore * scoreMultiplier;
    }
    #endregion

    #region Input
    private void GetInput()
    {
          Vector3 screenCursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane); 
        //  cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);
      //  Vector3 screenCursorPos = Input.mousePosition;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(screenCursorPos);
        Vector2 direction = cursorPos - (Vector2)transform.position;
        direction.Normalize();

        //Vector3 cursorPos = Input.mousePosition;
        //cursorPos.z = 10f;
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        /*if(Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(5000f);
        }*/

        if (Input.GetMouseButton(0))
        {
            // Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //  Vector2 direction = new Vector2(cursorPos.x, cursorPos.y) - new Vector2(transform.position.x, transform.position.y);
            // direction.Normalize();
          //  shootDir.Normalize();

            if(shootCooldown <= 0f)
            {
                Shoot(direction);
                shootCooldown = shootCooldownMax;
            }
        }

        if(Input.GetKeyDown(pauseKey1) || Input.GetKeyDown(pauseKey2) || Input.GetKeyDown(pauseKey3))
        {
            if (menuOpen)
                CloseMenu();
            else
                OpenMenu();

        }
    }
    #endregion
    #region Menus and Stats

    private void HeroismCounter(int speedMultiplier)
    {
        deathMenuTimer = deathMenuTimerMax;

        if (deathMenuCounter < totalHeroism)
        {
            if ((deathMenuCounter + 1 * speedMultiplier) <= totalHeroism)
                deathMenuCounter += 1 * speedMultiplier;
            else
                deathMenuCounter = totalHeroism;
            // TODO: Play some increasing sound effect here
            deathHeroismGained.text = deathMenuCounter.ToString();
        }
        else if (deathMenuCounter >= totalHeroism)
        {
            deathMenuTimerOn = false;
            deathMenuCounter = totalHeroism;
            deathHeroismGained.text = totalHeroism.ToString();
            SaveHighScore();

            deathCounterSkipButton.SetActive(false);
            deathScreenButtons.SetActive(true);
        }
    }

    public void SkipCounting()
    {
        if(deathMenuTimerOn)
        {
            deathMenuTimerOn = false;
            deathMenuCounter = totalHeroism;
            deathHeroismGained.text = totalHeroism.ToString();
            SaveHighScore();
            
            deathCounterSkipButton.SetActive(false);
            deathScreenButtons.SetActive(true);
        }
    }

    private void SaveHighScore()
    {
        // TODO: Post score to HIGHSCORE here, or save to player game!
    }

    private void CloseMenu()
    {
        //   SoundManager.instance.PlaySound("pauseGame", Vector3.zero, transform.gameObject);
        pauseMenu.gameObject.SetActive(false);
        pauseMenuConfirmBox.SetActive(false);
        menuOpen = false;
        UpdatePlayerStatsUI();

        if(!UpgradeSpawnManager.instance.upgradeScreenOpen)
            Time.timeScale = 1;
    }

    private void OpenMenu()
    {
        //   SoundManager.instance.PlaySound("pauseGame", Vector3.zero, transform.gameObject);
        pauseMenu.gameObject.SetActive(true);
        menuOpen = true;
        UpdatePlayerStatsUI();

        Time.timeScale = 0;
    }

    public void OpenConfirmBox()
    {
        pauseMenuConfirmBox.SetActive(true);
    }

    public void CloseConfirmBox()
    {
        pauseMenuConfirmBox.SetActive(false);
    }

    public int OutputPlayerScore()
    {
        // Score comes from timesurvived * luckiness + score, so longer survival is also as important, plus the luckiness stat

        return (int) ((timeSurvived * luckiness) + score);
    }

    private void OpenDeathScreen()
    {
        if (pauseMenu.activeSelf)
            CloseMenu();
        text_playerHealthUI.gameObject.SetActive(false);
        text_playerScoreUI.gameObject.SetActive(false);
        UpgradeSpawnManager.instance.CloseUpgradeScreen();
        deathScreen.SetActive(true);
        deathDescription.gameObject.SetActive(true);
        deathDescription.text = deathDescriptions[UnityEngine.Random.Range(0, deathDescriptions.Count)];
        //   SoundManager.instance.PlaySound("deathJingle", Vector3.zero, this.gameObject);
        StartCoroutine(ShowDeath1());

    }

    IEnumerator ShowDeath1()
    {
        yield return new WaitForSeconds(deathMenuWait);
        deathSurvivedText.gameObject.SetActive(true);
        numTime.gameObject.SetActive(true);
        numTime.text = TimeSpan.FromSeconds((double)timeSurvived).ToString(@"dd\:hh\:mm\:ss");
        //SoundManager.instance.PlaySound("screenTextAppears", Vector3.zero, this.gameObject);
        StartCoroutine(ShowDeath2());
    }
    IEnumerator ShowDeath2()
    {
        yield return new WaitForSeconds(deathMenuWait);
        deathValourText.gameObject.SetActive(true);
        numValour.gameObject.SetActive(true);
        numValour.text = score.ToString();
        //SoundManager.instance.PlaySound("screenTextAppears", Vector3.zero, this.gameObject);
        StartCoroutine(ShowDeath3());
    }

    IEnumerator ShowDeath3()
    {
        yield return new WaitForSeconds(deathMenuWait);
        deathHeroismText.gameObject.SetActive(true);
        StartCoroutine(ShowDeath4());
    }
    IEnumerator ShowDeath4()
    {
        yield return new WaitForSeconds(deathMenuWait / 2f);
        deathCounterSkipButton.SetActive(true);
        deathHeroismGained.gameObject.SetActive(true);
        deathHeroismGained.text = "0";
        deathMenuTimerOn = true;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }
    public void RestartGame()
    {
        // TODO: Add other way to do this? Make sure nothing breaks!

        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }
    private void UpdatePlayerUI()
    {
        text_playerHealthUI.text = "Energy: " + health.ToString();
        text_playerScoreUI.text = "Valour: " + score.ToString();
    }
    private string FormatSurvivedTime()
    {
        return "Time Survived: " + TimeSpan.FromSeconds((double)timeSurvived).ToString(@"dd\:hh\:mm\:ss");
    }
    public void UpdatePlayerStatsUI()
    {
        // Player values
        text_playerHealth.text = "Energy: " + health.ToString();
        text_playerMaxHealth.text = "Max Energy: " + healthMax.ToString();
        text_playerSpeed.text = "Move Speed: " + speedMultiplier.ToString();
        text_playerFrailness.text = "Defense: " + frailnessMultiplier.ToString();
        text_playerInvincibilityFrames.text = "Invincibility Length: " + invincibilityLengthMax.ToString();
        bar_playerInvincibilityFrames.fillAmount = Math.Abs((invincibilityLengthMax - invincibilityLengthCurrent) / invincibilityLengthMax);

        text_playerTimeSurvived.text = FormatSurvivedTime();

        if (canLifeSteal)
            text_playerLifesteal.text = "Lifesteal: " + lifeStealAmount.ToString();
        else
            text_playerLifesteal.text = "";

        if (canHealthRegen)
            text_playerRegen.text = "Energy Regen: " + healthRegenAmount.ToString();
        else
            text_playerRegen.text = "";

        if (luckiness > 1)
            text_playerLuckiness.text = "Luckiness: " + luckiness.ToString();
        else
            text_playerLuckiness.text = "";

        if (scoreMultiplier > 0)
            text_playerValourMultiplier.text = "Extra Valour Multiplier: " + scoreMultiplier.ToString();
        else
            text_playerValourMultiplier.text = "";

        // Bullet values
        text_bulletDamage.text = "Damage: " + bulletDamage.ToString();
        text_bulletAmount.text = "Bullets: " + bulletAmount.ToString();
        text_bulletSpeed.text = "Bullet Speed: " + bulletSpeed.ToString();
        text_bulletCooldown.text = "Firing Rate: " + shootCooldownMax.ToString();
        bar_bulletCooldown.fillAmount = Math.Abs((shootCooldownMax - shootCooldown)/ shootCooldownMax);

        text_bulletSpread.text = "Bullet Spread: " + bulletSpread.ToString();
        text_bulletSizeMultiplier.text = "Bullet Size Multiplier: " + bulletSizeMultiplier.ToString();
        text_bulletLifetime.text = "Bullet Lifetime: " + bulletLifetime.ToString();

       // if (bulletBounces > 0)
       //     text_bulletBounces.text = "Bullet Bounces: " + bulletBounces.ToString();
      //  else
      //      text_bulletBounces.text = "";

    }
    #endregion

    #region Health

    public void healPlayer(float amount)
    {
        if(amount >= 0)
        {
            if (health < healthMax)
            {
                if ((health + amount) < healthMax)
                {
                    Debug.Log("Healing");
                    healthRegenDelay = healthRegenDelayMax;
                    health += amount;
                }
                else if ((health + amount) >= healthMax)
                {
                    Debug.Log("heal max");
                    healthRegenDelay = healthRegenDelayMax;
                    health = healthMax;
                }

            }
        }
        else // Apparently this deals damage, so takeDamage
        {
            Debug.Log("Take damage");
            TakeDamage(amount);
        }
    }

    public void TakeDamage(float damage)
    {
        if(!isDead)
        {
            // Defense works as a "layer", so damage does not go through if it is there.
            // Current solution, negative values do nothing, but buffer "not having" defense!
            //Debug.Log("As such, damage taken will be: " + (health - (damage + damage * frailness)));
          //  if ((health - (damage + damage * frailness)) <= 0)


            // Cases where Defense is over 0, and where it isn't. If it's less than 0, do not take into account.
            if(frailnessMultiplier > 0)
            {
                if (frailnessMultiplier < damage)
                {
                    if (health - Math.Abs((frailnessMultiplier - damage)) <= 0)
                    {
                        isDead = true;
                        StartCoroutine(PlayerDeath());
                    }
                    else
                    {
                        if (frailnessMultiplier <= damage) 
                            health -= Math.Abs((frailnessMultiplier - damage));
                        ChangePlayerColor(playerColorHurt, playerSpriteColorInvis);
                        // colorLerpTimer = colorLerpTimerMax;
                        invincibilityLengthCurrent = invincibilityLengthMax;
                    }
                }
                else // Frailness is better than damage, do nothing
                {
                    Debug.Log("Defense!");
                    // Play "Dink" sound effect?
                }
                    
            }
            else
            {
                if (health - damage <= 0)
                {
                    isDead = true;
                    StartCoroutine(PlayerDeath());
                }
                else
                {
                    health -= damage;
                    ChangePlayerColor(playerColorHurt, playerSpriteColorInvis);
                    // colorLerpTimer = colorLerpTimerMax;
                    invincibilityLengthCurrent = invincibilityLengthMax;
                }
            }
        }
    }

    private void LerpPlayerColor()
    {
        //  colorLerpTimer -= Time.deltaTime;
        invincibilityLengthCurrent -= Time.deltaTime;
        // Lerps player color back to default
      //  playerColor = Color.Lerp(playerColor, playerColorDefault, Mathf.PingPong(Time.deltaTime, invincibilityLengthMax));
      //  playerSpriteColor = Color.Lerp(playerSpriteColor, playerSpriteColorDefault, Mathf.PingPong(Time.deltaTime, invincibilityLengthMax));
        playerColor = Color.Lerp(playerColor, playerColorDefault, Time.deltaTime / invincibilityLengthMax );
        playerSpriteColor = Color.Lerp(playerSpriteColor, playerSpriteColorDefault, Time.deltaTime / invincibilityLengthMax);
        ChangePlayerColor(playerColor, playerSpriteColor);
    }

    IEnumerator PlayerDeath()
    {
        totalHeroism = OutputPlayerScore();
        GameObject deathPs = Instantiate(deathParticles, transform);
        // deathPs.GetComponent<ParticleEmission>().particleAmount = Mathf.RoundToInt((float)Math.Round((double)(totalHeroism / 10f)));
        if (totalHeroism <= 10)
        {
            deathPs.GetComponent<ParticleEmission>().particleAmount = 10;
        }
        else if (totalHeroism > 10 && totalHeroism <= 1000)
        {
            deathPs.GetComponent<ParticleEmission>().particleAmount = 10 + (Math.Abs((int)(totalHeroism / 10f)));
        }
        else if (totalHeroism > 1000 && totalHeroism <= 10000)
        {
            deathPs.GetComponent<ParticleEmission>().particleAmount = 100 + (Math.Abs((int)(totalHeroism / 100f)));
        }
        else if (totalHeroism > 10000 && totalHeroism <= 100000)
        {
            deathPs.GetComponent<ParticleEmission>().particleAmount = 200 + (Math.Abs((int)(totalHeroism / 1000f)));
        }
        else
        {
            deathPs.GetComponent<ParticleEmission>().particleAmount = 300;
        }
        deathPs.GetComponent<ParticleEmission>().PlayParticleBurst();
        playerSprite.SetActive(false);
        EnemySpawnManager.instance.gameEnded = true;
        UpgradeSpawnManager.instance.gameEnded = true;
        // SoundManager.instance.PlaySound("playerDeath", Vector3.zero, this.gameObject);
        // TODO: Stop music from playing here, then show END SCREEN after a while
        yield return new WaitForSeconds(4f);
        OpenDeathScreen();
    }
    #endregion

    #region Movement
    private void MovePlayer()
    {
        if(rgb.velocity.magnitude < playerMaxVelocity)
            rgb.velocity += movement * speedMultiplier;

        // float z = Mathf.SmoothDamp(zRotation, movement.x, ref rgb.velocity.x, 0.2f);

        //  playerSprite.transform.Rotate(new Vector3(0f,0f, movement.x) * 10f, Space.World);

         /*Vector3 euler = playerSprite.transform.localEulerAngles;
         Debug.Log("Euler is: " + euler);
         if (movement.x > 0)
             euler.z = Mathf.Lerp(euler.z, movement.x * 15f, 2.0f * Time.deltaTime);
         else if (movement.x < 0)
             euler.z = Mathf.Lerp(euler.z, movement.x * -15f, 2.0f * Time.deltaTime);
         else
             euler.z = Mathf.Lerp(euler.z, 0f, 2.0f * Time.deltaTime);*/

        Vector3 euler = playerSprite.transform.localEulerAngles;
        if (movement.x > 0)
            zRotation = Mathf.Lerp(zRotation, -5f, 5f * Time.deltaTime);
        else if (movement.x < 0)
            zRotation = Mathf.Lerp(zRotation, 5f, 5f * Time.deltaTime);
        else
            zRotation = 0f;

        euler.z = zRotation;
        playerSprite.transform.rotation = Quaternion.Euler(euler);
    }

    private void CheckPlayerInGameSpace()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float ratioY = screenPos.y / Camera.main.pixelHeight;
        float ratioX = screenPos.x / Camera.main.pixelWidth;

        if (ratioY < -0.01f) // Below screen
        {
            Vector2 middle = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
            Vector2 direction = middle - (Vector2)transform.position;
            direction.Normalize();
            rgb.velocity += direction * 10f;
            isInGameSpace = false;
        }

        else if (ratioY > 1.01f) // Above screen   
        {
            Vector2 middle = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
            Vector2 direction = middle - (Vector2)transform.position;
            direction.Normalize();
            rgb.velocity += direction * 10f;
            isInGameSpace = false;
        }

        else if (ratioX < -0.01f) // Left of screen
        {
            Vector2 middle = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
            Vector2 direction = middle - (Vector2)transform.position;
            direction.Normalize();
            rgb.velocity += direction * 10f;
            isInGameSpace = false;
        }

        else if (ratioX > 1.01f) // Right of screen
        {
            Vector2 middle = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
            Vector2 direction = middle - (Vector2)transform.position;
            direction.Normalize();
            rgb.velocity += direction * 10f;
            isInGameSpace = false;
        }
        else
            isInGameSpace = true;
    }
    #endregion

    #region Shooting

    private void Shoot(Vector2 direction)
    {
        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletObject, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().ShootBullet(bulletDamage, bulletLifetime, bulletSpeed, direction, bulletColor, bulletSizeMultiplier, lifeStealAmount);
        }
    }

    #endregion

}
