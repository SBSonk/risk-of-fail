using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] ParticleSystem weaponParticles;

    public void EnableParticles()
    {
        weaponParticles.Play();
    }

    public void DisableParticles()
    {
        weaponParticles.Stop();
    }
}
