using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    //singleton
    public static GameMaster gm;

    void Awake() {
        if(gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab; 

    public float spawnDelay = 3f;

    public CameraShake camShake;

    void Start() {
        if(camShake == null) {
            Debug.LogError("no camshake in game master!");
        }
    }

    public IEnumerator respawnPlayer() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 1.5f);
    }

    public static void killBaddie(Baddie baddie) {
        Transform clone = Instantiate(baddie.deathParticles, baddie.transform.position, Quaternion.identity) as Transform;
        Destroy(clone.gameObject, 3f);
        gm.camShake.shake(baddie.shakeAmt, baddie.shakeLength);
        gm.killDashNine(baddie.gameObject, false);
    }

    public static void killPlayer(Player player) {
        gm.killDashNine(player.gameObject, true);
    }

    private void killDashNine(GameObject obj, bool respawn) {
        Destroy(obj);
        if (respawn) {
            gm.StartCoroutine(gm.respawnPlayer());
        }
    }
}
