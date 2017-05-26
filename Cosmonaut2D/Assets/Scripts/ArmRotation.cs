using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 90;
	
	// Update is called once per frame
	void Update () {
        //difference between mouse and player
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //Normalize the vector x + y + z = 1
        diff.Normalize();

        //find the angle in degrees
        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        //apply the rotation
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);//degrees not radians
	}
}
