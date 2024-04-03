using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] public SpriteRenderer sprite { get; private set; }
    [SerializeField] private Animator anim;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();    
        anim = GetComponent<Animator>();
        
        PlayEquipAnimation();
    }

    public virtual void PlayEquipAnimation()
    {
        anim.CrossFade("Equip", .1f, 0, 0f);
    }
    
    public virtual void PlayShootAnimation()
    {
        anim.CrossFade("Shoot", .1f, 0, 0f);
    }

    public virtual void PlayReloadAnimation()
    {
        anim.CrossFade("Reload", .1f, 0, 0f);
    }

    
    public virtual void PlayShoveAnimation()
    {
        anim.CrossFade("Shove", .1f, 0, 0);
    }

    public virtual void PlayHoldAnimation()
    {
        anim.CrossFade("Hold", .1f, 0, 0f);
    }
    
}
