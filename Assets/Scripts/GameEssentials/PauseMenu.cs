using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public static bool settings = false;

    public GameObject pauseMenuUI;
    public GameObject pauseButtons;
    public GameObject settingsButtons;

    // reset the paused and settings variables since they are static. on start
    void Start ()
    {
        paused = false;
        Time.timeScale = 1f;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {                     
            
            if (settings)
            {
                Back();
            }
            else if (paused)
            {
                Resume();

                MoveCursor.main.LeaveMenu();
            }
            else
            {
                Pause();

                MoveCursor.main.EnterMenu();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Settings()
    {
        pauseButtons.SetActive(false);
        settings = true;
        settingsButtons.SetActive(true);
    }

    public void Back()
    {
        settingsButtons.SetActive(false);
        settings = false;
        pauseButtons.SetActive(true);
    }
}
