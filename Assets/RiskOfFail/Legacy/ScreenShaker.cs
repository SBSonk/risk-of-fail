using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public ShakeData screenShake;

    public void ShakeScreen()
    {
        CameraShakerHandler.Shake(screenShake);
    }
}
