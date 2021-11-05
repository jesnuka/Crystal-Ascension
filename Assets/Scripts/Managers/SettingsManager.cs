using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    // [Header("References")]
    // [SerializeField] 

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
            Debug.Log("Play");
            Debug.Log("In game scene");
            inGame = true;
            returned = true;
            TransferSettings();
            gameMusicSource.Play();
        }
    }

    private void TransferSettings()
    {
        // TODO: Transfer over settings to Game Scene. 
        // OR, just dontdestroyonload the music manager as well ? 
    }
}
