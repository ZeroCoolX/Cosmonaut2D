using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    //cache
    private AudioManager audioManager;

    [SerializeField]
    string mouseHoverSoundName = "ButtonHover";

    [SerializeField]
    string buttonPressSoundName = "ButtonPress";

    void Start() {
        //cache
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("FREAK OUT - no audioManager found");
        }
    }

    public void OnMouseOver() {
        audioManager.playSound(mouseHoverSoundName);
    }

    public void quit() {
        audioManager.playSound(buttonPressSoundName);
        Debug.Log("APPLICATION QUIT");
        Application.Quit();
    }

    public void retry() {
        audioManager.playSound(buttonPressSoundName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
