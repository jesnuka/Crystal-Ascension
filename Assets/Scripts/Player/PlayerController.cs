using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rgb;
    [SerializeField] Material playerColorMaterial;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] bool menuOpen;

    [Header("Buttons")]
    [SerializeField] KeyCode escapeKey;

    [Header("Movement")]
    [SerializeField] Vector2 movement;

    [Header("Player Values")]
    [SerializeField] float timeSurvived;
    [SerializeField] Color playerColorDefault;
    [SerializeField] Color playerColor;
    [SerializeField] float score;
    [SerializeField] float healthMax;
    [SerializeField] float health;
    [SerializeField] float healthRegenAmount;
    [SerializeField] float lifeStealAmount;

    [SerializeField] bool canHealthRegen;
    [SerializeField] bool canLifeSteal;

    [SerializeField] float speedMultiplier;
    [SerializeField] float playerMaxVelocity;

    [Header("Bullets")]
    [SerializeField] GameObject bulletObject;
    [SerializeField] Color bulletColor;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletAmount;
    [SerializeField] float bulletSpread;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifetime;
    [SerializeField] float bulletBounces;

    [SerializeField] float shootCooldown;
    [SerializeField] float shootCooldownMax;

    [Header("UI References")]
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

    // Bullet values
    [SerializeField] TMP_Text text_bulletDamage;
    [SerializeField] TMP_Text text_bulletAmount;
    [SerializeField] TMP_Text text_bulletSpeed;
    [SerializeField] TMP_Text text_bulletCooldown;

    [SerializeField] TMP_Text text_bulletSpread;
    [SerializeField] TMP_Text text_bulletLifetime;
    [SerializeField] TMP_Text text_bulletBounces;


    private void Awake()
    {
        ChangePlayerColor(playerColorDefault);
    }

    private void Start()
    {
        UpdatePlayerStatsUI();
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0)
        {
            timeSurvived += Time.deltaTime;

            UpdatePlayerUI();
        }

        GetInput();
        MovePlayer();
    }

    public void ChangePlayerColor(Color newColor)
    {
        playerColor = newColor;
        playerColorMaterial.SetColor("Color_C13AA74B", playerColor);
    }

    #region Movement
    private void GetInput()
    {
          Vector3 screenCursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane); 
        //  cursorPos = Camera.main.ScreenToWorldPoint(cursorPos);
      //  Vector3 screenCursorPos = Input.mousePosition;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(screenCursorPos);
        Vector2 direction = cursorPos - (Vector2)transform.position;
        direction.Normalize();
        Debug.Log("Shoot dir: " + direction);

        //Vector3 cursorPos = Input.mousePosition;
        //cursorPos.z = 10f;
        shootCooldown -= Time.deltaTime;
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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

        if(Input.GetKeyDown(escapeKey))
        {
            if (menuOpen)
                CloseMenu();
            else
                OpenMenu();

        }
    }

    private void CloseMenu()
    {
        //   SoundManager.instance.PlaySound("pauseGame", Vector3.zero, transform.gameObject);
        pauseMenu.gameObject.SetActive(false);
        menuOpen = false;
        UpdatePlayerStatsUI();

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

    private void UpdatePlayerUI()
    {
        text_playerHealthUI.text = "Health: " + health.ToString();
        text_playerScoreUI.text = "Score: " + score.ToString();
    }

    public void UpdatePlayerStatsUI()
    {
        // Player values
        text_playerHealth.text = "Health: " + health.ToString();
        text_playerMaxHealth.text = "Max Health: " + healthMax.ToString();
        text_playerSpeed.text = "Move Speed: " + speedMultiplier.ToString();

        if (canLifeSteal)
            text_playerLifesteal.text = "Lifesteal: " + lifeStealAmount.ToString();
        else
            text_playerLifesteal.text = "";

        if(canHealthRegen)
            text_playerRegen.text = "Health Regen: " + healthRegenAmount.ToString();
        else
            text_playerRegen.text = "";

        // Bullet values
        text_bulletDamage.text = "Damage: " + bulletDamage.ToString();
        text_bulletAmount.text = "Bullets: " + bulletAmount.ToString();
        text_bulletSpeed.text = "Bullet Speed: " + bulletSpeed.ToString();
        text_bulletCooldown.text = "Firing Rate: " + shootCooldownMax.ToString();

        text_bulletSpread.text = "Bullet Spread: " + bulletSpread.ToString();
        text_bulletLifetime.text = "Bullet Lifetime: " + bulletLifetime.ToString();

        if(bulletBounces > 0)
            text_bulletBounces.text = "Bullet Bounces: " + bulletBounces.ToString();
        else
            text_bulletBounces.text = "";

    }
    private void MovePlayer()
    {
        if(rgb.velocity.magnitude < playerMaxVelocity)
            rgb.velocity += movement * speedMultiplier;
    }
    #endregion

    #region Shooting

    private void Shoot(Vector2 direction)
    {
        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletObject, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().ShootBullet(bulletDamage, bulletLifetime, bulletSpeed, direction, bulletColor);
        }
    }

    #endregion

}
