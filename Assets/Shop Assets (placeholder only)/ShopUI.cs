using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Notes:
 * Opens the shop through key press
 */

public class ShopUI : MonoBehaviour
{
    public bool isShopOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isShopOpen = !isShopOpen; // Added toggle to adjust for gamemanager object existing between scenes

            if (isShopOpen) SceneManager.LoadScene(1);
            else SceneManager.LoadScene(0); 

            /*
            if (isShopOpen == false)
            {
                Debug.Log("Shop opened");
                isShopOpen = true;
                SceneManager.LoadScene(1);
            }
            else if (isShopOpen == true)
            {
                Debug.Log("Shop closed");
                SceneManager.LoadScene(0);
            }*/
        }
    }
}