using System;
using System.IO;
using IniParser;
using IniParser.Model;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    private const string FILEPATH = "notes.ini";
    public static SettingsManager instance;

    [SerializeField] private int resolutionIndex;
    [SerializeField] private int resolutionWidth = 1920, resolutionHeight = 1080, fpsLimit = 60;
    [SerializeField] private bool fullscreen = true, vSync = true;
    [SerializeField] private float musicVolume = .5f, sfxVolume = .5f, masterVolume = .5f;

    [SerializeField] private AudioMixer mixer;

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
            resolutionIndex = Screen.resolutions.Length - 1;
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
            var data = new IniData();
            data["Volume"]["master"] = masterVolume.ToString();
            data["Volume"]["music"] = musicVolume.ToString();
            data["Volume"]["sfx"] = sfxVolume.ToString();

            data["Display"]["index"] = resolutionIndex.ToString();
            data["Display"]["width"] = resolutionWidth.ToString();
            data["Display"]["height"] = resolutionHeight.ToString();
            data["Display"]["fpsLimit"] = fpsLimit.ToString();
            data["Display"]["fullscreen"] = fullscreen.ToString();
            data["Display"]["vsync"] = vSync.ToString();

            var parser = new FileIniDataParser();
            parser.WriteFile(FILEPATH, data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void InitializeIniFile()
    {
        var data = new IniData();
        data["Volume"]["master"] = "0.5";
        data["Volume"]["music"] = "0.5";
        data["Volume"]["sfx"] = "0.5";

        var currentResolution = Screen.resolutions[^1];
        data["Display"]["index"] = (Screen.resolutions.Length - 1).ToString();
        data["Display"]["width"] = currentResolution.width.ToString();
        data["Display"]["height"] = currentResolution.height.ToString();
        data["Display"]["fpsLimit"] = currentResolution.refreshRate.ToString();
        data["Display"]["fullscreen"] = Screen.fullScreen.ToString();
        data["Display"]["vsync"] = "true";

        var parser = new FileIniDataParser();
        parser.WriteFile(FILEPATH, data);

        LoadSettings();
    }

    public void LoadSettings()
    {
        if (!File.Exists(FILEPATH)) InitializeIniFile();
        else
            try
            {
                print("exists");
                var parser = new FileIniDataParser();
                var data = parser.ReadFile(FILEPATH);

                masterVolume = float.Parse(data["Volume"]["master"]);
                musicVolume = float.Parse(data["Volume"]["music"]);
                sfxVolume = float.Parse(data["Volume"]["sfx"]);

                resolutionIndex = int.Parse(data["Display"]["index"]);
                resolutionWidth = int.Parse(data["Display"]["width"]);
                resolutionHeight = int.Parse(data["Display"]["height"]);
                fpsLimit = int.Parse(data["Display"]["fpsLimit"]);
                fullscreen = bool.Parse(data["Display"]["fullscreen"]);
                vSync = bool.Parse(data["Display"]["vsync"]);
            }
            catch
            {
                Debug.LogError("Settings file mismatch.");

                InitializeIniFile();
            }
    }

    public void ApplySettings()
    {
        Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height,
            fullscreen,
            Screen.resolutions[resolutionIndex].refreshRate);
        Application.targetFrameRate = fpsLimit;

        QualitySettings.vSyncCount = vSync ? 1 : 0;

        mixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat("Sfx", Mathf.Log10(sfxVolume) * 20);

        SaveSettings();
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