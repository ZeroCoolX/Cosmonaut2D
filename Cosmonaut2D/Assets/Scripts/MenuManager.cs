using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    public string hoverOverSound = "ButtonHover";

    [SerializeField]
    public string pressButtonSound = "ButtonPress";

    AudioManager audioManager;

    void Start() {
        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("No audio manager found");
        }
    }

    public void startGame() {
        audioManager.playSound(pressButtonSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGame() {
        audioManager.playSound(pressButtonSound);
        Debug.Log("WE QUIT THE GAME");
        Application.Quit();
    }

    public void OnMouseOver() {
        audioManager.playSound(hoverOverSound);
    }
}
