using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    //how  fast the weapon can shoot
    public float fireRate = 0f;
    //how much damage weapon does
    public int Damage = 10;
    public LayerMask whatToHit;      //will tell us what we want to hit

    //bullet graphics
    public Transform bulletTrailPrefab;
    public Transform hitPrefab;
    public Transform muzzleFlashPrefab;

    //graphics spawning
    public float timeToSpawnEffect = 0f;
    public float effectSpawnRate = 10f;

    //handle can shake for hits
    public float camShakeAmount = 0.025f;
    public float camShakeLength = 0.1f;
    private CameraShake camShake;

    //delat between each firing
    private float timeToFire = 0f;
    //where to create the bullet origin
    private Transform firePoint;

    //weapon audio
    //TODO: change based off weapon
    public string weaponShootSound = "Shot";

    //caching
    AudioManager audioManager;

    //store and cache the fire point
    void Awake() {
        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null) {
            Debug.LogError("Weapon.cs - no Fire Point found in Awake");
        }
    }

    //locate and cache camshake and audio manaager
    void Start() {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null) {
            Debug.LogError("Weapon.cs - No camShake found");
        }

        audioManager = AudioManager.instance;
        if(audioManager == null) {
            Debug.LogError("Weapon.cs - No audio manager found in scene");
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
        //store mouse position B
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        //store bullet origin spawn point A
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        //collect the hit data - distance and direction from A -> B
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        //the bullet fires off into the direction not stopping where the mouse is
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition)*100, Color.cyan);
        if(hit.collider != null) {
            Debug.DrawLine(firePointPosition,hit.point, Color.red);
            //check if we hit a baddie
            Baddie baddie = hit.collider.GetComponent<Baddie>();
            if (baddie != null) {
                baddie.damageHealth(Damage);
                Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage");
            }
        }

        if (Time.time >= timeToSpawnEffect) {
            //Bullet effect
            Vector3 hitPos;
            Vector3 hitNormal;

            if(hit.collider == null) {
                hitPos = (mousePosition - firePointPosition) * 30;//allows bullet to fly off into space
                hitNormal = new Vector3(9999, 9999, 9999);  //rediculously huge
            }else {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            generateEffect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void generateEffect(Vector3 hitPos, Vector3 hitNormal) {

        //Generate hit particles
        if (hitNormal != new Vector3(9999, 9999, 9999)) {
            //we hit something 
            //maske it so when we hit something the particles shoot OUT from it. To if south face hit particles shoot north. If east face hit, particles shoot west
            Transform hitParticles = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.up, hitNormal)) as Transform;
            Destroy(hitParticles.gameObject, 1f);
        }

        //Generate muzzleflash
        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform; //when assiging to variable remember to cast to what type you expect
        //parent to firepoint
        clone.parent = firePoint;
        //randomize size
        float size = Random.Range(0.4f, 0.7f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);

        //Generate bullet trail
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        //allows the bullet trail to stop where the collision happenned
        if (lr != null) {
            lr.SetPosition(0, firePoint.position);//start position index
            lr.SetPosition(1, hitPos);//end position index
        }
        Destroy(trail.gameObject, 0.02f);

        //Generate camera shake
        camShake.shake(camShakeAmount, camShakeLength);

        //Generate shot sound
        audioManager.playSound(weaponShootSound);
    }
}
