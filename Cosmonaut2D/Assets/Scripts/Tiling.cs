using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]     //makes sure we always have a spriterenderer

public class Tiling : MonoBehaviour {

    public int offsetX = 2;                                     //offset so we cont get clipping errors
    public bool hasRightBuddy = false, hasLeftBuddy = false;    //used for checking if we need to instantiate buddies

    //used for background elements who're not tilable
    public bool reverseScale = false;
    //width of our element
    private float spriteWidth = 0f;
    //camera ref
    private Camera cam;
    //faster to store a reference to it
    private Transform myTransform;

    private void Awake() {
        cam = Camera.main;
        myTransform = transform;
    }

	// Use this for initialization
	void Start () {
        //grab the first object of type given
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;       //gives width of element no matter how we size it
	}
	
	// Update is called once per frame
	void Update () {
        //check if a buddy is needed
		if(!hasLeftBuddy || !hasRightBuddy) {
            //calculate half the width of what the camera can see in world coordinates
            float camHorizontalExtend = cam.orthographicSize * (Screen.width / Screen.height); //center of cam to right bar = half the cam view

            //calculate the x position where the camera can see the edge of the sprite
            float visibleEdgePosRight = (myTransform.position.x + (spriteWidth * 0.75f) / 2) - camHorizontalExtend;
            float visibleEdgePosLeft = (myTransform.position.x - (spriteWidth * 0.75f) / 2) + camHorizontalExtend;
            
            //check if position of camera is >= to where the element is visible
            if(cam.transform.position.x >= visibleEdgePosRight - offsetX && !hasRightBuddy) {//dont instantiate if rightbuddy already exists
                createBuddy(1);//right
                hasRightBuddy = true;
            } else if(cam.transform.position.x <= visibleEdgePosLeft + offsetX && !hasLeftBuddy) {
                createBuddy(-1);//left
                hasLeftBuddy = true;
            }
        }
	}

    //Creates a buddy on the given side denoted by direction
    void createBuddy(int direction) { //-1<---  --->1
        //calculating the new position for the new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + (spriteWidth * 0.75f) * direction, myTransform.position.y, myTransform.position.z); // must half the width since origin is left corner
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;//clone of the object

        //flips the background sprite so it matches the seam perfectly. gg brackeys
        if (true) {//object is not tileable
            newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);//invert the x scale of the new buddy so it perfectly loops
        }

        newBuddy.parent = myTransform.parent;
        if(direction > 0) {
            newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;//the opposite side we're creating a buddy on already has a buddy so tell it so
        }else {
            newBuddy.GetComponent<Tiling>().hasRightBuddy = true;//same as above
        }
    }
}
