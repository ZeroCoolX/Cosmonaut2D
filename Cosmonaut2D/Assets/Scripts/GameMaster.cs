using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    //singleton instance for other scripts to access this
    public static GameMaster gm;

    //Health data
    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int remainingLives {
        get { return _remainingLives; }
    }

    //Currency data
    [SerializeField]
    private int startingCurrency;
    public static int currency;

    //Cache prefabs or objects we're going to enable/disable
    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject upgradeMenu;

    //Cache script references
    public CameraShake camShake;
    [SerializeField]
    private WaveSpawner waveSpawner;
    private AudioManager audioManager;

    //Set audio
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    //delay for player spawn when they die
    public float spawnDelay = 3f;

    //weapon choice 1, 2..etc default to 1 (pistol)
    [SerializeField]
    private int _weaponChoice = 1;
    public int weaponChoice { get{ return _weaponChoice; } }

    //a way for us to create a type that stores a bunch of references to function. Invoking delegate calls functions registered to delegate
    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    //Delegate for weapon switching
    public delegate void WeaponSwitchCallback(int choice);
    public WeaponSwitchCallback onWeaponSwitch;


    //store instance 
    void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    void Start() {
        if(camShake == null) {
            Debug.LogError("GameMaster.cs - No camShake found");
        }
        //set lives
        _remainingLives = maxLives;

        //set currency
        currency = startingCurrency;

        //cache
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("GameMaster.cs - no AudioManager found");
        }
    }

    //TODO: change into a switch for the key pressed
    private void Update() {
        //check for the menu hotkey
        if (Input.GetKeyDown(KeyCode.U)) {
            toggleUpgradeMenu();
        }else if (Input.GetKeyDown(KeyCode.Alpha1)) {//check for weapon switch hotkey
            //user pressed 1 = Pistol weapon choice
            _weaponChoice = 1;
            onWeaponSwitch.Invoke(weaponChoice);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            //user pressed 2 = machine gun weapon choice
            _weaponChoice = 2;
            onWeaponSwitch.Invoke(weaponChoice);
        }
    }

    //enable/disable the upgrade menu which will pause/continue objects in the game based off which action (enable/disable) is occuring
    private void toggleUpgradeMenu() {
        //toggle manu
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        //toggle baddie wave spawner;
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        //Invoke delegate so oither scripts realize the menu toggle occured
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    //Game over
    public void endGame() {
        audioManager.playSound(gameOverSoundName);
        Debug.Log("GAME OVER");
        //enable game over ut
        gameOverUI.SetActive(true);
    }

    public IEnumerator respawnPlayer() {
        if (_remainingLives <= 0) {
            gm.endGame();
        } else {
            //play sound and wait for delay
            audioManager.playSound(respawnCountdownSoundName);
            yield return new WaitForSeconds(spawnDelay);

            //spawn sound
            audioManager.playSound(spawnSoundName);

            //Generate player
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            //Generate spawn particles
            Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            Destroy(clone.gameObject, 1.5f);
        }
    }

    public static void killBaddie(Baddie baddie) {
        //Play sound effects
        gm.audioManager.playSound(baddie.deathSoundName);

        //TODO: change this so it doesn't happen everytime instead only when a player shot it to death
        //reward player for the kill
        currency += baddie.currencyDrop;
        //play currency pickup sound
        gm.audioManager.playSound("Bonus");

        //Generate death particles
        Transform clone = Instantiate(baddie.deathParticles, baddie.transform.position, Quaternion.identity) as Transform;
        Destroy(clone.gameObject, 3f);

        //generate Camera shake
        gm.camShake.shake(baddie.shakeAmt, baddie.shakeLength);

        //actually kill the baddie
        gm.killDashNine(baddie.gameObject, false);
    }

    public static void killPlayer(Player player) {
        //decrement lives
        --_remainingLives;
        //actually kill the player
        gm.killDashNine(player.gameObject, true);
    }

    //Actual destruction of the object with optional respawning of the player
    private void killDashNine(GameObject obj, bool respawn) {
        Destroy(obj);
        if (respawn) {
            gm.StartCoroutine(gm.respawnPlayer());
        }
    }
}
