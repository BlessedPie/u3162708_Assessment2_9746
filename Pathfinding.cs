using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Pathfinding : MonoBehaviour {    
    
    private Grid grid; // Create instance of Grid Script
    private PathRequester requestManager; // Create instance of PathRequester Script
    
    private void Awake() // On Awake
    {
        grid = GetComponent<Grid>(); // Properly reference Grid script and assign it to grid
        requestManager = GetComponent<PathRequester>(); // Properly reference PathRequester script and assign it to requestManager
    }    

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) // Calls coroutine Path and accepts 2 parameters of Vector3
    {
        StopCoroutine(Path(startPos, targetPos)); // Stop running Path coroutine
        StartCoroutine(Path(startPos, targetPos)); // Start running Path coroutine
    }

    private IEnumerator Path (Vector3 startPos, Vector3 targetPos) // IEnumerator which holds 2 parameters of Vector3
    {
        Vector3[] waypoints = new Vector3[0]; // Create an Array of waypoints for the AI to follow
        bool pathSuccess = false; // Set pathSuccess to false, because it is not completed

        Nodes startNode = grid.PlayerNode(startPos);  // Starting position of the algorithm (Start position of Enemy)      
        Nodes targetNode = grid.PlayerNode(targetPos); // End position of the algorithm (Either set point within Game or of player's position)

        if (startNode.walkable && targetNode.walkable) // Only run if both nodes are walkable, if they aren't, the AI cannot reach the position due to Obstacles
        {
            Heap<Nodes> openSet = new Heap<Nodes>(grid.MaxSize); // Create an openSet of nodes
            HashSet<Nodes> closedSet = new HashSet<Nodes>(); // Create a closedSet of nodes
            openSet.Add(startNode); // Adds the startNode to the openSet to determine its neighbours

            while (openSet.Count > 0) // If there is Nodes in the openSet, continue to run
            {              
                Nodes currentNode = openSet.RemoveFirstItem(); // Removes the startNode from the openSet and assign it to currentNode
                closedSet.Add(currentNode); // Add the currentNode (startNode) to the ClosedSet

                if (currentNode == targetNode) // If we have reached the target node
                {
                    pathSuccess = true; // Set to true, because we have completed the path
                    break; // break out of loop
                }

                foreach (Nodes neighbour in grid.NeighbourNode(currentNode)) // Iterate through the Neighbour nodes to the currentNode
                {                    
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) // If a neighbour node is an Obstacle or is already put within the closedSet, ignore
                    {
                        continue; // ignore and continue on
                    }


                    int newCostToNeighbour = currentNode.distanceCost + GetDistance(currentNode, neighbour); // Assigns the cost from the currentNode to the neighbourNode
                    if (newCostToNeighbour < neighbour.distanceCost || !openSet.Contains(neighbour))
                    {
                        neighbour.distanceCost = newCostToNeighbour; // The distance travelled between the currentNode and the neighbourNode
                        neighbour.heuristicCost = GetDistance(neighbour, targetNode); // The estimated cost for travelling towards the targetNode

                        neighbour.parent = currentNode; // The parent of neighbourNodes is the current Node

                        if (!openSet.Contains(neighbour)) // if the openSet doesn't contain a neighbourNode, add it to it
                        {
                            openSet.Add(neighbour); // Adds neighbour to openSet
                        }
                        else // otherwise
                        {
                            openSet.UpdateItemHeap(neighbour); // Update and resort the Heap
                        }
                    }
                }
            }
            yield return null; // Wait a frame before continuing

            if (pathSuccess) // if the path is completed
            {
                waypoints = RetracePath(startNode, targetNode); // Call the Retrace path Method with parameters(startNode, targetNode)
                pathSuccess = waypoints.Length > 0; // pathSuccess remains true if there is Nodes within the waypoints List
            }
            requestManager.FinishedProcessingPath(waypoints, pathSuccess); // Send back waypoints and pathSuccess being true to PathRequester Script
            

        }
    }

    private Vector3[] RetracePath(Nodes startNode, Nodes endNode) // Vector3 Array which holds 2 parameters of Vector3
    {
        List <Nodes> path = new List<Nodes>(); // Create a list called path

        Nodes currentNode = endNode; // Assign the currentNode to the endNode, due to the path should be completed

        while (currentNode != startNode) // While the currentNode is anything but the StartNode, run
        {
            path.Add(currentNode); // Add the currentNode to the path List
            currentNode = currentNode.parent; // Make the currentNode the parentNode
        }
        Vector3[] waypoints = SimplifyPath(path); // Call SimplifyPath method and assign it to waypoints array
        Array.Reverse(waypoints); // Reverse the list to give the proper order of waypoints
        return waypoints; // return the value of waypoints
        
    }

    private Vector3[] SimplifyPath(List<Nodes> path) // Vector3 Array which holds the parameters of List<Nodes>
    {
        List<Vector3> waypoints = new List<Vector3>(); // Create new list called waypoints using the type Vector3
        Vector2 directionOld = Vector2.zero; // Set position to 0, 0 in worldSpace

        for (int i = 1; i < path.Count; i++) // while i is Less than the amount in path, continue to iterate
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY); // Creates a Vector2 newDirection variable which works off the grid template 
            if (directionNew != directionOld) // if directionNew is not equal to directionOld
            {
                waypoints.Add(path[i].worldPosition); // Add the path[i].worldposition to waypoints
                
            }
            directionOld = directionNew; // Assign directionNew to directionOld
        }
        return waypoints.ToArray(); // return waypoints and convert it to an Array
    }

    private int GetDistance(Nodes nodeA, Nodes nodeB) // Int which holds 2 parameters of Nodes
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX); // Assign the absolute value to distX from Nodes.GridX
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY); // Assign the absolute value to distY from Nodes.GridY

        if (distX > distY) // if distX is greater than distY, run this
        {
            return 14 * distY + 10 * (distX - distY); // Determines distance between nodes going up
        } // else run
        return 14 * distX + 10 * (distY - distX); // Determines distance between nodes going down 
    }   
}
