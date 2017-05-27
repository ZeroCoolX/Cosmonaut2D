using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LivesCounterUI : MonoBehaviour {

    [SerializeField]
    private Text livesText;

	// Use this for initialization
	void Awake () {
        livesText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        livesText.text = "LIVES: " + GameMaster.remainingLives.ToString();
	}
}
