using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

    //stats reference like helath, speed...etc
    private PlayerStats stats;

    //health bar
    [SerializeField]
    private StatusIndicator statusIndicator;

    //-Y death point
    public int fallBoundary = -20;

    //set audio data
    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "GruntVoice02";
    private AudioManager audioManager;

    //weapon references to enable/disable based on player choice
    [SerializeField]    //TODO: only allow weapon switching if they HAVE the weapon - which they can get from the upgrade menu
    private GameObject machineGun; // 2
    [SerializeField]
    private GameObject pistol; // 1

    void Start() {
        //set player stats
        stats = PlayerStats.instance;
        stats.curHealth = stats.maxHealth;
        if (statusIndicator == null) {
            Debug.Log("Player.cs - No status indicator found");
        }else {
            statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
        }

        //select weapon
        selectWeapon(GameMaster.gm.weaponChoice);

        //set audiomanager
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("Player.cs - No Audio manager found");
        }

        // adding the menthod onto the GameMaster delegate to be called when its invoked. - used for menu opening
        GameMaster.gm.onToggleUpgradeMenu += onUpgradeMenuToggle;
        //Add the weapon switch method onto delegate
        GameMaster.gm.onWeaponSwitch += selectWeapon;

        //regen health over time
        InvokeRepeating("regenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
    }


    //set weapon
    private void selectWeapon(int choice) {
        //only allow weapon switching if they HAVE the weapon (for now)
        if (choice <= stats.weaponCapacity) {
            //if either of these are null that means the user is trying to switch weapons while they're dead - which I just won't allow
            if (machineGun != null && pistol != null) {
                machineGun.SetActive(false);
                pistol.SetActive(false);
                //set player weapon choice
                switch (choice) {
                    case 1://pistol selected
                        pistol.SetActive(true);
                        break;
                    case 2://machine gun selcted
                        machineGun.SetActive(true);
                        break;
                    default://default to pistol
                        pistol.SetActive(true);
                        break;
                }
            }
        }
    }

    //take damage
    public void damageHealth(int dmg) {
        //damage health and update health bar visual
        stats.curHealth -= dmg;
        if (statusIndicator != null) {
            statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
        }
        //Clear! *BZZZzzz*  CLEAR! *BZZZZZzz*
        lifeCheck();
    }

    //check if health is exhausted - if so kill em
    private void lifeCheck() {
        if(stats.curHealth <= 0) {
            //Play sound effect
            audioManager.playSound(deathSoundName);
            GameMaster.killPlayer(this);
        }else {
            //not dead, just hurt
            audioManager.playSound(damageSoundName);
        }
    }

    //regenerate health over time
    void regenHealth() {
        stats.curHealth += stats.healthRegenValue;
        //update visual health bar
        statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
    }

    void Update() {
        //just checking for death by falling
        if(transform.position.y <= fallBoundary) {
            damageHealth(stats.maxHealth*9);//arbitrary massive num to ensure death
        }      
    }

    //called when the upgrade menu is toggled
    void onUpgradeMenuToggle(bool activeState) {
        //disable player movement controls
        if (this != null) {
            GetComponent<Platformer2DUserControl>().enabled = !activeState;
        }
        //Disable weapon controls
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if(_weapon != null) {
            _weapon.enabled = !activeState;
        }
    }

    //release delegate methods
    private void OnDestroy() {
        GameMaster.gm.onToggleUpgradeMenu -= onUpgradeMenuToggle;
    }
}
