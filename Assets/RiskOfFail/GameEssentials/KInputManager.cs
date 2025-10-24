using System;
using System.Collections.Generic;
using System.IO;
using IniParser;
using IniParser.Model;
using UnityEngine;

public class KInputManager : MonoBehaviour
{
    public static KInputManager instance;

    public static Dictionary<string, KeyBind> keyBindDictionary;

    private static readonly string FILEPATH = "controls.ini";
    [SerializeField] private KeyBind[] keyBinds;

    private readonly KeyBind[] defaultBinds =
    {
        new KeyBind("Up", KeyCode.W, KeyCode.UpArrow),
        new KeyBind("Down", KeyCode.S, KeyCode.DownArrow),
        new KeyBind("Right", KeyCode.D, KeyCode.RightArrow),
        new KeyBind("Left", KeyCode.A, KeyCode.LeftArrow),
        new KeyBind("Dash", KeyCode.LeftShift, KeyCode.Space),
        new("Shoot", KeyCode.Mouse0),
        new("Shove", KeyCode.Mouse1, KeyCode.V),
        new("Reload", KeyCode.R),
        new("Interact", KeyCode.F),
        new("PreviousWeapon", KeyCode.Q),
        new("NextWeapon", KeyCode.E),
        new("WeaponA", KeyCode.Alpha1),
        new("WeaponB", KeyCode.Alpha2),
        new("WeaponC", KeyCode.Alpha3),
        new("SpecialShoot", KeyCode.None)
    };

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

        InitializeKeys();
        LoadKeys();
    }

    public void ResetBinds()
    {
        keyBindDictionary = new Dictionary<string, KeyBind>();

        foreach (var keybind in defaultBinds) keyBindDictionary.Add(keybind.name, keybind);

        print("KeyBinds Reset.");
    }

    public void ClearBinds()
    {
        foreach (var key in keyBindDictionary) key.Value.Reset();

        print("KeyBinds cleared.");
    }

    private void InitializeKeys()
    {
        keyBindDictionary = new Dictionary<string, KeyBind>();

        foreach (var keybind in keyBinds) keyBindDictionary.Add(keybind.name, keybind);

        print("KeyBinds initialized.");
    }

    private void LoadKeys()
    {
        if (!File.Exists(FILEPATH))
        {
            SaveKeys();
        }
        else
        {
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(FILEPATH);

            foreach (var key in data.Sections)
            {
                keyBindDictionary[key.SectionName].SetPrimary((KeyCode)int.Parse(key.Keys["Primary"]));
                keyBindDictionary[key.SectionName].SetSecondary((KeyCode)int.Parse(key.Keys["Secondary"]));
            }
        }

        print("KeyBinds loaded.");
    }

    public void SaveKeys()
    {
        var data = new IniData();
        foreach (var key in keyBindDictionary)
        {
            data[key.Key]["Primary"] = ((int)key.Value.primary).ToString();
            data[key.Key]["Secondary"] = ((int)key.Value.secondary).ToString();
        }

        var parser = new FileIniDataParser();
        parser.WriteFile(FILEPATH, data);

        print("KeyBinds saved.");
    }

    public static KeyBind GetKey(string keyName)
    {
        if (keyBindDictionary.TryGetValue(keyName, out var key)) return key;

        Debug.LogError("KeyBind '" + keyName + "' does not exist.");
        return null;
    }
}

[Serializable]
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
        if (primary == KeyCode.None)
        {
            primary = key;
        }
        else if (secondary == KeyCode.None)
        {
            if (key == primary) return;
            secondary = key;
        }
        else
        {
            Reset();
            primary = key;
        }
    }

    public void SetPrimary(KeyCode key)
    {
        primary = key;
    }

    public void SetSecondary(KeyCode key)
    {
        if (key == primary && key != KeyCode.None) return;
        secondary = key;
    }

    public bool Pressed()
    {
        return Input.GetKey(primary) || Input.GetKey(secondary);
    }

    public bool PressedDown()
    {
        return Input.GetKeyDown(primary) || Input.GetKeyDown(secondary);
    }

    public bool Released()
    {
        return Input.GetKeyUp(primary) || Input.GetKeyUp(secondary);
    }

    public static float GetAxis(KeyBind positive, KeyBind negative)
    {
        return Mathf.Clamp((positive.Pressed() ? 1 : 0) + (negative.Pressed() ? -1 : 0), -1, 1);
    }

    public static int GetAxisInt(KeyBind positive, KeyBind negative)
    {
        return Mathf.RoundToInt(GetAxis(positive, negative));
    }
}