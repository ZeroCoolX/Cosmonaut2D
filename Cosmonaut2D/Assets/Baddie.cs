using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour {

    [System.Serializable]//ensures in unity we can see the classs
    public class BaddieStats {
        public int maxHealth = 100;
        public int health = 100;
    }

    public BaddieStats stats = new BaddieStats();

    //take damage
    public void damageHealth(int dmg) {
        stats.health -= dmg;
        lifeCheck();
    }

    //check if health is exhausted - if so kill em
    private void lifeCheck() {
        if (stats.health <= 0) {
            GameMaster.killBaddie(this);
        }
    }

}
