using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public Weapon[] weapons;
    public Transform parent;
    public Button prefabTemplate;
    public int selIndex = 0;

    public LoadoutSelectManager loadoutSelector;

    private void Start()
    {
        DefaultWeapons();
        GenerateWeapons();
    }

    void DefaultWeapons()
    {
        PlayerPrefs.SetInt(weapons[0].name + "Acquired", 1);
    }
    
    void GenerateWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            var weapon = weapons[i];
            
            //#if !UNITY_EDITOR
            //    if (!HasWeapon(weapon.name)) continue;
            //#endif
            
            var slot = Instantiate(prefabTemplate, parent.position, quaternion.identity, parent);
            
            slot.transform.GetChild(0).GetComponent<Image>().sprite = weapon.hud.sprite;
            
            slot.onClick.AddListener(() => {  loadoutSelector.SetWeapon(selIndex, weapon);});
        }
        
        gameObject.SetActive(false);
        Destroy(prefabTemplate.gameObject);
    }

    public static void SetWeaponOwned(string weaponName)
    {
        PlayerPrefs.SetInt(weaponName + "Acquired", 1);
    }
    
    public static bool HasWeapon(string weaponName)
    {
        if (PlayerPrefs.GetInt(weaponName + "Acquired", 0) == 1)
        {
            return true;
        }

        return false;
    }
}