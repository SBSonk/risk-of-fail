using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    PauseMenu resume;

    private void Start()
    {
        GameObject r = GameObject.Find("Rect");
        resume = r.GetComponent<PauseMenu>();
    }
    public void EnterShop()
    {
        SceneManager.LoadScene(2);
    }
    
    public void ExitShop()
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

    public void Resume()
    {
        resume.Resume();
    }
}
