using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;
    private float shakeAmount = 0f;

    void Awake() {
        if(mainCam == null) {
            mainCam = Camera.main;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            shake(0.05f, 0.1f);
        }
    }

    public void shake(float amt, float length) {
        shakeAmount = amt;
        InvokeRepeating("doShake", 0, 0.01f);//repeat the method, delayed this many seconds, every this second time
        Invoke("stopShake", length);
    }
    
    void doShake() {
        if(shakeAmount > 0) {
            Vector3 camPos = mainCam.transform.position;

            //Get shake values
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount; //calculation just found online. (randval is [0,1])
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount; //calculation just found online. (randval is [0,1])

            //apply shake
            camPos.x += offsetX;
            camPos.y += offsetY;
            //move main camera
            mainCam.transform.position = camPos;
        }
    }

    void stopShake() {

        CancelInvoke("doShake");
        mainCam.transform.localPosition = Vector3.zero; // this will make the main camera reset to its parent cam which is following the player
    }
}
