using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    public Transform[] targets;
    public float speed = 40.0f;
    public bool haveSight = false;
    private EnemyController enemyController;
    private PathRequester pathRequester;
    private float pursuitTime = 0.0f;
    private float waitTime = 0.0f;
    private Vector3[] path;
    private int targetIndex;
    private int? oldTargetIndex = null; // Initalise fields 

    private void Start ()
    {
        int targetTemp = Random.Range(0, targets.Length); // Random between 0 and length of targets set
        if (targetTemp == oldTargetIndex) // if the target temp is equal to oldTargetIndex
        {
            if (targetTemp - 1 >= 0) // Is targetTemp - 1 >= 0?
            {
                targetTemp--; // Take away one from targetTemp
            }
            else // if anything else
            {
                targetTemp++; // Plus one  to targetTemp
            }
        }
        enemyController = GetComponent<EnemyController>(); // Instance of EnemyController
        Transform target = targets[targetTemp]; // Set the target to targetTemp
        oldTargetIndex = targetTemp; 
        haveSight = enemyController.CanSeePlayer(GameObject.FindGameObjectWithTag("Player").transform); // Checking boolean from EnemyController
        if (haveSight) // If true
        {            
            target = enemyController.PlayerTransform; // Set the target to the players Transform
        }          
        
        PathRequester.RequestPath(transform.position, target.position, OnPathFound); // Call Method from PathRequester with method OnPathFound                
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) 
    {
        if (pathSuccessful) // If this is true
        {
            path = newPath; 
            targetIndex = 0;
            StopCoroutine("FollowPath"); // Stop Coroutine if currently running
            StartCoroutine("FollowPath"); // Start Coroutine called FollowPath
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0]; // currentWayPoint = path[0]

        while (true) // while running
        {            
            if (transform.position == currentWaypoint)  // if the position of the Enemy Object is at the currentWayPoint
            {
                targetIndex++; // targetIndex plus one     
                if (targetIndex >= path.Length) // if the targetIndex is greater or equal to the length of path
                {
                    targetIndex = 0;
                    path = new Vector3[0]; // Reset path and targetIndex          
                    if (haveSight) // if havesight is true
                    {
                        yield return new WaitForSeconds(pursuitTime); // Run pursuitTime wait                     
                    }
                    else // if haveSight is false
                    {
                        yield return new WaitForSeconds(waitTime); // Run waitTime wait
                    }                    
                    Start(); // Call Start method
                    yield break; // break                                                                   
                }
                currentWaypoint = path[targetIndex]; // Increment targetIndex to path
            }            
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime); // Move towards the currentWaypoint 
            yield return null; // Wait for one frame
        }        
    }  
}
