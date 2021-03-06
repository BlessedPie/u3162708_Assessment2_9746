﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask ObstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    private Nodes[,] grid; // Gets 2D array Instance from Nodes
    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY; // Initialise fields


    private void Awake() // On initial run
    {
        nodeDiameter = nodeRadius * 2; 
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter); // Round to int to fit within grid measurements
        CreateGrid(); // Call CreateGrid method
    }

    private void CreateGrid() 
    {
        grid = new Nodes[gridSizeX, gridSizeY]; // Grid is equal to gridSizeX and gridSizeY
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; // Find the bottom position of the grid

        for (int x = 0; x < gridSizeX; x++) // While x is less than the gridSizeX
        {
            for (int y = 0; y < gridSizeY; y++) // While y is less than the gridSizeY
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); // Goes through each node within the gridSize
                bool obstacle = !(Physics.CheckSphere(worldPoint, nodeRadius, ObstacleMask)); // If there is an obstacle in the Node, set bool Obstacle is true

                grid[x, y] = new Nodes(obstacle, worldPoint, x, y); // Assign parameter obstacle, worldPoint, x, y to 2D array
            }
        }
    }

    public int MaxSize // Constructor MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY; // The max size of the grid
        }
    }
    
    public Nodes PlayerNode(Vector3 worldPosition) 
    {
        float posX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x; 
        float posY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        posX = Mathf.Clamp01(posX); // Clamp posX between a range of 0 - 1 (0% - 100%)
        posY = Mathf.Clamp01(posY); // Clamp posY between a range of 0 - 1 (0% - 100%)

        int x = Mathf.RoundToInt((gridSizeX - 1) * posX); // Determines the position where the target node on the X cooordinate and rounds it to an Int
        int y = Mathf.RoundToInt((gridSizeY - 1) * posY); // Determines the position where the target node on the Y cooordinate and rounds it to an Int
        return grid[x, y]; // Returns x and y to 2D grid Array
    }

    public List<Nodes> NeighbourNode(Nodes node)
    {
        List<Nodes> neighbours = new List<Nodes>(); // Create List<Nodes> for neighbours

        for (int x = -1; x <= 1; x++) // iterate through neighbourNodes on x axis
        {
            for (int y = -1; y <= 1; y++) // iterate through neighbourNodes on y axis
            {
                if (x == 0 && y == 0) // if x && y == 0 (In other words, parentNode), ignore
                {
                    continue; // ignore
                }
                int checkX = node.gridX + x; 
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) // Check if neighbourNode is within gridSize and not parentNode
                {
                    neighbours.Add(grid[checkX, checkY]); // Add to neighbours list in a 2D grid Array
                }
            }
        }
        return neighbours; // return neighbours
    }    
}
