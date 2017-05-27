using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

    [SerializeField]
    WaveSpawner waveSpawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

	// Use this for initialization
	void Start () {
		if(waveSpawner == null) {
            Debug.LogError("No Spawner Referenced");
            this.enabled = false;
        }
        if (waveAnimator == null) {
            Debug.LogError("No waveAnimator Referenced");
            this.enabled = false;
        }
        if (waveCountdownText == null) {
            Debug.LogError("No waveCountdownText Referenced");
            this.enabled = false;
        }
        if (waveCountText == null) {
            Debug.LogError("No waveNumberText Referenced");
            this.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        switch (waveSpawner.State) {
            case WaveSpawner.SpawnState.COUNTING:
                updateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                updateSpawningUI();
                break;
        }
        previousState = waveSpawner.State;
	}

    void updateCountingUI() {
        if(previousState != WaveSpawner.SpawnState.COUNTING) {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
        }
        //update the countdown
        waveCountdownText.text = (((int)waveSpawner.WaveCountdown)+1).ToString();
    }
    void updateSpawningUI() {
        if (previousState != WaveSpawner.SpawnState.SPAWNING) {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);
            //update the wave
            waveCountText.text = (waveSpawner.NextWave+1).ToString();
        }
    }
}
