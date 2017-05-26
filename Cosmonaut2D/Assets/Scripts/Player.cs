﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [System.Serializable]//ensures in unity we can see the classs
    public class PlayerStats {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();

    [SerializeField]
    private StatusIndicator statusIndicator;

    public int fallBoundary = -20;


    void Start() {
        stats.Init();

        if(statusIndicator == null) {
            Debug.Log("No status indicator found on player!");
        }else {
            statusIndicator.setHealth(stats.curHealth, stats.maxHealth);
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
        if(stats.curHealth <= 0) {
            GameMaster.killPlayer(this);
        }
    }

    void Update() {
        if(transform.position.y <= fallBoundary) {
            damageHealth(stats.maxHealth*9);//arbitrary massive num to ensure death
        }      
    }
}
