using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
using System.Collections;


[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class BaddieAI : MonoBehaviour {

    //What to chase
    public Transform target;

    //how many times a second we update our path
    public float updateRate = 2f;

    //Chaching
    private Seeker seekerRef;
    private Rigidbody2D rb;

    //The calculated path
    public Path path;

    //The AI speed per second
    public float speed = 300f;
    public ForceMode2D fMode;   //A way to change between force and impulse

    [HideInInspector]//public - but won't show in the unity inspector
    public bool pathIsEnded = false;

    //max dist from ai to waypoint for it to continue onto next waypoint
    public float nextWaypointDistance = 0.1f; //how close do we need to get to a waypoint till it says - we reached it - and continue onto the next one
    //The waypoint we're currently moving towards
    private int currentWaypoint = 0;    //0 based index

    private bool searchingForPlayer = false;

    void Start() {
        seekerRef = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //TODO: extract to another method...but had weird results earlier
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(searchForPlayer());
            }
            return;
        }

        //Start a new path to the target position, return the result to the OnPathComplete method
        seekerRef.StartPath(transform.position, target.position, onPathComplete);//start position, target position, result passed into function

        StartCoroutine(updatePath());
    }

    IEnumerator searchForPlayer() {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if(sResult == null) {
            yield return new WaitForSeconds(0.5f); //check twice a second for player
            StartCoroutine(searchForPlayer());
        }else {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(updatePath());
            yield break;
        }
    }

    IEnumerator updatePath() {
        //TODO: extract to another method...but had weird results earlier
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(searchForPlayer());
            }
            yield break;
        }
        seekerRef.StartPath(transform.position, target.position, onPathComplete);

        yield return new WaitForSeconds(1 / updateRate);    //the more we increase updateRate the smaller the waittime will be
        StartCoroutine(updatePath());   //recurse

    }

    public void onPathComplete(Path p) {
        Debug.Log("We got a path: did it have an error? " + p.error);
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    //instead of every frame updating - use this which is best for physics: DO NOT USE Update()
    void FixedUpdate() {
        //TODO: extract to another method...but had weird results earlier
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(searchForPlayer());
            }
            return;
        }
        //TODO: always look at player

        //Nothing to chase
        if (path == null) {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count) { //path.vectorPath returns all waypoints as arrray
            if (pathIsEnded) {
                return; //don't chase a path that has ended
            }

            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Find direction to next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the Baddie!
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);    //TODO manually get distance leaving out sqrt...unless it already does
        if (dist < nextWaypointDistance) {
            ++currentWaypoint;
            return;
        }
    }
}
