using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    // Also stores Highscore values
    public static SettingsManager instance;

    // [Header("References")]
    // [SerializeField] 

    [Header("Highscore")]
    [SerializeField] public float bestSurvival;
    [SerializeField] public int bestValour;
    [SerializeField] public int bestHeroism;

    [Header("Settings")]
    [SerializeField] public float musicVolume;
    [SerializeField] public float soundVolume;

    [SerializeField] public AudioSource menuMusicSource;
    [SerializeField] public AudioSource gameMusicSource;

    [Header("State")]
    [SerializeField] public bool inMenu;
    [SerializeField] public bool inGame;
    [SerializeField] public bool returned;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadPrefs();
    }


    public void LoadPrefs()
    {
        bestSurvival = PlayerPrefs.GetFloat("SurvivalHighscore", 0f);
        bestValour = PlayerPrefs.GetInt("ValourHighscore", 0);
        bestHeroism =  PlayerPrefs.GetInt("HeroismHighscore", 0);
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetFloat("SurvivalHighscore", bestSurvival);
        PlayerPrefs.SetInt("ValourHighscore", bestValour);
        PlayerPrefs.SetInt("HeroismHighscore", bestHeroism);
    }

    // Update is called once per frame
    void Update()
    {
        if(!inMenu && !inGame)
            CheckSceneChange();
    }
    private void CheckSceneChange()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            inGame = true;
            returned = true;
        //    TransferSettings();
            gameMusicSource.Play();
        }
    }

    #region Highscores

    public void UpdateMenuHighscores()
    {
        // Changes the TMP_Text to the corresponding highscores
        MenuManager.instance.SetHighScore(bestSurvival, bestValour, bestHeroism);
    }

    public bool CheckSurvivalHighscore(float survivalTime)
    {
        if(survivalTime > bestSurvival) // new highscore
        {
            bestSurvival = survivalTime;
            SavePrefs();
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool CheckValourHighscore(int valourGained)
    {
        if (valourGained > bestValour) // new highscore
        {
            bestValour = valourGained;
            SavePrefs();
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckHeroismHighscore(int heroismGained)
    {
        if (heroismGained > bestHeroism) // new highscore
        {
            bestHeroism = heroismGained;
            SavePrefs();
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    /*  private void TransferSettings()
      {
          // TODO: Transfer over settings to Game Scene. 
          // OR, just dontdestroyonload the music manager as well ? 
      }*/
}
