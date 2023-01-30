using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapons/Melee")]
public class MeleeWeapon : Weapon
{
    public float swingDelay = 0.1f;
    public float hitArea = 2;

    public IEnumerator SwingWeapon(Transform player)
    {
        yield return new WaitForSeconds(swingDelay);

        Vector3 hitVector = ((Vector3)InputManager.mousePosition - player.position).normalized;

        // Attack check
        Collider2D[] col = Physics2D.OverlapCircleAll(player.position + hitVector * hitArea , hitArea);
        List<Alive> hit = new List<Alive>();
        foreach(Collider2D c in col)
        {
            if (c.TryGetComponent(out Alive a) && !c.CompareTag("Player") && !hit.Contains(a))
            {
                a.GiveDamage(baseDamage, stunLength);
                if (a.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(hitVector * knockbackAmount, ForceMode2D.Impulse);
                }

                hit.Add(a);
            }
        }

        if (shootShake) CameraShakerHandler.Shake(shootShake);
        if (hit.Count > 0)
        {
            // Screenshake
            if (hitScreenShake) CameraShakerHandler.Shake(hitScreenShake);
        }
    }
}
