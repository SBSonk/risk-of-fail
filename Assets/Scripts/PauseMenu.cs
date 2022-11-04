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

    void Start()
    {
        pauseMenuUI.SetActive(false);
        settingsButtons.SetActive(false);
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
            }
            else
            {
                Pause();
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
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
