using System;
using UnityEngine;
using IniParser;
using IniParser.Model;
using UnityEngine.Audio;


public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    
    [SerializeField] private int resolutionIndex;
    [SerializeField] private int resolutionWidth = 1920, resolutionHeight = 1080, fpsLimit = 60;
    [SerializeField] private bool fullscreen = true, vSync = true;
    [SerializeField] private float musicVolume = .5f, sfxVolume = .5f, masterVolume = .5f;

    [SerializeField] private AudioMixer mixer;
    private const string FILEPATH = "notes.ini";

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        try
        {
            LoadSettings();
            ApplySettings();
            Debug.Log("Loaded!");
        }
        catch (Exception e)
        {
            Debug.Log("File not loaded :(");
            InitializeIniFile();
            Console.WriteLine(e);
            throw;
        }
    }

    private void Start()
    {
        SettingsUI.instance.InitializeSettings();
    }

    public void SaveSettings()
    {
        try
        {
            print("appl");
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
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
        
        LoadSettings();
    }
    
    public void LoadSettings()
    {
        if (!System.IO.File.Exists(FILEPATH)) InitializeIniFile();
        else
        {
            print("exists");
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(FILEPATH);

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

    public void ApplySettings()
    {
        Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreen,
            Screen.resolutions[resolutionIndex].refreshRate);
        Application.targetFrameRate = fpsLimit;

        QualitySettings.vSyncCount = vSync ? 1 : 0;
        
        mixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat("Sfx", Mathf.Log10(sfxVolume) * 20);

        SaveSettings();
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
        set
        {
            musicVolume = value;
            mixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
            SaveSettings(); 
        }
    }

    public float SfxVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            mixer.SetFloat("Sfx", Mathf.Log10(sfxVolume) * 20);
            SaveSettings(); 
        }
    }

    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = value;
            mixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20);
            SaveSettings(); 
        }
    }

    public int ResolutionIndex
    {
        get => resolutionIndex;
        set
        {
            resolutionIndex = value;
            resolutionWidth = Screen.resolutions[resolutionIndex].width;
            resolutionHeight = Screen.resolutions[resolutionIndex].height;
        }
    }

    public void SetFPSLimit(string fpsString)
    {
        try
        {
            FPSLimit = int.Parse(fpsString);
        }
        catch (Exception e)
        {
            
        }
    }
}
