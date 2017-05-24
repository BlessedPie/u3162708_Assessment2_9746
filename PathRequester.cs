using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequester : MonoBehaviour {
        
    public bool isProcessingPath = false;

    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>(); // Create Queue of <PathRequest>
    private PathRequest currentPathRequest;
    private static PathRequester instance; // Creates an Instance of PathRequester
    private Pathfinding pathfinding; // Gets Pathfinding Script

    void Awake()
    {
        instance = this; // Assigns it to this object
        pathfinding = GetComponent<Pathfinding>(); // Creates Instance of pathfinding script
    }

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> called)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, called); // Assign parameters to newRequest from struct PathRequest
        instance.pathRequestQueue.Enqueue(newRequest); // Put the newRequest into the Queue
        instance.TryProcessNext(); // Call Method TryProcessNext()
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0) // If the pathfinding script isn't generating a path and there is a variable in the queue, run
        {            
            currentPathRequest = pathRequestQueue.Dequeue(); // Remove the request from the queue and assign it  
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // Call pathfinding script method 'StartFindPath'
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.called(path, success); // Assigns the path and success to the delegate called
        isProcessingPath = false;
        TryProcessNext(); // Call TryProcessNext()
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> called;

        public PathRequest (Vector3 Start, Vector3 End, Action <Vector3[], bool> CalledPath)
        {
            pathStart = Start;
            pathEnd = End;
            called = CalledPath; // Assign fields to parameters
        }

    }


}
