using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    //singleton
    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int remainingLives {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingCurrency;
    public static int currency;

    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";

    public string gameOverSoundName = "GameOver";

    public float spawnDelay = 3f;

    public CameraShake camShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    //a way for us to create a type that stores a bunch of references to functino. Invoking delegate calls functions registered to delegate
    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    //cache
    private AudioManager audioManager;

    void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    void Start() {
        if(camShake == null) {
            Debug.LogError("no camshake in game master!");
        }
        //set lives
        _remainingLives = maxLives;

        //set currency
        currency = startingCurrency;

        //cache
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("FREAK OUT - no audioManager found");
        }
    }

    private void Update() {//always check for the menu hotkey
        if (Input.GetKeyDown(KeyCode.U)) {
            toggleUpgradeMenu();
        }
    }

    private void toggleUpgradeMenu() {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;//disable wave spawner
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void endGame() {
        audioManager.playSound(gameOverSoundName);

        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public IEnumerator respawnPlayer() {
        if (_remainingLives <= 0) {
            gm.endGame();
        } else {
            audioManager.playSound(respawnCountdownSoundName);
            yield return new WaitForSeconds(spawnDelay);

            audioManager.playSound(spawnSoundName);

            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
            Destroy(clone.gameObject, 1.5f);
        }
    }

    public static void killBaddie(Baddie baddie) {
        //Play sound effects
        gm.audioManager.playSound(baddie.deathSoundName);

        //reward player for the kill
        currency += baddie.currencyDrop;
        gm.audioManager.playSound("Bonus");

        //Add particles
        Transform clone = Instantiate(baddie.deathParticles, baddie.transform.position, Quaternion.identity) as Transform;
        Destroy(clone.gameObject, 3f);

        //Camera shake
        gm.camShake.shake(baddie.shakeAmt, baddie.shakeLength);
        gm.killDashNine(baddie.gameObject, false);
    }

    public static void killPlayer(Player player) {
        --_remainingLives;
        gm.killDashNine(player.gameObject, true);
    }

    private void killDashNine(GameObject obj, bool respawn) {
        Destroy(obj);
        if (respawn) {
            gm.StartCoroutine(gm.respawnPlayer());
        }
    }
}
