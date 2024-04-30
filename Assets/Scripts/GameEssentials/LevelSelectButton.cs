using System;
using System.Collections;
using System.Collections.Generic;
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
        menuBox.SetLevel(level);
        menuBox.ShowLevel();
    }
}
