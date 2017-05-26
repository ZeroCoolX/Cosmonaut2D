using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;     //Apply parallax to these
    private float[] parallaxScales;     //porportion of how much to move the backgrounds by
    public float smoothing = 1f;        //how smooth the parallax is going to be

    private Transform cam;              //ref to main camera tranform
    private Vector3 previousCamPos;     //store the position of the camera in the previous frame


    //Called before Start()
    private void Awake() {
        //setup the cam reference
        cam = Camera.main.transform;
    }

	// Use this for initialization
	void Start () {
        //Store previous frame
        previousCamPos = cam.position;

        //for each background assign that background pos to the corresponding parallax scale
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; ++i) {
            parallaxScales[i] = backgrounds[i].position.z * -1;//don't worry about this....sneaky Brackeys
        }
	}
	
	// Update is called once per frame
	void Update () {
		//for each background
        for(int i = 0; i < backgrounds.Length; ++i) {
            //parallax value is the opposite of the camera movement * scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            //set a target x position which is the current pos + parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create target position which is the backgrounds current pos with it's target X pos
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current and target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);//Time.deltaTime converts frames to seconds
        }

        //set previous cam position
        previousCamPos = cam.position;
	}
}
