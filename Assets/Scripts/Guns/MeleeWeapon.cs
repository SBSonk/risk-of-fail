using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Melee", menuName = "Melee")]
public class MeleeWeapon : Weapon
{
    public float hitArea = 2;

    public override void ShootWeapon(Transform player)
    {
        // Check for enemies in area
        Vector3 hitVector = ((Vector3)InputManager.mousePosition - player.position).normalized;
        Collider2D[] col = Physics2D.OverlapCircleAll(player.position + hitVector, hitArea);

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

            Debug.Log(c.name); // collides twice with written works since they have 2 colliders

        }

        base.ShootWeapon(player);
    }
}
