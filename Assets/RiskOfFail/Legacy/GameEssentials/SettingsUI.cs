using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI instance;
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_InputField fpsInputField;
    [SerializeField] private Toggle fullscreenToggle, vSyncToggle;
    [SerializeField] private Button applyButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (SettingsManager.instance) InitializeSettings();
    }

    public void InitializeSettings()
    {
        InitializeDropdowns();
        InitializeSettingsValues();
    }

    private void InitializeSettingsValues()
    {
        masterSlider.value = SettingsManager.instance.MasterVolume;
        musicSlider.value = SettingsManager.instance.MusicVolume;
        sfxSlider.value = SettingsManager.instance.SfxVolume;

        fpsInputField.text = SettingsManager.instance.FPSLimit.ToString();
        fullscreenToggle.isOn = SettingsManager.instance.Fullscreen;
        vSyncToggle.isOn = SettingsManager.instance.VSync;

        applyButton.onClick.AddListener(() =>
        {
            UpdateValues();
            SettingsManager.instance.ApplySettings();
        });
    }

    private void UpdateValues()
    {
        SettingsManager.instance.MasterVolume = masterSlider.value;
        SettingsManager.instance.MusicVolume = musicSlider.value;
        SettingsManager.instance.SfxVolume = sfxSlider.value;

        SettingsManager.instance.SetFPSLimit(fpsInputField.text);
        SettingsManager.instance.Fullscreen = fullscreenToggle.isOn;
        SettingsManager.instance.VSync = vSyncToggle.isOn;

        SettingsManager.instance.ResolutionIndex = resolutionDropdown.value;
    }

    private void InitializeDropdowns()
    {
        var resolutions = Screen.resolutions;
        var resolutionStrings = new List<string>();
        var resolutionIndex = 0;
        for (var i = 0; i < resolutions.Length; i++)
        {
            if (SettingsManager.instance.ResolutionWidth == resolutions[i].width &&
                SettingsManager.instance.ResolutionHeight == resolutions[i].height)
                resolutionIndex = i;

            resolutionStrings.Add(resolutions[i].width + "x" + resolutions[i].height + "@" +
                                  resolutions[i].refreshRate);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionStrings);

        resolutionDropdown.value = resolutionIndex;
    }
}