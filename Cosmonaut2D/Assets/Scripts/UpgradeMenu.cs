using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private float healthMultiplier = 1.2f;
    //TODO: tweak these numbers
    [SerializeField]
    private float speedMultiplier = 1.1f;

    private PlayerStats stats;

    [SerializeField]
    private int upgradeCost = 50;


    private void UpdateValues() {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
    }

    private void OnEnable() {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    public void upgradeSpeed() {// TODO: make this wayyyy better  - also this is happeneing doulbe i think
        if (GameMaster.currency < upgradeCost) {
            AudioManager.instance.playSound("NoMoney");
            return;
        }
        stats.movementSpeed = (int)(stats.movementSpeed * speedMultiplier);

        GameMaster.currency -= upgradeCost;
        AudioManager.instance.playSound("Bonus");

        UpdateValues();
    }

    public void upgradeHealth() {// TODO: make this wayyyy better
        if(GameMaster.currency < upgradeCost) {
            AudioManager.instance.playSound("NoMoney");
            return;
        }
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);

        GameMaster.currency -= upgradeCost;
        AudioManager.instance.playSound("Bonus");

        UpdateValues();
    }

}
