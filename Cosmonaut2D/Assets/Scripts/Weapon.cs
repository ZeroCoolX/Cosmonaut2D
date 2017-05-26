using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0f;
    public int Damage = 10;
    public LayerMask whatToHit;      //will tell us what we want to hit

    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;

    public float timeToSpawnEffect = 0f;
    public float effectSpawnRate = 10f;

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
        //collect the hit data
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        if (Time.time >= timeToSpawnEffect) {
            //Bullet effect
            //use with yield if you want below  
            //StartCoroutine("generateEffect"); //allows to use yield
            generateEffect();
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100, Color.cyan);//the bullet fires off into the direction not stopping where the mouse is
        if(hit.collider != null) {
            Debug.DrawLine(firePointPosition,hit.point, Color.red);//the bullet fires off into the direction not stopping where the mouse is
            Baddie baddie = hit.collider.GetComponent<Baddie>();
            if (baddie != null) {
                baddie.damageHealth(Damage);
                Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage");
            }
        }
    }

    //return type allows for yield to be used
    /*IEnumerator*/void generateEffect() {
        Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform; //when assiging to variable remember to cast to what type you expect
        //parent to firepoint
        clone.parent = firePoint;

        //randomize size
        float size = Random.Range(0.4f, 0.7f);
        clone.localScale = new Vector3(size, size, size);

        //use if you want with coroutine above 
        //yield return 0; //skip one frame before calling below code
        Destroy(clone.gameObject, 0.02f);
    }
}
