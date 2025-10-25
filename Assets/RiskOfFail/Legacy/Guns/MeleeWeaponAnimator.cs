using UnityEngine;

public class MeleeWeaponAnimator : WeaponAnimator
{
    [SerializeField] private ParticleSystem swingEffects;

    public void EnableParticles()
    {
        swingEffects.Play();
    }

    public void DisableParticles()
    {
        swingEffects.Stop();
    }
}