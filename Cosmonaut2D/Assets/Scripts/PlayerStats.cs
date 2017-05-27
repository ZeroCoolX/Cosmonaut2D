using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;

    public int maxHealth = 100;
    private int _curHealth;
    public int curHealth {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    public float healthRegenRate = 2f;

    public float movementSpeed = 10f;                    // The fastest the player can travel in the x axis.


    void Awake() {
        if (instance == null) {
            instance = this;
        }
        curHealth = maxHealth;
    }
}
