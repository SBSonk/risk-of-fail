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
        while (KInputManager.GetKey("Shoot").Pressed() && shooting.GetHeldWeapon().clip > 0)
        {
            // raycast
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(player.position, (mousePosition - player.position).normalized, contactFilter.layerMask);
            if (hit) {
                // attach end of line to hit

                // if enemy, damage
                if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.GiveDamage(baseDamage * multiplier, stunLength, KillFlag.Ranged);
                }
            }

            shooting.GetHeldWeapon().clip--;
            shooting.OnShoot.Invoke();
            shooting.OnAmmoUpdate.Invoke(shooting.GetHeldWeapon());
            yield return new WaitForSeconds(fireRate);
        }
    }
}
