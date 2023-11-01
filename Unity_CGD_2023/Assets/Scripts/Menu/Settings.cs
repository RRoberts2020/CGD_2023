using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public SettingsManager settingsManager;

    private void Start()
    {
        settingsManager.LoadSettings();
    }

    public void ApplyGraphics()
    {
        settingsManager.ApplySettings();
    }

    public void ResetToDefaults()
    {
        settingsManager.ResetToDefaults();
    }

    public void SetMasterVolume()
    {
        settingsManager.SetMasterVolume();
    }

    public void SetSfxVolume()
    {
        settingsManager.SetSfxVolume();
    }

    public void SetMusicVolume()
    {
        settingsManager.SetMusicVolume();
    }

    public void FrameRateLeft()
    {
        settingsManager.FrameRateLeft();
    }

    public void FrameRateRight()
    {
        settingsManager.FrameRateRight();
    }

    public void ResLeft()
    {
        settingsManager.ResLeft();
    }
    public void ResRight()
    {
        settingsManager.ResRight();
    }
}

/*public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public TMP_Text resolutionLabel;
    public TMP_Text frameRateLabel;
    public TMP_Text masterVolumeLabel;
    public TMP_Text sfxVolumeLabel;
    public TMP_Text musicVolumeLabel;
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Resolution Options")]
    public List<ResolutionOption> resolutionOptions = new List<ResolutionOption>();
    private int selectedResolutionIndex;

    [Header("Frame Rate Options")]
    public List<FrameRateOption> frameRateOptions = new List<FrameRateOption>();
    private int selectedFrameRateIndex;

    private float cachedMasterVolume;
    private float cachedSfxVolume;
    private float cachedMusicVolume;

    private void Start()
    {
        LoadGraphicsSettings();
        LoadAudioSettings();
    }

    private void LoadGraphicsSettings()
    {
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        vsyncToggle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
        LoadResolution();
        if (vsyncToggle.isOn)
        {
            Application.targetFrameRate = 60;
            frameRateLabel.text = "60";
        }
        else
        {
            LoadFrameRate();
        }
    }

    private void LoadResolution()
    {
        selectedResolutionIndex = PlayerPrefs.GetInt("SelectedResolution", 0);
        UpdateResolutionLabel();
        ApplyResolution();
    }

    private void LoadFrameRate()
    {
        selectedFrameRateIndex = PlayerPrefs.GetInt("SelectedFrameRate", 0);
        UpdateFrameRateLabel();
        ApplyFrameRate();
    }

    private void LoadAudioSettings()
    {
        cachedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0f);
        cachedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0f);
        cachedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);

        masterVolumeSlider.value = cachedMasterVolume;
        sfxVolumeSlider.value = cachedSfxVolume;
        musicVolumeSlider.value = cachedMusicVolume;

        UpdateVolumeLabels();
    }

    public void FrameRateLeft()
    {
        selectedFrameRateIndex--;
        if (selectedFrameRateIndex < 0)
        {
            selectedFrameRateIndex = 0;
        }
        UpdateFrameRateLabel();
    }

    public void FrameRateRight()
    {
        selectedFrameRateIndex++;
        if (selectedFrameRateIndex >= frameRateOptions.Count)
        {
            selectedFrameRateIndex = frameRateOptions.Count - 1;
        }
        UpdateFrameRateLabel();
    }

    public void ResLeft()
    {
        selectedResolutionIndex--;
        if (selectedResolutionIndex < 0)
        {
            selectedResolutionIndex = 0;
        }
        UpdateResolutionLabel();
    }

    public void ResRight()
    {
        selectedResolutionIndex++;
        if (selectedResolutionIndex >= resolutionOptions.Count)
        {
            selectedResolutionIndex = resolutionOptions.Count - 1;
        }
        UpdateResolutionLabel();
    }

    private void UpdateResolutionLabel()
    {
        resolutionLabel.text = resolutionOptions[selectedResolutionIndex].ToString();
    }

    private void UpdateFrameRateLabel()
    {
        frameRateLabel.text = frameRateOptions[selectedFrameRateIndex].ToString();
    }

    private void UpdateVolumeLabels()
    {
        masterVolumeLabel.text = (masterVolumeSlider.value + 80).ToString();
        sfxVolumeLabel.text = (sfxVolumeSlider.value + 80).ToString();
        musicVolumeLabel.text = (musicVolumeSlider.value + 80).ToString();
    }

    public void ApplyGraphics()
    {
        ApplyResolution();
        ApplyScreenMode();
        if (vsyncToggle.isOn)
        {
            Application.targetFrameRate = 60;
            frameRateLabel.text = "60";
            PlayerPrefs.SetInt("SelectedFrameRate", 0);
        }
        else
        {
            ApplyFrameRate();
            PlayerPrefs.SetInt("SelectedFrameRate", selectedFrameRateIndex);
        }
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("VSync", vsyncToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("SelectedResolution", selectedResolutionIndex);
    }
    public void ResetToDefaults()
    {
        PlayerPrefs.DeleteKey("Fullscreen");
        PlayerPrefs.DeleteKey("VSync");
        PlayerPrefs.DeleteKey("SelectedResolution");
        PlayerPrefs.DeleteKey("SelectedFrameRate");
        PlayerPrefs.DeleteKey("MasterVolume");
        PlayerPrefs.DeleteKey("SfxVolume");
        PlayerPrefs.DeleteKey("MusicVolume");

        LoadGraphicsSettings();
        LoadAudioSettings();
    }

    private void ApplyResolution()
    {
        ResolutionOption selectedResolution = resolutionOptions[selectedResolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
    }

    private void ApplyFrameRate()
    {
        FrameRateOption selectedFrameRate = frameRateOptions[selectedFrameRateIndex];
        Application.targetFrameRate = selectedFrameRate.frameRate;
    }

    private void ApplyScreenMode()
    {
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
    }

    public void SetMasterVolume()
    {
        cachedMasterVolume = masterVolumeSlider.value;
        audioMixer.SetFloat("MasterVolume", cachedMasterVolume);
        PlayerPrefs.SetFloat("MasterVolume", cachedMasterVolume);
        UpdateVolumeLabels();
    }

    public void SetSfxVolume()
    {
        cachedSfxVolume = sfxVolumeSlider.value;
        audioMixer.SetFloat("SfxVolume", cachedSfxVolume);
        PlayerPrefs.SetFloat("SfxVolume", cachedSfxVolume);
        UpdateVolumeLabels();
    }

    public void SetMusicVolume()
    {
        cachedMusicVolume = musicVolumeSlider.value;
        audioMixer.SetFloat("MusicVolume", cachedMusicVolume);
        PlayerPrefs.SetFloat("MusicVolume", cachedMusicVolume);
        UpdateVolumeLabels();
    }
}

[System.Serializable]
public class ResolutionOption
{
    public int width;
    public int height;

    public override string ToString()
    {
        return $"{width} X {height}";
    }
}

[System.Serializable]
public class FrameRateOption
{
    public int frameRate;

    public override string ToString()
    {
        return frameRate.ToString();
    }
}*/
