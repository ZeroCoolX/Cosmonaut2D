using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBulletTrail : MonoBehaviour {

    public int movementSpeed = 230;

	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);//takes direction and spacial orientation
        Destroy(this.gameObject, /*time to destroy in seconds*/ 1);//this game object
	}
}
