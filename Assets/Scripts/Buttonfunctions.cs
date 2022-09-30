using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttonfunctions : MonoBehaviour
{
    public void EnterShop ()
    {
        SceneManager.LoadScene(2);
    }
    
    public void ExitShop ()
    {
        SceneManager.LoadScene(1);
    }

    public void Start()
    {
        SceneManager.LoadScene(1);
    }
}
