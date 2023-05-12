using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine.Events;

public class ControlsUI : MonoBehaviour
{
    public GameObject setBindUI;
    public TextMeshProUGUI bindTimer;

    public UnityEvent ControlBinded, BindCanceled;
    
    private string keyChanging;
    private bool changingBind = false;
    public void ChangeBind(string keyName)
    {
        keyChanging = keyName;
        StartCoroutine(WaitForBind());
    }

    private void Update()
    {
        if (changingBind)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    StopAllCoroutines();
                    
                    KInputManager.GetKey(keyChanging).AddKey(key);
                    setBindUI.SetActive(false);
                    changingBind = false;
                    
                    ControlBinded?.Invoke();
                }
            }
        }
    }

    IEnumerator WaitForBind()
    {
        setBindUI.SetActive(true);
        changingBind = true;

        int seconds = 5;
        for (int i = seconds; i > 0; i--)
        {
            bindTimer.text = "Press a button. (" + i + ")";
            yield return new WaitForSeconds(1);
            
        }

        setBindUI.SetActive(false);
        changingBind = false;

        BindCanceled?.Invoke();
    }
}
