using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    //singleton
    public static GameMaster gm;

    void Start() {
        if(gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;

    public IEnumerator respawnPlayer() {
        //TODO tell user we're respawning
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        //TODO add spawn particles
    }

    public static void killDashNine(Player player) {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.respawnPlayer());
    }
}
