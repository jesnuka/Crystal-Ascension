using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [Header("UI References")]
    [SerializeField] GameObject panel_MainMenu;
    [SerializeField] GameObject panel_Settings;
    [SerializeField] GameObject panel_Credits;
    [SerializeField] GameObject panel_Unlockables;
    [SerializeField] GameObject panel_Controls;
    [SerializeField] GameObject panel_Highscores;

    [Header("High scores")]
    [SerializeField] TMP_Text highscore_Survival_text;
    [SerializeField] TMP_Text highscore_Valour_text;
    [SerializeField] TMP_Text highscore_Heroism_text;

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

    [SerializeField] GameObject graphicsText_High;
    [SerializeField] GameObject graphicsText_Low;

    [SerializeField] int graphicsSettings;

    bool settingsLoaded;
    bool settingsSaved;

    bool musicTransitioned;

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

    void Update()
    {
        if (panel_Settings.activeSelf && musicTransitioned)
            UpdateSettings();

        if(!musicTransitioned)
        {
           // Debug.Log("Menu volume is: " + SettingsManager.instance.menuMusicSource.volume);
           // Debug.Log("needs to be: " + (musicVolume / 100f));
            //if (SettingsManager.instance.menuMusicSource.volume == (musicVolume / 100f))
            if (Mathf.Abs(SettingsManager.instance.menuMusicSource.volume - (musicVolume / 100f)) <= 0.1)
            {
                SettingsManager.instance.gameMusicSource.Stop();
                SettingsManager.instance.gameMusicSource.volume = (musicVolume / 100f);
                musicTransitioned = true;
            }
        }
    }

    private void Start()
    {
        LoadSettings();
        if (SettingsManager.instance.returned)
        {
            StartCoroutine(StartFade(SettingsManager.instance.gameMusicSource, 2f, 0f));
        }

        SettingsManager.instance.menuMusicSource.volume = 0f;
        // SettingsManager.instance.menuMusicSource.volume = Mathf.Lerp(0f, (musicVolume / 100f), Time.deltaTime);
        StartCoroutine(StartFade(SettingsManager.instance.menuMusicSource, 2f, (musicVolume / 100f)));


        SettingsManager.instance.menuMusicSource.Play();
        // SettingsManager.instance.gameMusicSource.Stop();
        SettingsManager.instance.inMenu = true;
        SettingsManager.instance.inGame = false;

        SettingsManager.instance.UpdateMenuHighscores();
    }

    #region Highscore
    public void SetHighScore(float survival, int valour, int heroism)
    {
        highscore_Survival_text.text = System.TimeSpan.FromSeconds((double)survival).ToString(@"dd\:hh\:mm\:ss"); ;
        highscore_Valour_text.text = valour.ToString();
        highscore_Heroism_text.text = heroism.ToString();
    }
    #endregion

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
    #region Sound

    public void PlayButtonClick()
    {
        SoundManager.instance.PlaySoundOnce("buttonPress", Vector3.zero, this.gameObject, true);
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

            button_SaveChangesActive.SetActive(false);
            button_SaveChangesDisabled.SetActive(true);
        }
        panel_Credits.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Unlockables.SetActive(false);
        panel_Controls.SetActive(false);
        panel_Highscores.SetActive(false);

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
        panel_Highscores.SetActive(false);

        panel_Settings.SetActive(true);
    }

    public void GoToCredits()
    {
        panel_Unlockables.SetActive(false);
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Controls.SetActive(false);
        panel_Highscores.SetActive(false);

        panel_Credits.SetActive(true);
    }
    public void GoToHighscores()
    {
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Credits.SetActive(false);
        panel_Controls.SetActive(false);
        panel_Unlockables.SetActive(false);

        panel_Highscores.SetActive(true);
    }
    public void GoToUnlockables()
    {
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Credits.SetActive(false);
        panel_Controls.SetActive(false);
        panel_Highscores.SetActive(false);

        panel_Unlockables.SetActive(true);
    }
    public void GoToControls()
    {
        panel_MainMenu.SetActive(false);
        panel_Settings.SetActive(false);
        panel_Credits.SetActive(false);
        panel_Unlockables.SetActive(false);
        panel_Highscores.SetActive(false);

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

    public void ToggleGraphicsSettings()
    {
        if(graphicsSettings == 1) // High
        {
            graphicsText_High.SetActive(false);
            graphicsText_Low.SetActive(true);
            graphicsSettings = 0;
            PlayerPrefs.SetInt("GraphicsSettings", 0);
            QualitySettings.SetQualityLevel(0);
        }
        else // Low
        {
            graphicsText_Low.SetActive(false);
            graphicsText_High.SetActive(true);
            graphicsSettings = 1;
            PlayerPrefs.SetInt("GraphicsSettings", 1);
            QualitySettings.SetQualityLevel(2);
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

        graphicsSettings = PlayerPrefs.GetInt("GraphicsSettings", 1);

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 55f);
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 50f);
        musicVolumeEarlier = musicVolume;
        soundVolumeEarlier = soundVolume;

        SettingsManager.instance.musicVolume = musicVolume;
        SettingsManager.instance.soundVolume = soundVolume;

        SettingsManager.instance.gameMusicSource.volume = (musicVolume / 100f);


        slider_MusicVolume.value = musicVolume;
        slider_SoundVolume.value = soundVolume;

        settingsLoaded = true;
    }

    public void SaveSettings()
    {
        // TODO: Add saving of settings for user when leaving settings
        // soundVolume = slider_SoundVolume.value * 100;
        //  musicVolume = slider_MusicVolume.value * 100;

        SoundManager.instance.PlaySoundOnce("saveSettings", Vector3.zero, this.gameObject, true);

        soundVolume = (float)System.Math.Round(slider_SoundVolume.value);
        musicVolume = (float)System.Math.Round(slider_MusicVolume.value);

        musicVolumeEarlier = musicVolume;
        soundVolumeEarlier = soundVolume;

        SettingsManager.instance.musicVolume = musicVolume;
        SettingsManager.instance.soundVolume = soundVolume;

        SettingsManager.instance.menuMusicSource.volume = (musicVolume / 100f);
        SettingsManager.instance.gameMusicSource.volume = (musicVolume / 100f);


        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);

        settingsSaved = true;

        button_SaveChangesActive.gameObject.SetActive(false);
        button_SaveChangesDisabled.gameObject.SetActive(true);
    }
    #endregion
}
