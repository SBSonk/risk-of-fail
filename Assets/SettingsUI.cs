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
    void Start()
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
    }

    void InitializeDropdowns()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<string> resolutionStrings = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
                resolutionDropdown.value = i;
            
            resolutionStrings.Add(resolutions[i].width + "x" + resolutions[i].height);
        }
        
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionStrings);
    }
}
