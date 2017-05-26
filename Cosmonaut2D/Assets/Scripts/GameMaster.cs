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

    public IEnumerator respawnPlayer() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
    }

    public static void killBaddie(Baddie baddie) {
        killDashNine(baddie.gameObject, false);
    }

    public static void killPlayer(Player player) {
        killDashNine(player.gameObject, true);
    }

    private static void killDashNine(GameObject obj, bool respawn) {
        Destroy(obj);
        if (respawn) {
            gm.StartCoroutine(gm.respawnPlayer());
        }
    }
}
