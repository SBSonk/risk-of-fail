using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Menu_Infobox menuBox;
    [SerializeField] private LevelSelectItem level;
    [SerializeField] private TextMeshProUGUI name;
    public RectTransform selectorSprite;
    
    void Start()
    {
        name.SetText(level.name);
   
        LoadoutSelectManager.instance.startButton.onClick.RemoveAllListeners();
        LoadoutSelectManager.instance.startButton.onClick.AddListener(() =>
        {
            LevelFade.FadeIn(() =>
            {
                if (!level.followLoadout)
                {
                    Destroy(LoadoutManager.instance.gameObject);
                }
            
                SceneManager.LoadScene(level.buildIndex);
                LevelFade.FadeOut();
            });
        });
    }

    public void MouseEnter(BaseEventData b)
    {
        SelectLevel(.1f);
    }

    public void SelectLevel(float t)
    {
        menuBox.SetLevel(level);
        menuBox.ShowLevel();

        selectorSprite.DOMove(transform.position, t);
    }

}
