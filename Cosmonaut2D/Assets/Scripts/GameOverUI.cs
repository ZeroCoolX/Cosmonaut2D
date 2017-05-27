using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    public void quit() {
        Debug.Log("APPLICATION QUIT");
        Application.Quit();
    }

    public void retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
