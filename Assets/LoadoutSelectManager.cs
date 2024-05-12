using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSelectManager : MonoBehaviour
{
    public static LoadoutSelectManager instance;

    public bool isSelectingLoadout;
    
    public LoadoutContainer[] loadoutContainers;

    public Button startButton;

    public int selIndex;

    public void IsSelectingLoadout(bool b) => isSelectingLoadout = b;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < loadoutContainers.Length; i++)
        {
            UpdateContainer(i);
        }
    }

    public void SetWeapon(Weapon w)
    {
        loadoutContainers[selIndex].selected = w;
        
        UpdateContainer(selIndex);
    }

    public void ApplySelection()
    {
        for (int i = 0; i < loadoutContainers.Length; i++)
        {
            LoadoutManager.instance.AssignSlot(i, loadoutContainers[i].selected);
        }
    }
    
    void UpdateContainer(int index)
    {
        var container = loadoutContainers[index];
        var wep = container.selected;

        container.sprite.sprite = wep.hud.sprite;
        container.name.SetText(wep.name);
        container.description.SetText(wep.shopData.weaponDescription);
    }

    public void SetSelection(int i)
    {
        selIndex = i;
    }

    [System.Serializable]
    public struct LoadoutContainer
    {
        public Weapon selected;
        public Image sprite;
        public TextMeshProUGUI name, description;
        public Image strength, firerate, range, spread, piercing;
    }
}
