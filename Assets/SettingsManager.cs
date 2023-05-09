using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IniParser;
using IniParser.Model;

public class SettingsManager : MonoBehaviour
{
    // Resolution, Post processing, fullscreen, FPS limit, Vsync
    // Controls
    private int resolutionWidth = 1920, resolutionHeight = 1080, fpsLimit = 60;
    private bool fullscreen = true, vSync = true;
    private float musicVolume = .5f, sfxVolume = .5f, masterVolume = .5f;
    
    private const string FILEPATH = "notes.ini";

    private void Start()
    {
        try
        {
            SaveSettings();
            print("Svaed!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
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
        
    }
    
    public void LoadSettings()
    {
        FileIniDataParser parser = new FileIniDataParser();
        IniData data = parser.ReadFile(FILEPATH);;
        
        
    }

}
