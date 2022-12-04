using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*things to do
 * 
 * call this class if the specified shop button is pressed
 * create inventory pass function
 * create a ui script for a situation: if the weapon is already obtained/bought
 * 
 */


public class ShopManager : MonoBehaviour                    //call this class if the specified shop button is pressed
{
    public int totalCoins = 0;
    public TMP_Text coinUI;
    public Gun[] shopItemsSO;                               //contains all weapon scriptable objects.
    public GameObject[] shopPanelsGO;                       //contains all loaded product template included in the shop.
    public ShopTemplate[] shopPanels;                       //contains all product template included in the shop.
    public Button[] myPurchaseBins;                         //identifier for PurchaseItem() which weapon is being purchased (done through a btn)

    void Start()
    {
        for (int i =  0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }

        totalCoins = GameManager.main.pData.fPoints;
        coinUI.text = "FP: " + totalCoins;
        //Debug.Log("Score: " + GameManager.main.score.ToString());
        //Debug.Log("Coins: " + totalCoins);

        LoadPanels();
        CheckPurchasable();
    }

    public void CheckPurchasable()
    {
        //Debug.Log("CheckPurchasable passed");
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (totalCoins >= shopItemsSO[i].weaponCost && shopItemsSO[i].isWeaponObtained == false)
                myPurchaseBins[i].interactable = true;
            else
                myPurchaseBins[i].interactable = false;
        }
    }

    // TO DO: Create a function that adds purchased item to inventory, making that item unavailable for purchase.


    public void PurchaseItem(int btnNo)
    {
        totalCoins = GameManager.main.pData.fPoints;
        if (totalCoins >= shopItemsSO[btnNo].weaponCost)
        {
            //GameManager.SetScore(totalCoins - shopItemsSO[btnNo].weaponCost);   //problematic
            totalCoins = GameManager.main.pData.fPoints; // I had to restructure the game manager a bit so i edited this
            coinUI.text = "FP: " + totalCoins;

            shopItemsSO[btnNo].isWeaponObtained = true;
            shopPanels[btnNo].itemCostTxt.text = "";
            shopPanels[btnNo].itemPurchaseTxt.text = "Obtained";

            //Debug.Log("Cost: " + shopItemsSO[btnNo].weaponCost.ToString());
            //Debug.Log("Coins left: " + totalCoins.ToString());
            //Debug.Log("Score Left: " + GameManager.main.score.ToString());
            //Debug.Log("PurchaseItem passed");

            CheckPurchasable();
        }
    }

    public void LoadPanels()                                //same integration with void Start(),, just mix the two or not
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopItemsSO[i].isWeaponObtained = false;
            shopPanels[i].itemNameTxt.text = shopItemsSO[i].weaponName;
            shopPanels[i].itemDescriptionTxt.text = shopItemsSO[i].weaponDescription;
            shopPanels[i].itemCostTxt.text = shopItemsSO[i].weaponCost.ToString() + " FP";
        }
    }
}
