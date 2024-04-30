using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public Weapon[] weapons;
    public Transform parent;
    public Button prefabTemplate;

    public LoadoutSelectManager loadoutSelector;

    private void Start()
    {
        GenerateWeapons();
    }

    void GenerateWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            var slot = Instantiate(prefabTemplate, parent.position, quaternion.identity, parent);
            var weapon = weapons[i];
            slot.transform.GetChild(0).GetComponent<Image>().sprite = weapon.hud.sprite;
            
            slot.onClick.AddListener(() => {  loadoutSelector.SetWeapon(weapon);});
        }
        
        gameObject.SetActive(false);
        Destroy(prefabTemplate.gameObject);
    }
}
