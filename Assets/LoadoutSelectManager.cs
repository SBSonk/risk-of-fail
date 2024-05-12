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

    public GameObject slotASel, slotBSel;

    public void ToggleSlotA() => slotASel.SetActive(!slotASel.activeInHierarchy);
    public void ToggleSlotB() => slotBSel.SetActive(!slotBSel.activeInHierarchy);

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

    public void SetWeapon(int selIndex, Weapon w)
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

        if (wep.shopData.weaponStrength > 0) container.strength.fillAmount = wep.shopData.weaponStrength / 3f;
        else container.strength.fillAmount = 0;
        
        if (wep.shopData.weaponFireRate > 0) container.firerate.fillAmount = wep.shopData.weaponFireRate / 3f;
        else container.firerate.fillAmount = 0;
        
        if (wep.shopData.range > 0) container.range.fillAmount = wep.shopData.range / 3f;
        else container.range.fillAmount = 0;
        
        if (wep.shopData.spread > 0) container.spread.fillAmount = wep.shopData.spread / 3f;
        else container.spread.fillAmount = 0;
        
        if (wep.shopData.weaponPiercing > 0) container.piercing.fillAmount = wep.shopData.weaponPiercing / 3f;
        else container.piercing.fillAmount = 0;
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
