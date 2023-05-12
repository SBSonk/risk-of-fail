using UnityEngine;
using System;
using System.Collections.Generic;
using IniParser;
using IniParser.Model;

public class KInputManager : MonoBehaviour
{
    public static KInputManager instance;
    
    public static Dictionary<string, KeyBind> keyBindDictionary;
    [SerializeField] KeyBind[] keyBinds;

    private static string FILEPATH = "controls.ini";
    
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        InitializeKeys();
        SaveKeys(); 
    }

    public void ClearBinds()
    {
        foreach (KeyValuePair<string,KeyBind> key in keyBindDictionary)
        {
            key.Value.Reset();
        }
        
        print("KeyBinds cleared.");
    }
    
    void InitializeKeys()
    {
        keyBindDictionary = new Dictionary<string, KeyBind>();
        
        foreach (var keybind in keyBinds)
        {
            keyBindDictionary.Add(keybind.name, keybind);
        }
        
        print("KeyBinds initialized.");
    }

    void LoadKeys()
    {
        /*if (!System.IO.File.Exists(FILEPATH)) InitializeIniFile();
        else
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(FILEPATH);;
        }*/
        
        print("KeyBinds loaded.");
    }

    void SaveKeys()
    {
        IniData data = new IniData();
        foreach (KeyValuePair<string,KeyBind> key in keyBindDictionary)
        {
            data[key.Key]["Primary"] = ((int) key.Value.primary).ToString();
            data[key.Key]["Secondary"] = ((int) key.Value.secondary).ToString();
        }

        FileIniDataParser parser = new FileIniDataParser();
        parser.WriteFile(FILEPATH, data);
        
        print("KeyBinds saved.");
    }
    
    public static KeyBind GetKey(String keyName)
    {
        if (keyBindDictionary.TryGetValue(keyName, out var key)) return key;
        
        Debug.LogError("KeyBind '" + keyName + "' does not exist.");
        return null;
    }
}

[System.Serializable]
public class KeyBind
{
    public string name = "New Keybind";
    
    public KeyCode primary = KeyCode.None;
    public KeyCode secondary = KeyCode.None;

    public KeyBind(string _name, KeyCode _primary, KeyCode _secondary = KeyCode.None)
    {
        name = _name;
        primary = _primary;
        secondary = _secondary;
    }
    
    public void Reset()
    {
        primary = KeyCode.None;
        secondary = KeyCode.None;
    }

    public void AddKey(KeyCode key)
    {
        if (primary == KeyCode.None) primary = key;
        else if (secondary == KeyCode.None) secondary = key;
        else
        {
            Reset();
            primary = key;
        }
    }

    public void SetPrimary(KeyCode key) => primary = key;
    public void SetSecondary(KeyCode key) => secondary = key;

    public bool Pressed() => Input.GetKey(primary) || Input.GetKey(secondary);
    public bool PressedDown() => Input.GetKeyDown(primary) || Input.GetKeyDown(secondary);
    public bool Released() => Input.GetKeyUp(primary) || Input.GetKeyUp(secondary);

    public static float GetAxis(KeyBind positive, KeyBind negative) => Mathf.Clamp((positive.Pressed() ? 1 : 0) + (negative.Pressed() ? -1 : 0), -1, 1);
    public static int GetAxisInt(KeyBind positive, KeyBind negative) => Mathf.RoundToInt(GetAxis(positive, negative));
}