using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]//ensures in unity we can see the classs
    public class PlayerStats {
        public int maxHealth = 100;
        public int health = 100;
    }

    public PlayerStats playerStats = new PlayerStats();
    public int fallBoundary = -20;

    //take damage
    public void damageHealth(int dmg) {
        playerStats.health -= dmg;
        lifeCheck();
    }

    //check if health is exhausted - if so kill em
    private void lifeCheck() {
        if(playerStats.health <= 0) {
            GameMaster.killPlayer(this);
        }
    }

    void Update() {
        if(transform.position.y <= fallBoundary) {
            damageHealth(playerStats.maxHealth*9);//arbitrary massive num to ensure death
        }      
    }
}
