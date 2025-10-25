using System.Collections;
using UnityEngine;

public class Ruler : MeleeWeapon
{
    public float dashLength;

    public override IEnumerator SwingWeapon(Transform player, float multiplier = 1)
    {
        return base.SwingWeapon(player, multiplier);
    }
}