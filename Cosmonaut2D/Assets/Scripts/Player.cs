using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {

    private PlayerStats stats;

    [SerializeField]
    private StatusIndicator statusIndicator;

    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "GruntVoice02";

    private AudioManager audioManager;

    void Start() {
        stats = PlayerStats.instance;

        stats.curHealth = stats.maxHealth;

        if (statusIndicator == null) {
            Debug.Log("No status indicator found on player!");
        }else {
            statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
        }

        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("Ther was a problem getting the audiop manger");
        }

        GameMaster.gm.onToggleUpgradeMenu += onUpgradeMenuToggle;// adding the menthod onto the delegate to be called when its invoked. wow.

        //regen health over time
        InvokeRepeating("regenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
    }

    //take damage
    public void damageHealth(int dmg) {

        stats.curHealth -= dmg;
        if (statusIndicator != null) {
            statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
        }
        lifeCheck();
    }

    //check if health is exhausted - if so kill em
    private void lifeCheck() {
        if(stats.curHealth <= 0) {
            //Play sound effect
            audioManager.playSound(deathSoundName);
            GameMaster.killPlayer(this);
        }else {
            audioManager.playSound(damageSoundName);
        }
    }

    //regenerate health over time
    void regenHealth() {
        stats.curHealth += 1;
        statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
    }

    void Update() {
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

    private void OnDestroy() {
        GameMaster.gm.onToggleUpgradeMenu -= onUpgradeMenuToggle;
    }
}
