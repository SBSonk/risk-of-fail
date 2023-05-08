using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public static void Restart()
    {
        Time.timeScale = 1;
        
        LevelFade.FadeIn(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LevelFade.FadeOut();
        });
    }

    public static void SwitchScene(int sceneNum)
    {
        Time.timeScale = 1;
        
        LevelFade.FadeIn(() =>
        {
            SceneManager.LoadScene(sceneNum);
            LevelFade.FadeOut();
        });
    }

    public static void ExitGame()
    {
        // Save game here
        LevelFade.FadeIn(() =>
        {
            Application.Quit();
        });
    }
}
