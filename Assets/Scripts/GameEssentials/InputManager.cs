using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Vector2 playerDirection;
    public static Vector2 mouseRawPosition, mousePosition, mouseMovement;
    public static bool shoot, shootAuto, shootRelease, dodge, reload;
    public static bool shove;
    public static int swapWeapon;
    public static bool interact;

    public void LoadInputs()
    {
        
    }
    
    private void Update()
    {
        mouseRawPosition = Input.mousePosition;

        if (PauseMenu.paused) return;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        shoot = Input.GetButtonDown("Shoot");
        shootAuto = Input.GetButton("Shoot");
        shootRelease = Input.GetButtonUp("Shoot");
        shove = Input.GetButtonDown("Shove");
        dodge = Input.GetButtonDown("Dodge");
        reload = Input.GetButtonDown("Reload");
        interact = Input.GetKeyDown(KeyCode.F);

        if (Input.GetButtonDown("SwapLeft")) swapWeapon = -1;
        else if (Input.GetButtonDown("SwapRight")) swapWeapon = 1;
        else swapWeapon = 0;

        swapWeapon += (int) Input.GetAxisRaw("Mouse ScrollWheel");

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                print(key);
                print((int) key);
            }
            else
            {
                print((KeyCode) 100);
            }
        }
    }
}

/*
public enum 
*/
