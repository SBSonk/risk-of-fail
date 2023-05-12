using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsUI : MonoBehaviour
{
    public GameObject setBindUI;
    public TextMeshProUGUI bindTimer;


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
                    KInputManager.GetKey(keyChanging).AddKey(key);
                    setBindUI.SetActive(false);
                    changingBind = false;
                }
            }
        }
    }

    IEnumerator WaitForBind()
    {
        setBindUI.SetActive(true);
        changingBind = true;

        int seconds = 5;
        bindTimer.text = "Press a button. (5)";
        for (int i = seconds; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            bindTimer.text = "Press a button. (" + i + ")";
        }

        setBindUI.SetActive(false);
        changingBind = false;
    }
}
