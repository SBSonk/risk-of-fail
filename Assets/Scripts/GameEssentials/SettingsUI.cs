using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_InputField fpsInputField;
    [SerializeField] private Toggle fullscreenToggle, vSyncToggle;
    [SerializeField] private Button applyButton;

    public static SettingsUI instance;

    private void Awake()
    {
        instance = this;
        
        if (SettingsManager.instance) InitializeSettings();
    }

    public void InitializeSettings()
    {
        InitializeDropdowns();
        InitializeSettingsValues();
    }

    void InitializeSettingsValues()
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

    void UpdateValues()
    {
        SettingsManager.instance.MasterVolume = masterSlider.value;
        SettingsManager.instance.MusicVolume = musicSlider.value;
        SettingsManager.instance.SfxVolume = sfxSlider.value;
        
        SettingsManager.instance.SetFPSLimit(fpsInputField.text);
        SettingsManager.instance.Fullscreen = fullscreenToggle.isOn;
        SettingsManager.instance.VSync = vSyncToggle.isOn;

        SettingsManager.instance.ResolutionIndex = resolutionDropdown.value;
    }

    void InitializeDropdowns()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<string> resolutionStrings = new List<string>();
        int resolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (SettingsManager.instance.ResolutionWidth == resolutions[i].width && SettingsManager.instance.ResolutionHeight == resolutions[i].height)
                resolutionIndex = i;
            
            resolutionStrings.Add(resolutions[i].width + "x" + resolutions[i].height + "@" + resolutions[i].refreshRate);
        }
        
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionStrings);
        
        resolutionDropdown.value = resolutionIndex;
    }
}
