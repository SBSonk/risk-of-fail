using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    public float openDelay = 0.5f;
    public float closeDelay = 0f;

    public bool opened;
    private Animator anim;

    public AudioSource openSFX, closeSFX;
    //Collider2D col;

    private void Awake()
    {
        //col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (opened)
        {
            anim.Play("OpenDoor", 0, 0);
        }
        else
        {
            anim.Play("CloseDoor", 0, 0);
        }
    }

    public void ToggleDoor(bool val)
    {
        float delay = val ? closeDelay : openDelay;
        if (closeDelay < 0.1f) delay = 0.1f;

        StartCoroutine(Toggle(val, delay));
    }
    
    [ContextMenu("ToggleDoor")]
    public void ToggleDoor()
    {
        bool val = !opened;
        
        float delay = val ? closeDelay : openDelay;
        if (closeDelay < 0.1f) delay = 0.1f;

        StartCoroutine(Toggle(val, delay));
    }

    protected virtual IEnumerator Toggle(bool val, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        opened = val;

        if (opened)
        {
            anim.Play("OpenDoor", 0, 0);
            openSFX.Play();
        }
        else
        {
            anim.Play("CloseDoor", 0, 0);
            yield return new WaitForSeconds(.3f);
            closeSFX.Play();
        }
        
        //col.enabled = !opened;

        //AstarPath.active.UpdateGraphs(col.bounds, 1);
    }
}
