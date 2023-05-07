using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Infobox : MonoBehaviour
{
    [SerializeField] private GameObject info, tutorial;
    
    [SerializeField] private Image levelPreview;
    [SerializeField] private TextMeshProUGUI levelDescription, highScore, bestTime;

    public void SetLevel(LevelSelectItem level)
    {
        levelDescription.SetText(level.description);
        levelPreview.sprite = level.preview;
    }

    public void ShowLevel()
    {
        info.SetActive(true);
        tutorial.SetActive(false);
    }

    public void ShowTutorial()
    {
        info.SetActive(false);
        tutorial.SetActive(true);
    }
}
