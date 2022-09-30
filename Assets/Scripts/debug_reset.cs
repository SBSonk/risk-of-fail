using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class debug_reset : MonoBehaviour
{
    public KeyCode resetKey;
    private void Update()
    {
        if (Input.GetKeyDown(resetKey)) SceneManager.LoadScene(0);
    }
}
