using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] PlayerStatus player;
    [SerializeField] GameObject reloadingText;
    [SerializeField] TextMeshProUGUI gunText, healthText, pointsText;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (player.pShooting.reloading) reloadingText.SetActive(true);
        else reloadingText.SetActive(false);

        // worlds longest f string
        gunText.text = $"{player.pShooting.weaponPool[player.pShooting.currentWeaponIndex].weapon.weaponName}:" +
            $" {player.pShooting.weaponPool[player.pShooting.currentWeaponIndex].clip}/" +
            $"{player.pShooting.weaponPool[player.pShooting.currentWeaponIndex].pool}";

        healthText.text = $"{System.Math.Round(player.health, 2)} / {(int) player.maxHealth}";
        pointsText.text = GameManager.main.score.ToString();
    }
}
