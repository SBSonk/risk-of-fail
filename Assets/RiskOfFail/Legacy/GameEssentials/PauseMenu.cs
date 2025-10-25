using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool paused;
    public static bool settings;

    public GameObject pauseMenuUI;
    public GameObject pauseButtons;
    public GameObject settingsButtons;

    // reset the paused and settings variables since they are static. on start
    private void Start()
    {
        paused = false;
        Time.timeScale = 1f;
    }

    private void Update()
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

                MoveCursor.instance.LeaveMenu();
            }
            else
            {
                Pause();

                MoveCursor.instance.EnterMenu();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    private void Pause()
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