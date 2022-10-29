using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void EnterShop ()
    {
        SceneManager.LoadScene(2);
    }
    
    public void ExitShop ()
    {
        SceneManager.LoadScene(1);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        // Save game here

        Application.Quit();
    }
}
