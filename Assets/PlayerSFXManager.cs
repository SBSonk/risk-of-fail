using GameAudioScriptingEssentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    public AudioClipRandomizer shoot, shove, reload;
    public AudioClipRandomizer dash;

    public int volume = 1;

    public void Initialize(PlayerShooting shooting, PlayerMovement movement)
    {
        shooting.OnShoot.AddListener(PlayShootSound);
        shooting.OnMelee.AddListener(PlayShootSound);
        shooting.OnShove.AddListener(PlayShoveSound);
        shooting.OnReloadStart.AddListener(PlayReloadSound);
        shooting.OnWeaponSwitch.AddListener(ChangeSFXProfile);

        movement.OnDodge.AddListener(PlayDashSound);

        ChangeSFXProfile(shooting.GetHeldWeapon());
    }

    void PlayShootSound()
    {
        if (!shoot.HasAudioClips())
        {
            print("No shooting audio found...");
            return;
        }

        shoot.SFXVolume = volume; // TODO: Hookup to volume variable when that exists + Add sound mixing
        shoot.PlaySFX();
    }

    void PlayShoveSound()
    {
        if (!shove.HasAudioClips())
        {
            print("No shoving audio found...");
            return;
        }

        shove.SFXVolume = volume;
        shove.PlaySFX();
    }

    void PlayReloadSound(float _)
    {
        if (!reload.HasAudioClips())
        {
            print("No reloading audio found...");
            return;
        }

        reload.SFXVolume = volume;
        reload.PlaySFX();
    }

    void PlayDashSound()
    {
        if (!dash.HasAudioClips())
        {
            print("No dash audio found...");
            return;
        }

        dash.SFXVolume = volume;
        dash.PlaySFX();
    }

    public void ChangeSFXProfile(InventoryWeapon w)
    {
        shoot.SetAudioClips(w.weapon.effects.shootSounds);
        shove.SetAudioClips(w.weapon.effects.shoveSounds);
        reload.SetAudioClips(w.weapon.effects.reloadSounds);
    }
}
