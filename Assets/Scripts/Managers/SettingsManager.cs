using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
  // [Header("References")]
  // [SerializeField] 

    [Header("Settings")]
    [SerializeField] float musicVolume;
    [SerializeField] float soundVolume;

    [Header("State")]
    [SerializeField] bool inMenu;
    [SerializeField] bool inGame;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
            Debug.Log("In game scene");
            inGame = true;
            TransferSettings();
        }
        
    }

    private void TransferSettings()
    {
        // TODO: Transfer over settings to Game Scene. 
        // OR, just dontdestroyonload the music manager as well ? 
    }
}
