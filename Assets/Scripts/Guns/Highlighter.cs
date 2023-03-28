using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Highlighter", menuName = "Weapons/Highlighter")]
public class Highlighter : Weapon
{
    public ContactFilter2D contactFilter;

    // make a new shootweapon variant
    public void ShootWeapon(Transform player, PlayerShooting shooting, float multiplier = 1)
    {
        shooting.StartCoroutine(ShootSustain(player, shooting, multiplier));
    }

    IEnumerator ShootSustain(Transform player, PlayerShooting shooting, float multiplier = 1)
    {
        while (InputManager.shootAuto && shooting.GetHeldWeapon().clip > 0)
        {
            // raycast
            RaycastHit2D hit = Physics2D.Raycast(player.position, ((Vector3) InputManager.mousePosition - player.position).normalized, contactFilter.layerMask);
            if (hit) {
                // attach end of line to hit

                // if enemy, damage
                if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.GiveDamage(baseDamage * multiplier, stunLength);
                }
            }

            shooting.GetHeldWeapon().clip--;
            shooting.OnShoot.Invoke();
            shooting.OnAmmoUpdate.Invoke(shooting.GetHeldWeapon());
            yield return new WaitForSeconds(fireRate);
        }
    }
}
