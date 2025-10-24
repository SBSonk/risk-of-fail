using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public SpriteRenderer sprite { get; private set; }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();

        PlayEquipAnimation();
    }

    public virtual void PlayEquipAnimation()
    {
        if (!anim.HasState(0, Animator.StringToHash("Equip"))) return;
        anim.CrossFade("Equip", .1f, 0, 0f);
    }

    public virtual void PlayShootAnimation()
    {
        anim.CrossFade("Shoot", .1f, 0, 0f);
    }

    public virtual void PlayReloadAnimation()
    {
        if (!anim.HasState(0, Animator.StringToHash("Reload"))) return;
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