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
    [SerializeField] GameObject panel_Controls;

    [Header("Settings")]
    [SerializeField] float musicVolume;
    [SerializeField] float soundVolume;

    [SerializeField] float musicVolumeEarlier;
    [SerializeField] float soundVolumeEarlier;

   // [SerializeField] AudioSource menuMusicSource;
  //  [SerializeField] AudioSource gameMusicSource;

    [SerializeField] Slider slider_MusicVolume;
    [SerializeField] Slider slider_SoundVolume;

    [SerializeField] TMP_Text text_MusicVolume;
    [SerializeField] TMP_Text text_SoundVolume;

    [SerializeField] GameObject button_SaveChangesActive;
    [SerializeField] GameObject button_SaveChangesDisabled;

    bool settingsLoaded;
    bool settingsSaved;

    bool musicTransitioned;


    void Update()
    {
        if (panel_Settings.activeSelf && musicTransitioned)
            UpdateSettings();

        if(!musicTransitioned)
        {
            if (SettingsManager.instance.menuMusicSource.volume == (musicVolume / 100f))
            {
                SettingsManager.instance.gameMusicSource.Stop();
                SettingsManager.instance.gameMusicSource.volume = (musicVolume / 100f);
                musicTransitioned = true;
            }
        }
    }

    private void Start()
    {
        if (SettingsManager.instance.returned)
        {
            StartCoroutine(StartFade(SettingsManager.instance.gameMusicSource, 2f, 0f));
        }

        SettingsManager.instance.menuMusicSource.volume = 0f;
        // SettingsManager.instance.menuMusicSource.volume = Mathf.Lerp(0f, (musicVolume / 100f), Time.deltaTime);
        StartCoroutine(StartFade(SettingsManager.instance.menuMusicSource, 2f, (musicVolume / 100f)));


        SettingsManager.instance.menuMusicSource.Play();
        LoadSettings();
        // SettingsManager.instance.gameMusicSource.Stop();
        SettingsManager.instance.inMenu = true;
        SettingsManager.instance.inGame = false;
    }
    #region Music
    public static IEnumerator StartFade(AudioSource source, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    #endregion
    #region Navigation

    public void ReturnToMenu()
    {
        if(settingsSaved == false)
        {
            musicVolume = musicVolumeEarlier;
            soundVolume = soundVolumeEarlier;
            slider_MusicVolume.value = musicVolume;
            slider_SoundVolume.value = soundVolume;
            SettingsManager.instance.menuMusicSource.volume = (musicVolume / 100f);
            SettingsManager.instance.gameMusicSource.volume = (musicVolume / 100f);

            text_MusicVolume.text = (slider_MusicVolume.value).ToString() + "%";
            text_SoundVolume.text = (slider_SoundVolume.value).ToString() + "%";
        }
        panel_Credits.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Unlockables.SetActive(false);
        panel_Controls.SetActive(false);

        panel_MainMenu.SetActive(true);
    }

    public void GoToSettings()
    {
        musicVolumeEarlier = slider_MusicVolume.value;
        soundVolumeEarlier = slider_SoundVolume.value;

        panel_Credits.SetActive(false);
        panel_Unlockables.SetActive(false);
        panel_MainMenu.SetActive(false);
        panel_Controls.SetActive(false);

        panel_Settings.SetActive(true);
    }

    public void GoToCredits()
    {
        panel_Unlockables.SetActive(false);
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Controls.SetActive(false);

        panel_Credits.SetActive(true);
    }

    public void GoToUnlockables()
    {
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Credits.SetActive(false);
        panel_Controls.SetActive(false);

        panel_Unlockables.SetActive(true);
    }
    public void GoToControls()
    {
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Credits.SetActive(false);
        panel_Unlockables.SetActive(false);

        panel_Controls.SetActive(true);
    }

    public void StartGame()
    {
        SettingsManager.instance.menuMusicSource.Stop();
        SettingsManager.instance.inMenu = false;
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
        SettingsManager.instance.menuMusicSource.volume = (slider_MusicVolume.value / 100f);
        SettingsManager.instance.gameMusicSource.volume = (slider_MusicVolume.value / 100f);
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

        musicVolumeEarlier = musicVolume;
        soundVolumeEarlier = soundVolume;

        SettingsManager.instance.musicVolume = musicVolume;
        SettingsManager.instance.soundVolume = soundVolume;

        SettingsManager.instance.menuMusicSource.volume = (musicVolume / 100f);
        SettingsManager.instance.gameMusicSource.volume = (musicVolume / 100f);

        settingsSaved = true;

        button_SaveChangesActive.gameObject.SetActive(false);
        button_SaveChangesDisabled.gameObject.SetActive(true);
    }
    #endregion
}
