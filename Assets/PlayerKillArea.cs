using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerKillArea : MonoBehaviour
{
    public SpriteRenderer areaSprite;
    public Color inside, outside;
    public float colorLerp = 0.25f;
    public UnityEvent OnKill;

    private Color targetColor;
    private bool inArea;

    private void Update()
    {
        targetColor = Color.Lerp(targetColor, inArea ? inside : outside, colorLerp);
        
        if (areaSprite) areaSprite.color = targetColor;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out PlayerStatus e))
        {
            LevelStats.main.EnemyKilled.AddListener(KillEvent);
            inArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerStatus e))
        {
            LevelStats.main.EnemyKilled.RemoveListener(KillEvent);
            inArea = false;
        }
    }

    void KillEvent() => OnKill?.Invoke();
}
