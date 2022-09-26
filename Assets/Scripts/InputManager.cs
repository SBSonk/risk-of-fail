using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Vector2 playerDirection;
    public static Vector2 mousePosition, mouseMovement;
    public static bool shoot, shootAuto, dodge, reload;
    public static bool shove;
    public static int swapWeapon;

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        shoot = Input.GetButtonDown("Shoot");
        shootAuto = Input.GetButton("Shoot");
        shove = Input.GetButtonDown("Shove");
        dodge = Input.GetButtonDown("Dodge");
        reload = Input.GetButtonDown("Reload");

        if (Input.GetButtonDown("SwapLeft")) swapWeapon = -1;
        else if (Input.GetButtonDown("SwapRight")) swapWeapon = 1;
        else swapWeapon = 0;
    }
}
