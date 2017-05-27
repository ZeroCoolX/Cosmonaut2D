using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]   //allows us to change the values in unity 
    public class Wave {
        public string name;
        public Transform baddie;
        public int count;
        public float rate;
    }

    public Wave[] waves;

    private int nextWave = 0; //index in waves up next
    public int NextWave {
        get { return nextWave; }
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f; //in seconds
    private float waveCountdown;
    public float WaveCountdown {
        get { return waveCountdown; }
    }

    private float baddieSearchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State {
        get { return state; }
    }

    void Start() {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0) {
            Debug.LogError("No spawn Points...BAD");
        }
    }

    void Update() {

        if(state == SpawnState.WAITING) {
            //check if player killed everyone
            if (!baddieIsAlive()) {
                //Begin the next wave
                waveCompleted();
            }else {
                return; //baddies still exist. KILL THEM ALL
            }
        }

        if(waveCountdown <= 0) {
            if(state != SpawnState.SPAWNING) {
                StartCoroutine(spawnWave(waves[nextWave]));
            }
        }else {
            waveCountdown -= Time.deltaTime;//decrement once a second
        }
    }

    bool baddieIsAlive() {
        baddieSearchCountdown -= Time.deltaTime;
        if (baddieSearchCountdown <= 0f) {
            baddieSearchCountdown = 1f;
            Debug.Log("checking baddie is alive");
            return (GameObject.FindGameObjectWithTag("BADDIE") != null);  //if something not null is returned there is a baddie somewhre
        }
        return true;
    }

    void waveCompleted() {
        Debug.Log("Wave Completed!");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1) {
            nextWave = 0;
            Debug.Log("All Waves Completed - Looping");
        }else {
            ++nextWave;
        }
    }

    IEnumerator spawnWave(Wave _wave) {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; ++i) {
            spawnBaddie(_wave.baddie);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void spawnBaddie(Transform _baddie) {
        //spawn enemy
        Debug.Log("Spawning Baddie: " + _baddie.name);
        if(spawnPoints.Length == 0) {
            Debug.LogError("No spawn Points...BAD");
        }
        //Choose a random spawn point
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];//max is exclusive
        Instantiate(_baddie, _sp.position, _sp.rotation);
    }

}
