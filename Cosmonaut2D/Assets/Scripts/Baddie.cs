using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour {

    [System.Serializable]//ensures in unity we can see the classs
    public class BaddieStats {
        public int maxHealth = 100;

        //actual value
        private int _curHealth;
        //accessor and mutator
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;
 
        public void Init() {
            curHealth = maxHealth;
        }
    }

    public BaddieStats stats = new BaddieStats();

    public Transform deathParticles;
    public string deathSoundName = "Explosion";

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    //for death camera shake
    public float shakeAmt = 0.05f;
    public float shakeLength = 0.1f;

    void Start() {
        stats.Init();
        if(statusIndicator != null) {
            statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
        }

        if(deathParticles == null) {
            Debug.LogError("no death particles in baddie");
        }
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
        if (stats.curHealth <= 0) {
            GameMaster.killBaddie(this);
        }
    }

    //everytime this object collides with another object this method is called
    private void OnCollisionEnter2D(Collision2D _colInfo) {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null) {
            _player.damageHealth(stats.damage);
            damageHealth(stats.maxHealth * 9);//combust the enemy
        }
    }

}
