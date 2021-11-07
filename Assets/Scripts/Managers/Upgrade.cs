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

    // The tiers randomized for the values
    [SerializeField] public int positiveTier;
    [SerializeField] public int negativeTier;

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

                    // positiveIndex = Random.Range(0, targetValues2.Count);
                    // negativeIndex = Random.Range(0, targetValues2.Count);
                    // if (positiveIndex == negativeIndex)
                    //    negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues2.Count);

                    //  positiveUIText.text = targetValues2[positiveIndex];
                    //  negativeUIText.text = targetValues2[negativeIndex];

                    // positiveString = targetValues2[positiveIndex];
                    // negativeString = targetValues2[negativeIndex];


                    // Randomize indexes, choose from Tier 1 + Tier 2
                    //  ChooseTier2(positiveString, positiveIndex, positiveTier, true);
                   // ChooseTier2(negativeString, negativeIndex, negativeTier, false, positiveIndex);

                    // Bad way to do this, but works for now
                    ChooseTier2Positive();
                    ChooseTier2Negative();

                    positiveValue = posValue;
                    negativeValue = negValue;

                    spaceButtonGUIText.text = "Tier " + upgradeTier.ToString();

                    break;
                }
            case 3:
                {
                    // Randomize indexes
                    //positiveIndex = Random.Range(0, targetValues3.Count);
                    // negativeIndex = Random.Range(0, targetValues3.Count);
                    // if (positiveIndex == negativeIndex)
                    //      negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues3.Count);


                    //  positiveUIText.text = targetValues3[positiveIndex];
                    //  negativeUIText.text = targetValues3[negativeIndex];

                    //   positiveString = targetValues3[positiveIndex];
                    //  negativeString = targetValues3[negativeIndex];


                    // Randomize indexes, choose from Tier 1 + Tier 2
                    //ChooseTier3(positiveString, positiveIndex, positiveTier);
                    // ChooseTier3(negativeString, negativeIndex, negativeTier, positiveIndex);

                    // Bad way to do this, but works for now
                    ChooseTier3Positive();
                    ChooseTier3Negative();

                    positiveValue = posValue;
                    negativeValue = negValue;

                    spaceButtonGUIText.text = "Tier " + upgradeTier.ToString();

                    break;
                }
        }

        //positiveValue = CheckForRealValues(positiveIndex);
        //negativeValue = CheckForRealValues(negativeIndex);


    }
    private void ChooseTier2Positive() // very bad way to do this, but works for now
    {
        int tierRand = Random.Range(0, 2);
        if (tierRand == 0)
        {
            positiveTier = 1;
            positiveIndex = Random.Range(0, targetValues1.Count);
            positiveString = targetValues1[positiveIndex];
        }
        else
        {
            positiveTier = 2;
            positiveIndex = Random.Range(0, targetValues2.Count);
            positiveString = targetValues2[positiveIndex];
        }
    }

    private void ChooseTier2Negative() // very bad way to do this, but works for now
    {
        int tierRand = Random.Range(0, 2);
        if (tierRand == 0)
        {
            negativeTier = 1;
            negativeIndex = Random.Range(0, targetValues1.Count);

            if (positiveIndex == negativeIndex)
                negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues1.Count);

            negativeString = targetValues1[negativeIndex];
        }
        else
        {
            negativeTier = 2;
            negativeIndex = Random.Range(0, targetValues2.Count);

            if (positiveIndex == negativeIndex)
                negativeIndex = Mathf.Abs((negativeIndex - 2) % targetValues2.Count);

            negativeString = targetValues2[negativeIndex];
        }
    }

    private void ChooseTier3Positive() // very bad way to do this, but works for now
    {
        int tierRand = Random.Range(0, 3);
        if (tierRand == 0)
        {
            positiveTier = 1;
            positiveIndex = Random.Range(0, targetValues1.Count);
            positiveString = targetValues1[positiveIndex];
        }
        else if(tierRand == 1)
        {
            positiveTier = 2;
            positiveIndex = Random.Range(0, targetValues2.Count);
            positiveString = targetValues2[positiveIndex];
        }
        else
        {
            positiveTier = 3;
            positiveIndex = Random.Range(0, targetValues3.Count);
            positiveString = targetValues3[positiveIndex];
        }
    }

    private void ChooseTier3Negative() // very bad way to do this, but works for now
    {
        int tierRand = Random.Range(0, 2);
        if (tierRand == 0)
        {
            negativeTier = 1;
            negativeIndex = Random.Range(0, targetValues1.Count);

            if (positiveIndex == negativeIndex)
                negativeIndex = Mathf.Abs((negativeIndex - 1) % targetValues1.Count);

            negativeString = targetValues1[negativeIndex];
        }
        else if(tierRand == 1)
        {
            negativeTier = 2;
            negativeIndex = Random.Range(0, targetValues2.Count);

            if (positiveIndex == negativeIndex)
                negativeIndex = Mathf.Abs((negativeIndex - 2) % targetValues2.Count);

            negativeString = targetValues2[negativeIndex];
        }
        else
        {
            negativeTier = 3;
            negativeIndex = Random.Range(0, targetValues3.Count);

            if (positiveIndex == negativeIndex)
                negativeIndex = Mathf.Abs((negativeIndex - 3) % targetValues3.Count);

            negativeString = targetValues3[negativeIndex];
        }
    }

    /*private void ChooseTier2(string stringName, int index, int newTier, bool isPos, int comparisonIndex = 0)
    {
        int tierRand = Random.Range(0, 2);
        if(tierRand == 0)
        {
            newTier = 1;
            index = Random.Range(0, targetValues1.Count);

            if (comparisonIndex == index)
                index = Mathf.Abs((index - 1) % targetValues1.Count);


            stringName = targetValues1[index];
            Debug.Log("Tier 2 gets tier 1");
            Debug.Log("Tier 2 name: " + stringName);
            Debug.Log("Tier 2 index: " + index);
        }
        else
        {
            newTier = 2;
            index = Random.Range(0, targetValues2.Count);

            if (comparisonIndex == index)
                index = Mathf.Abs((index - 1) % targetValues2.Count);


            stringName = targetValues2[index];
            Debug.Log("Tier 2 gets tier 2");
            Debug.Log("Tier 2 name: " + stringName);
            Debug.Log("Tier 2 index: " + index);
        }
    }*/

    private void ChooseTier3(string stringName, int index, int newTier, int comparisonIndex = 0)
    {
        int tierRand = Random.Range(0, 3);
        if (tierRand == 0)
        {
            newTier = 1;
            index = Random.Range(0, targetValues1.Count);

            if (comparisonIndex == index)
                index = Mathf.Abs((index - 1) % targetValues1.Count);



            stringName = targetValues1[index];
        }
        else if(tierRand == 1)
        {
            newTier = 2;
            index = Random.Range(0, targetValues2.Count);

            if (comparisonIndex == index)
                index = Mathf.Abs((index - 1) % targetValues2.Count);



            stringName = targetValues2[index];
        }
        else
        {
            newTier = 3;
            index = Random.Range(0, targetValues3.Count);

            if (comparisonIndex == index)
                index = Mathf.Abs((index - 1) % targetValues3.Count);


            stringName = targetValues3[index];

        }
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
        //  Debug.Log("Picking object");
        //  Debug.Log("PosIndex: " + positiveIndex + " and positive value: " + positiveValue);
        // Debug.Log("negativeIndex: " + negativeIndex + " and negative value: " + negativeValue);

        CheckNewTier(positiveTier, positiveIndex, positiveValue);
        CheckNewTier(negativeTier, negativeIndex, negativeValue);

        /*switch (upgradeTier)
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
                  //  CheckNewTier();
                  //  int rand1 = Random.Range(0, 2);
                  //  int rand2 = Random.Range(0, 2);
                 //   ChooseUpgradeTier2(rand1, positiveIndex, positiveValue);
                   // ChooseUpgradeTier2(rand2, negativeIndex, negativeValue);

                    //  positiveUIText.text = targetValues2[positiveIndex];
                    //  negativeUIText.text = targetValues2[negativeIndex];
                    break;
                }
            case 3:
                {

                    //  positiveUIText.text = targetValues3[positiveIndex];
                    //  negativeUIText.text = targetValues3[negativeIndex];
                    break;
                }
        }*/

    }
    private void CheckNewTier(int upgradeNewTier, int upgradeIndex, float upgradeValue)
    {
        if (upgradeNewTier == 1)
        {
            UpgradeRealValues1(upgradeIndex, upgradeValue);
        }
        else if(upgradeNewTier == 2)
        {
            UpgradeRealValues2(upgradeIndex, upgradeValue);
        }
        else
        {
            UpgradeRealValues3(upgradeIndex, upgradeValue);
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
                    // TODO: Double check that this works!
                    if (player.healthMax == 0)
                        player.healthMax = 0.01f;

                    if ((player.healthMax + (Mathf.Abs(player.healthMax * value) * Mathf.Sign(value))) <= 0)
                    {
                        player.healthMax = 0.1f;
                        if (player.health > player.healthMax)
                            player.health = player.healthMax;
                        else if(Mathf.Sign(value) > 0) // Only increase player HP if the upgrade is increases max HP
                            player.health += Mathf.Abs(player.healthMax * value) * Mathf.Sign(value);
                    }
                    else
                    {
                        player.healthMax = player.healthMax + (Mathf.Abs(player.healthMax * value) * Mathf.Sign(value)); // Just incase to avoid insane fire rate, this is done.
                        if (player.health > player.healthMax)
                            player.health = player.healthMax;
                        else if (Mathf.Sign(value) > 0) // Only increase player HP if the upgrade is increases max HP
                            player.health += Mathf.Abs(player.healthMax * value) * Mathf.Sign(value);
                    }

                    // player.healthMax = player.healthMax + player.healthMax * value;
                    //  player.healthMax = (player.healthMax != 0) ? player.healthMax - (Mathf.Abs(player.healthMax * value) * Mathf.Sign(value)) :
                //   player.healthMax + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                  //  player.health += Mathf.Abs(player.healthMax * value) * Mathf.Sign(value);
                  //  if (player.health > player.healthMax)
                 //       player.health = player.healthMax;
                    break;
                }
            case 1:
                {
                    if (player.shootCooldownMax == 0)
                        player.shootCooldownMax = 0.05f;

                    if ((player.shootCooldownMax - (Mathf.Abs(player.shootCooldownMax * value) * Mathf.Sign(value))) <= 0.05f)
                        player.shootCooldownMax = 0.05f;
                    else
                        player.shootCooldownMax = player.shootCooldownMax - (Mathf.Abs(player.shootCooldownMax * value) * Mathf.Sign(value)); // Just incase to avoid insane fire rate, this is done.

                    // Negative value, so Positive stats make this faster, and negative stats increase this (Slow it down)
                    //  player.shootCooldownMax = (player.shootCooldownMax != 0) ? player.shootCooldownMax - (Mathf.Abs(player.shootCooldownMax * value) * Mathf.Sign(value)) :
                    //      player.shootCooldownMax + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    //  if (player.shootCooldown > player.shootCooldownMax)
                    //       player.shootCooldown = player.shootCooldownMax;
                    break;
                }
            case 2:
                {
                    if (player.speedMultiplier == 0)
                        player.speedMultiplier = 0.01f;

                    //  Debug.Log("Speed");
                    //  Debug.Log("Speed");
                    if ((player.speedMultiplier - (Mathf.Abs(player.speedMultiplier * value) * Mathf.Sign(value))) <= 0.01f)
                        player.speedMultiplier = 0.01f;
                    else
                        player.speedMultiplier = player.speedMultiplier + (Mathf.Abs(player.speedMultiplier * value) * Mathf.Sign(value));

                 //   player.speedMultiplier = (player.speedMultiplier != 0) ? player.speedMultiplier + (Mathf.Abs(player.speedMultiplier * value) * Mathf.Sign(value)) :
                 //       player.speedMultiplier + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    player.playerMaxVelocity = Mathf.Abs(player.speedMultiplier * 5);
                    
                    break;
                }
            case 3:
                {
                  //  Debug.Log("Bullet speed");
                    player.bulletSpeed = (player.bulletSpeed != 0) ? player.bulletSpeed + (Mathf.Abs(player.bulletSpeed * value) * Mathf.Sign(value)) :
                        player.bulletSpeed + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;

                }
            case 4:
                {
                    if (player.frailnessMultiplier == 0)
                        player.frailnessMultiplier = 0.01f;

                    // Debug.Log("Frailness first" + player.frailnessMultiplier);
                    //  Debug.Log("Math is: " + Mathf.Abs(1 * value) * Mathf.Sign(value));
                    if ((player.frailnessMultiplier - (Mathf.Abs(player.frailnessMultiplier * value) * Mathf.Sign(value))) <= 0.01f)
                        player.frailnessMultiplier = 0.01f;
                    else
                        player.frailnessMultiplier = player.bulletDamage + (Mathf.Abs(player.frailnessMultiplier * value) * Mathf.Sign(value));

                    //player.frailnessMultiplier = (player.frailnessMultiplier != 0) ? player.frailnessMultiplier + (Mathf.Abs(player.frailnessMultiplier * value) * Mathf.Sign(value)) :
                  //      player.frailnessMultiplier + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                   // Debug.Log("Frailness then" + player.frailnessMultiplier);

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
                    if (player.bulletAmount == 0)
                        player.bulletAmount = 1.0f;

                    // TODO: Make sure float bulletAmount is shot as correct amount
                    if ((player.bulletAmount - (Mathf.Abs(player.bulletAmount * value) * Mathf.Sign(value))) <= 1f)
                        player.bulletAmount = 1.0f;
                    else
                        player.bulletAmount = player.bulletAmount + (Mathf.Abs(player.bulletAmount * value) * Mathf.Sign(value));
                    break;
                }
            case 1:
                {
                    if (player.bulletDamage == 0)
                        player.bulletDamage = 0.1f;

                    if ((player.bulletDamage - (Mathf.Abs(player.bulletDamage * value) * Mathf.Sign(value))) <= 0.1)
                        player.bulletDamage = 0.1f;
                    else
                        player.bulletDamage = player.bulletDamage + (Mathf.Abs(player.bulletDamage * value) * Mathf.Sign(value));

                   // player.bulletDamage = (player.bulletDamage != 0) ? player.bulletDamage + (Mathf.Abs(player.bulletDamage * value) * Mathf.Sign(value)) :
                  //      player.bulletDamage + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 2:
                {
                    if (player.luckiness == 0)
                        player.luckiness = 0.01f;

                    if ((player.luckiness - (Mathf.Abs(player.luckiness * value) * Mathf.Sign(value))) <= 0.01f)
                        player.luckiness = 0.01f;
                    else
                        player.luckiness = player.luckiness - (Mathf.Abs(player.luckiness * value) * Mathf.Sign(value));

                  //  player.luckiness = (player.luckiness != 0) ? player.luckiness + (Mathf.Abs(player.luckiness * value) * Mathf.Sign(value)) :
                  //           player.luckiness + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 3:
                {
                    if (player.bulletLifetime == 0)
                        player.bulletLifetime = 0.1f;

                    if ((player.bulletLifetime - (Mathf.Abs(player.bulletLifetime * value) * Mathf.Sign(value))) <= 0.1f)
                        player.bulletLifetime = 0.1f;
                    else
                        player.bulletLifetime = player.bulletLifetime - (Mathf.Abs(player.bulletLifetime * value) * Mathf.Sign(value));

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
                    if (player.invincibilityLengthMax == 0)
                        player.invincibilityLengthMax = 0.01f;

                    if ((player.invincibilityLengthMax - (Mathf.Abs(player.invincibilityLengthMax * value) * Mathf.Sign(value))) <= 0.01f)
                        player.invincibilityLengthMax = 0.01f;
                    else
                        player.invincibilityLengthMax = player.invincibilityLengthMax - (Mathf.Abs(player.invincibilityLengthMax * value) * Mathf.Sign(value));

                    //player.invincibilityLengthMax = (player.invincibilityLengthMax != 0) ? player.invincibilityLengthMax + (Mathf.Abs(player.invincibilityLengthMax * value) * Mathf.Sign(value)) :
                   //         player.invincibilityLengthMax + (Mathf.Abs(1 * value) * Mathf.Sign(value));

                    if (player.invincibilityLengthCurrent > player.invincibilityLengthMax)
                        player.invincibilityLengthCurrent = player.invincibilityLengthMax;
                    break;
                }
            case 1:
                {
                    if (player.healthRegenAmount == 0)
                        player.healthRegenAmount = 0.01f;

                    if ((player.healthRegenAmount - (Mathf.Abs(player.healthRegenAmount * value) * Mathf.Sign(value))) <= 0f)
                        player.healthRegenAmount = 0f;
                    else
                        player.healthRegenAmount = player.healthRegenAmount - (Mathf.Abs(player.healthRegenAmount * value) * Mathf.Sign(value));


                    //player.healthRegenAmount = player.healthRegenAmount + player.healthRegenAmount * value;
                 //   player.healthRegenAmount = (player.healthRegenAmount != 0) ? player.healthRegenAmount + (Mathf.Abs(player.healthRegenAmount * value) * Mathf.Sign(value)) :
                  //      player.healthRegenAmount + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 2:
                {
                    if (player.bulletSizeMultiplier == 0)
                        player.bulletSizeMultiplier = 0.01f;

                    if ((player.bulletSizeMultiplier - (Mathf.Abs(player.bulletSizeMultiplier * value) * Mathf.Sign(value))) <= 0.01f)
                        player.bulletSizeMultiplier = 0.01f;
                    else
                        player.bulletSizeMultiplier = player.bulletSizeMultiplier - (Mathf.Abs(player.bulletSizeMultiplier * value) * Mathf.Sign(value));

                  //  player.bulletSizeMultiplier = (player.bulletSizeMultiplier != 0) ? player.bulletSizeMultiplier + (Mathf.Abs(player.bulletSizeMultiplier * value) * Mathf.Sign(value)) :
                  //      player.bulletSizeMultiplier + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;
                }
            case 3:
                {
                    if (player.lifeStealAmount == 0)
                        player.lifeStealAmount = 0.01f;

                    if ((player.lifeStealAmount - (Mathf.Abs(player.lifeStealAmount * value) * Mathf.Sign(value))) <= 0f)
                        player.lifeStealAmount = 0f;
                    else
                        player.lifeStealAmount = player.lifeStealAmount - (Mathf.Abs(player.lifeStealAmount * value) * Mathf.Sign(value));

                  //  player.lifeStealAmount = (player.lifeStealAmount != 0) ? player.lifeStealAmount + (Mathf.Abs(player.lifeStealAmount * value) * Mathf.Sign(value)) :
                  //        player.lifeStealAmount + (Mathf.Abs(1 * value) * Mathf.Sign(value));
                    break;

                }
            case 4:
                {
                    break;
                }
            case 5:
                {
                    break;
                }
        }

        DestroyUpgradeObject();
    }
}
