﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : IHeapItem<Nodes> { // Inherit from interface IHeapItem

    public bool walkable;            
    public Vector3 worldPosition;  
    public Nodes parent;            
    public int gridX;               
    public int gridY;               
    public int distanceCost;       
    public int heuristicCost;      
    private int heapIndex;  // Initalise fields 

    public Nodes (bool Walkable, Vector3 worldPos, int GridX, int GridY) // Method holding 4 parameters of bool, Vector3, int, int
    {
        walkable = Walkable; // Assign the value of Walkable to walkable field
        worldPosition = worldPos; // Assign the value of worldPosition to worldPos field
        gridX = GridX; // Assign the value of gridX to GridX field
        gridY = GridY; // Assign the value of gridY to GridY field

    }

    public int fCost // Constructor fCost
    {
        get
        {
            return distanceCost + heuristicCost; // returns distanceCost + heuristicCost, creating the effective cost
        }
    }

    public int HeapIndex // Constructor HeapIndex
    {
        get
        {
            return heapIndex; // return heapIndex
        }
        set
        {
            heapIndex = value; // return heapIndex = value
        }
    }

    public int CompareTo (Nodes nodeToCompare) // Method holding parameter of Nodes
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost); // Assign the comparison between the fCost and the nodes fCost
        if (compare == 0) // if this is equal to 0
        {
            compare = heuristicCost.CompareTo(nodeToCompare.heuristicCost); // Assign the comparison between the heuristicCost and the nodes heuristicCost
        }
        return -compare; // return negative compare
    }

}
