using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeybindButton : MonoBehaviour
{
    [SerializeField] private ControlsUI ui;
    
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string keyToChange;

    private void Start()
    {
        UpdateKeyUI();
        
        ui.ControlBinded.AddListener(UpdateKeyUI);
    }

    void UpdateKeyUI()
    {
        KeyBind key = KInputManager.GetKey(keyToChange);
        buttonText.text = key.primary.ToString();
        if (key.secondary != KeyCode.None) buttonText.text += " / " + key.secondary;
    }
}
