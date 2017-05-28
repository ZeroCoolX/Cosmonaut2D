using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {
    [SerializeField]
    private Text healthText;
    //max health 
    [SerializeField]
    private float healthMultiplier = 0.1f;
    public string formatHealthPercent { get { return ((int)(healthMultiplier * 100) + "%"); } }

    //how manu units the health increases per rate
    [SerializeField]
    private int regenerationIncrementor = 1;
    public string formatRegenIncrementor { get { return (regenerationIncrementor + " per rate"); } }
    [SerializeField]
    private Text regenerationText;

    //how many weapons can the user hold
    [SerializeField]
    private int weaponCapacity = 1;
    [SerializeField]
    private Text weaponCapacityText;

    private PlayerStats stats;

    [SerializeField]
    private int upgradeCost = 50;


    private void UpdateValues() {
        healthText.text = "INCREASE HEALTH: +" + formatHealthPercent.ToString() + "\nHEALTH: " + stats.maxHealth.ToString();
        regenerationText.text = "INCREASE REGEN: +" + stats.healthRegenValue.ToString() + "\nREGENERATION: " + stats.healthRegenValue.ToString();
        weaponCapacityText.text = "INCREASE WEAPONS: +" + stats.weaponCapacity.ToString() + "\nWEAPONS: " + stats.weaponCapacity.ToString();
    }

    private void OnEnable() {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    private bool currencyCheck(int price) {
        return (GameMaster.currency >= price);
    }

    public void upgradeRegeneration() {// TODO: make this price specific to upgrade
        if (!currencyCheck(upgradeCost)) {
            AudioManager.instance.playSound("NoMoney");
            return;
        }
        //upgrade the player stats
        stats.healthRegenValue += regenerationIncrementor;
        //Double the next upgrade value
        regenerationIncrementor *= 2;

        GameMaster.currency -= upgradeCost;
        AudioManager.instance.playSound("Bonus");

        UpdateValues();
    }

    public void upgradeHealth() {// TODO: make this price specific to upgrade
        if (!currencyCheck(upgradeCost)) {
            AudioManager.instance.playSound("NoMoney");
            return;
        }
        stats.maxHealth += (int)(stats.maxHealth * (1 + healthMultiplier));
        //increase next upgrade
        healthMultiplier += 0.1f;

        GameMaster.currency -= upgradeCost;
        AudioManager.instance.playSound("Bonus");

        UpdateValues();
    }

    public void upgradeWeaponCapacity() {
        if (!currencyCheck(upgradeCost)) {
            AudioManager.instance.playSound("NoMoney");
            return;
        }
        stats.weaponCapacity += weaponCapacity;

        GameMaster.currency -= upgradeCost;
        AudioManager.instance.playSound("Bonus");

        UpdateValues();
    }

}
