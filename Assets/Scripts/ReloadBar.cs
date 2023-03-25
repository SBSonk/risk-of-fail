using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] PlayerShooting shooting;

    [SerializeField] GameObject reloadParent;
    [SerializeField] Transform pivot;

    SpriteRenderer[] sprites;
    Color[] spriteColors;

    private void Start()
    {
        sprites = reloadParent.transform.GetComponentsInChildren<SpriteRenderer>();
        spriteColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteColors[i] = sprites[i].color;
        }

        shooting.OnReloadStart.AddListener(StartAnim);
        shooting.OnWeaponSwitch.AddListener(HideBar);
    }

    void StartAnim(float time)
    {
        StopAllCoroutines();
        StartCoroutine(ReloadBarAnim(time));
    }

    void HideBar(InventoryWeapon _)
    {
        reloadParent.SetActive(false);
    }

    public IEnumerator ReloadBarAnim(float time)
    {
        // Fade in
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = spriteColors[i];
        }
        reloadParent.SetActive(true);

        pivot.localScale = new Vector3(0, 1, 1);

        float t = 0;
        while (t < time)
        {
            pivot.localScale = new Vector3(Mathf.Lerp(0, 1, t / time), 1, 1);
            t += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        pivot.localScale = Vector3.one;

        // Fade out
        for (int i = 0; i < sprites.Length; i++)
        {
            StartCoroutine(SprFunctions.Fade(sprites[i], sprites[i].color, Color.clear, 0.25f));
        }

        yield return new WaitForSeconds(0.25f);
        reloadParent.SetActive(false);
    }
}
