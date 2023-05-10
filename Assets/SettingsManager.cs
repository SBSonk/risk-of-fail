using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using IniParser;
using IniParser.Model;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private int resolutionIndex, fpsLimitIndex;
    [SerializeField] private int resolutionWidth = 1920, resolutionHeight = 1080, fpsLimit = 60;
    [SerializeField] private bool fullscreen = true, vSync = true;
    [SerializeField] private float musicVolume = .5f, sfxVolume = .5f, masterVolume = .5f;
    
    private const string FILEPATH = "notes.ini";

    [SerializeField] private TMP_Dropdown resolutionDropdown, fpsDropdown;

    private void Start()
    {
        InitializeDropdowns();

        try
        {
            LoadSettings();
            Debug.Log("Saved!");
        }
        catch (Exception e)
        {
            Debug.Log("File not saved :(");
            Console.WriteLine(e);
            throw;
        }
        
    }

    void InitializeDropdowns()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<String> resolutionStrings = new List<string>();
        List<String> refreshRateStrings = new List<string>();
        foreach (var res in resolutions)
        {
            resolutionStrings.Add(res.width + "x" + res.height);
            refreshRateStrings.Add(res.refreshRate.ToString());
        }
        
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionStrings);
        
        fpsDropdown.ClearOptions();
        fpsDropdown.AddOptions(refreshRateStrings);
    }
    
    public void SaveSettings()
    {
        IniData data = new IniData();
        data["Volume"]["master"] = masterVolume.ToString();
        data["Volume"]["music"] = musicVolume.ToString();
        data["Volume"]["sfx"] = sfxVolume.ToString();

        data["Display"]["width"] = resolutionWidth.ToString();
        data["Display"]["height"] = resolutionHeight.ToString();
        data["Display"]["fpsLimit"] = fpsLimit.ToString();
        data["Display"]["fullscreen"] = fullscreen.ToString();
        data["Display"]["vsync"] = vSync.ToString();

        FileIniDataParser parser = new FileIniDataParser();
        parser.WriteFile(FILEPATH, data);
    }

    void InitializeIniFile()
    {
        IniData data = new IniData();
        data["Volume"]["master"] = "0.5";
        data["Volume"]["music"] = "0.5";
        data["Volume"]["sfx"] = "0.5";

        Resolution currentResolution = Screen.currentResolution;
        data["Display"]["width"] = currentResolution.width.ToString();
        data["Display"]["height"] = currentResolution.height.ToString();
        data["Display"]["fpsLimit"] = currentResolution.refreshRate.ToString();
        data["Display"]["fullscreen"] = Screen.fullScreen.ToString();
        data["Display"]["vsync"] = "true";

        FileIniDataParser parser = new FileIniDataParser();
        parser.WriteFile(FILEPATH, data);
    }
    
    public void LoadSettings()
    {
        if (!System.IO.File.Exists(FILEPATH)) InitializeIniFile();
        else
        {
            print("exists");
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(FILEPATH);;

            masterVolume = float.Parse(data["Volume"]["master"]);
            musicVolume = float.Parse(data["Volume"]["music"]);
            sfxVolume = float.Parse(data["Volume"]["sfx"]);

            resolutionWidth = int.Parse(data["Display"]["width"]);
            resolutionHeight = int.Parse(data["Display"]["height"]);
            fpsLimit = int.Parse(data["Display"]["fpsLimit"]);
            fullscreen = bool.Parse(data["Display"]["fullscreen"]);
            vSync = bool.Parse(data["Display"]["vsync"]);
        }
    }

    public void ApplyDisplaySettings()
    {
        Screen.SetResolution(resolutionWidth, resolutionHeight, Screen.fullScreen);
        Application.targetFrameRate = fpsLimit;
        Screen.fullScreen = fullscreen;
        
        // TODO: set vsync
        
        // TODO: Apply volume settings
    }
    
    public int ResolutionWidth
    {
        get => resolutionWidth;
        set => resolutionWidth = value;
    }

    public int ResolutionHeight
    {
        get => resolutionHeight;
        set => resolutionHeight = value;
    }

    public int FPSLimit
    {
        get => fpsLimit;
        set => fpsLimit = value;
    }

    public bool Fullscreen
    {
        get => fullscreen;
        set => fullscreen = value;
    }

    public bool VSync
    {
        get => vSync;
        set => vSync = value;
    }

    public float MusicVolume
    {
        get => musicVolume;
        set => musicVolume = value;
    }

    public float SfxVolume
    {
        get => sfxVolume;
        set => sfxVolume = value;
    }

    public float MasterVolume
    {
        get => masterVolume;
        set => masterVolume = value;
    }

    public int ResolutionIndex
    {
        get => resolutionIndex;
        set => resolutionIndex = value;
    }

    public int FPSLimitIndex
    {
        get => fpsLimitIndex;
        set => fpsLimitIndex = value;
    }
}
