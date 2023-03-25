using GameAudioScriptingEssentials;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSFXManager : NetworkBehaviour
{
    public AudioClipRandomizer shoot, shove, reload;
    public AudioClipRandomizer dash;

    public int volume = 1;

    public void Initialize(PlayerShooting shooting, PlayerMovement movement)
    {
        shooting.OnWeaponSwitch.AddListener(ChangeSFXProfile);

        movement.OnDodge.AddListener(PlayDashSound);

        ChangeSFXProfile(shooting.GetHeldWeapon());
    }

    public void PlayShootSound()
    {
        if (!IsOwner) return;
        
        if (!shoot.HasAudioClips())
        {
            print("No shooting audio found...");
            return;
        }

        shoot.SFXVolume = volume;
        shoot.PlaySFX();
    }

    public void PlayShoveSound()
    {
        if (!IsOwner) return;
        
        if (!shove.HasAudioClips())
        {
            print("No shoving audio found...");
            return;
        }

        shove.SFXVolume = volume;
        shove.PlaySFX();
    }

    public void PlayReloadSound()
    {
        if (!IsOwner) return;
        
        if (!reload.HasAudioClips())
        {
            print("No reloading audio found...");
            return;
        }

        reload.SFXVolume = volume;
        reload.PlaySFX();
    }

    public void PlayDashSound()
    {
        if (!IsOwner) return;
        
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
