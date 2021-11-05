using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Upgrade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerController player;
    [Tooltip("The floating SPACE button text when player is close enough to pickup upgrade")]
    [SerializeField] GameObject spaceButtonGUI;
    [SerializeField] TMP_Text spaceButtonGUIText;


    [Header("Upgrades")]
    [Tooltip("Target values listed as readable strings here. This is fetched with the indexes and shown on screen, " +
        "then if picked up, a switch statement is used to change the values accordingly")]
    [SerializeField] public List<string> targetValues1; // Tier 1
    [SerializeField] public List<string> targetValues2; // Tier 2
    [SerializeField] public List<string> targetValues3; // Tier 3

    [SerializeField] public int positiveIndex;
    [SerializeField] public int negativeIndex;

    [SerializeField] public float positiveValue;
    [SerializeField] public float negativeValue;

    [SerializeField] public string positiveString;
    [SerializeField] public string negativeString;

    [Tooltip("The tier of this upgrade, either 1, 2 or 3")]
    [SerializeField] public int upgradeTier;
    [SerializeField] bool canBePickedUp;
    [SerializeField] float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        // Ignore collision between enemies and bullets
        Physics2D.IgnoreLayerCollision(13, 10, true);
        Physics2D.IgnoreLayerCollision(13, 11, true);
        Physics2D.IgnoreLayerCollision(13, 12, true);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            DestroyUpgradeObject();

        if(canBePickedUp)
        {
            if(Input.GetKeyDown(player.pickupKey))
            {
                if(UpgradeSpawnManager.instance.currentUpgrade == null)
                    UpgradeSpawnManager.instance.OpenUpgradeScreen(this);
            }
        }
    }

    public void InitializeUpgrade(float posValue, float negValue)
    {
        switch (upgradeTier)
        {
            case 1:
                {
                    // Randomize indexes
                    positiveIndex = Random.Range(0, targetValues1.Count);
                    negativeIndex = Random.Range(0, targetValues1.Count);
                    if (positiveIndex == negativeIndex)
                        negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues1.Count);

                    //  positiveUIText.text = targetValues1[positiveIndex];
                    //  negativeUIText.text = targetValues1[negativeIndex];

                    positiveString = targetValues1[positiveIndex];
                    negativeString = targetValues1[negativeIndex];

                    positiveValue = posValue;
                    negativeValue = negValue;

                    spaceButtonGUIText.text = "Tier " + upgradeTier.ToString();

                    break;
                }
            case 2:
                {
                    // Randomize indexes
                    positiveIndex = Random.Range(0, targetValues2.Count);
                    negativeIndex = Random.Range(0, targetValues2.Count);
                    if (positiveIndex == negativeIndex)
                        negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues2.Count);

                    //  positiveUIText.text = targetValues2[positiveIndex];
                    //  negativeUIText.text = targetValues2[negativeIndex];

                    positiveString = targetValues2[positiveIndex];
                    negativeString = targetValues2[negativeIndex];

                    positiveValue = posValue;
                    negativeValue = negValue;

                    spaceButtonGUIText.text = "Tier " + upgradeTier.ToString();

                    break;
                }
            case 3:
                {
                    // Randomize indexes
                    positiveIndex = Random.Range(0, targetValues3.Count);
                    negativeIndex = Random.Range(0, targetValues3.Count);
                    if (positiveIndex == negativeIndex)
                        negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues3.Count);

                    
                    //  positiveUIText.text = targetValues3[positiveIndex];
                    //  negativeUIText.text = targetValues3[negativeIndex];

                    positiveString = targetValues3[positiveIndex];
                    negativeString = targetValues3[negativeIndex];

                    positiveValue = posValue;
                    negativeValue = negValue;

                    spaceButtonGUIText.text = "Tier " + upgradeTier.ToString();

                    break;
                }
        }

        //positiveValue = CheckForRealValues(positiveIndex);
        //negativeValue = CheckForRealValues(negativeIndex);


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            spaceButtonGUI.SetActive(false);
            canBePickedUp = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
          //  Debug.Log("Player nearby!");
            spaceButtonGUI.SetActive(true);
            canBePickedUp = true;
        }
    }

    public void DestroyUpgradeObject()
    {
       // Instantiate(bulletDeathParticle, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void PickupUpgradeObject()
    {
        PlayerController.instance.upgradesTaken += 1;
        Debug.Log("Picking object");
        Debug.Log("PosIndex: " + positiveIndex + " and positive value: " + positiveValue);
        Debug.Log("negativeIndex: " + negativeIndex + " and negative value: " + negativeValue);
        switch (upgradeTier)
        {
            case 1:
                {
                    UpgradeRealValues1(positiveIndex, positiveValue);
                    UpgradeRealValues1(negativeIndex, negativeValue);
                    //  positiveUIText.text = targetValues1[positiveIndex];
                    //  negativeUIText.text = targetValues1[negativeIndex];

                    break;
                }
            case 2:
                {
                    UpgradeRealValues2(positiveIndex, positiveValue);
                    UpgradeRealValues2(negativeIndex, negativeValue);
                    //  positiveUIText.text = targetValues2[positiveIndex];
                    //  negativeUIText.text = targetValues2[negativeIndex];
                    break;
                }
            case 3:
                {
                    UpgradeRealValues3(positiveIndex, positiveValue);
                    UpgradeRealValues3(negativeIndex, negativeValue);
                    //  positiveUIText.text = targetValues3[positiveIndex];
                    //  negativeUIText.text = targetValues3[negativeIndex];
                    break;
                }
        }

    }

    // private float CheckForRealValues1(int value)
    private void UpgradeRealValues1(int index, float value)
    {
        // Generate the randomized new value based on the old value

        switch (index)
        {
            case 0:
                {
                    Debug.Log("health");
                   // player.healthMax = player.healthMax + player.healthMax * value;
                    player.healthMax = (player.healthMax != 0) ? player.healthMax - (Mathf.Abs(player.healthMax * value) * Mathf.Sign(value)) :
                        player.healthMax + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    player.health += Mathf.Abs(player.healthMax * value) * Mathf.Sign(value);
                    if (player.health > player.healthMax)
                        player.health = player.healthMax;
                    break;
                }
            case 1:
                {
                    Debug.Log("shootcooldown");
                    // Negative value, so Positive stats make this faster, and negative stats increase this (Slow it down)
                    player.shootCooldownMax = (player.shootCooldownMax != 0) ? player.shootCooldownMax - (Mathf.Abs(player.shootCooldownMax * value) * Mathf.Sign(value)) :
                        player.shootCooldownMax + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    if (player.shootCooldown > player.shootCooldownMax)
                        player.shootCooldown = player.shootCooldownMax;
                    break;
                }
            case 2:
                {
                    Debug.Log("Speed");
                    player.speedMultiplier = (player.speedMultiplier != 0) ? player.speedMultiplier + (Mathf.Abs(player.speedMultiplier * value) * Mathf.Sign(value)) :
                        player.speedMultiplier + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    player.playerMaxVelocity = Mathf.Abs(player.speedMultiplier * 5);
                    
                    break;
                }
            case 3:
                {
                    Debug.Log("Bullet speed");
                    player.bulletSpeed = (player.bulletSpeed != 0) ? player.bulletSpeed + (Mathf.Abs(player.bulletSpeed * value) * Mathf.Sign(value)) :
                        player.bulletSpeed + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;

                }
            case 4:
                {
                    Debug.Log("Frailness first" + player.frailnessMultiplier);
                    Debug.Log("Math is: " + Mathf.Abs(1 * value) * Mathf.Sign(value));
                    player.frailnessMultiplier = (player.frailnessMultiplier != 0) ? player.frailnessMultiplier + (Mathf.Abs(player.frailnessMultiplier * value) * Mathf.Sign(value)) :
                        player.frailnessMultiplier + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    Debug.Log("Frailness then" + player.frailnessMultiplier);

                    break;
                }
        }


        DestroyUpgradeObject();

    }

    private void UpgradeRealValues2(int index, float value)
    {
        // Generate the randomized new value based on the old value

        switch (index)
        {
            case 0:
                {
                    // TODO: Make sure float bulletAmount is shot as correct amount
                    player.bulletAmount = (player.bulletAmount != 0) ? player.bulletAmount + (Mathf.Abs(player.bulletAmount * value) * Mathf.Sign(value)) :
                        player.bulletAmount + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 1:
                {
                    player.bulletDamage = (player.bulletDamage != 0) ? player.bulletDamage + (Mathf.Abs(player.bulletDamage * value) * Mathf.Sign(value)) :
                        player.bulletDamage + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 2:
                {
                    player.luckiness = (player.luckiness != 0) ? player.luckiness + (Mathf.Abs(player.luckiness * value) * Mathf.Sign(value)) :
                             player.luckiness + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
          /*  case 3:
                {
                    break;
                }*/
        }

        DestroyUpgradeObject();
    }

    private void UpgradeRealValues3(int index, float value)
    {
        // Generate the randomized new value based on the old value

        switch (index)
        {
            case 0:
                {
                    player.invincibilityLengthMax = (player.invincibilityLengthMax != 0) ? player.invincibilityLengthMax + (Mathf.Abs(player.invincibilityLengthMax * value) * Mathf.Sign(value)) :
                            player.invincibilityLengthMax + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    if (player.invincibilityLengthCurrent > player.invincibilityLengthMax)
                        player.invincibilityLengthCurrent = player.invincibilityLengthMax;
                    break;
                }
            case 1:
                {
                    //player.healthRegenAmount = player.healthRegenAmount + player.healthRegenAmount * value;
                    player.healthRegenAmount = (player.healthRegenAmount != 0) ? player.healthRegenAmount + (Mathf.Abs(player.healthRegenAmount * value) * Mathf.Sign(value)) :
                        player.healthRegenAmount + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 2:
                {
                    player.bulletSizeMultiplier = (player.bulletSizeMultiplier != 0) ? player.bulletSizeMultiplier + (Mathf.Abs(player.bulletSizeMultiplier * value) * Mathf.Sign(value)) :
                        player.bulletSizeMultiplier + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 3:
                {
                    player.lifeStealAmount = (player.lifeStealAmount != 0) ? player.lifeStealAmount + (Mathf.Abs(player.lifeStealAmount * value) * Mathf.Sign(value)) :
                          player.lifeStealAmount + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;

                }
           /* case 4:
                {
                    break;
                }
            case 5:
                {
                    break;
                }*/
        }

        DestroyUpgradeObject();
    }
}
