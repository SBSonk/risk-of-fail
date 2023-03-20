using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttonfunctions : MonoBehaviour
{
    public static RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public static void EnterShop()
    {
        SceneManager.LoadScene(2);
    }
    
    public static void ExitShop()
    {
        SceneManager.LoadScene(1);
    }

    public static void Play(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public static void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public static void ExitGame()
    {
        // Save game here

        Application.Quit();
    }
}
