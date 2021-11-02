using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject panel_MainMenu;
    [SerializeField] GameObject panel_Settings;
    [SerializeField] GameObject panel_Credits;
    [SerializeField] GameObject panel_Unlockables;

    [Header("Settings")]
    [SerializeField] float musicVolume;
    [SerializeField] float soundVolume;

    [SerializeField] Slider slider_MusicVolume;
    [SerializeField] Slider slider_SoundVolume;

    [SerializeField] TMP_Text text_MusicVolume;
    [SerializeField] TMP_Text text_SoundVolume;

    [SerializeField] GameObject button_SaveChangesActive;
    [SerializeField] GameObject button_SaveChangesDisabled;

    bool settingsLoaded;
    bool settingsSaved;


    void Update()
    {
        if (panel_Settings.activeSelf)
            UpdateSettings();
    }

    private void Start()
    {
        LoadSettings();
    }

    #region Navigation

    public void ReturnToMenu()
    {
        panel_Credits.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Unlockables.SetActive(false);

        panel_MainMenu.SetActive(true);
    }

    public void GoToSettings()
    {
        panel_Credits.SetActive(false);
        panel_Unlockables.SetActive(false);
        panel_MainMenu.SetActive(false);

        panel_Settings.SetActive(true);
    }

    public void GoToCredits()
    {
        panel_Unlockables.SetActive(false);
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);

        panel_Credits.SetActive(true);
    }

    public void GoToUnlockables()
    {
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Credits.SetActive(false);

        panel_Unlockables.SetActive(true);
    }

    public void StartGame()
    {
        Debug.Log("Start!");
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

    #region Unlockables
    #endregion

    #region Settings
    private void UpdateSettings()
    {
        if(settingsLoaded)
        {
            text_MusicVolume.text = musicVolume.ToString() + "%";
            text_SoundVolume.text = soundVolume.ToString() + "%";
            UpdateMusicVolume();
            UpdateSoundVolume();
        }
    }

    public void UpdateMusicVolume()
    {
        // musicVolume = slider_MusicVolume.value;
        text_MusicVolume.text = (slider_MusicVolume.value).ToString() + "%";
        if (musicVolume != slider_MusicVolume.value)
        {
            settingsSaved = false;
            button_SaveChangesActive.gameObject.SetActive(true);
            button_SaveChangesDisabled.gameObject.SetActive(false);
        }
    }

    public void UpdateSoundVolume()
    {
        // soundVolume = slider_SoundVolume.value;
        text_SoundVolume.text = (slider_SoundVolume.value).ToString() + "%";
        if (soundVolume != slider_SoundVolume.value)
        {
            settingsSaved = false;
            button_SaveChangesActive.gameObject.SetActive(true);
            button_SaveChangesDisabled.gameObject.SetActive(false);
        }
    }

    private void LoadSettings()
    {
        // TODO: Add loading of settings for user when game starts
        slider_MusicVolume.value = musicVolume;
        slider_SoundVolume.value = soundVolume;

        settingsLoaded = true;
    }

    public void SaveSettings()
    {
        // TODO: Add saving of settings for user when leaving settings
       // soundVolume = slider_SoundVolume.value * 100;
      //  musicVolume = slider_MusicVolume.value * 100;

        soundVolume = (float)System.Math.Round(slider_SoundVolume.value);
        musicVolume = (float)System.Math.Round(slider_MusicVolume.value);

        settingsSaved = true;

        button_SaveChangesActive.gameObject.SetActive(false);
        button_SaveChangesDisabled.gameObject.SetActive(true);
    }
    #endregion
}
