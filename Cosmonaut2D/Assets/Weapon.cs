using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0f;
    public float Damage = 10f;
    public LayerMask whatToHit;      //will tell us what we want to hit

    private float timeToFire = 0f;
    private Transform firePoint;

    void Awake() {
        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null) {
            Debug.LogError("For some reason there is no firepoint!");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(fireRate == 0) {//single burst
            if (Input.GetButtonDown("Fire1")) {
                shoot();
            }
        } else { //automatic
            if(Input.GetButton("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                shoot();
            }
        } 
	}

    //fire a projectile
    void shoot() {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100, Color.cyan);//the bullet fires off into the direction not stopping where the mouse is
        if(hit.collider != null) {
            Debug.DrawLine(firePointPosition,hit.point, Color.red);//the bullet fires off into the direction not stopping where the mouse is
        }
    }
}
