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

    public float healthRegenRate = 2f;  //how many times per second regeneration occurs
    public int healthRegenValue = 1; //how MUCH regeneration occurs each instance of a regen - can also be upgraded
    public float movementSpeed = 10f; // The fastest the player can travel in the x axis.
    public int weaponCapacity = 1;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
        curHealth = maxHealth;
    }
}
