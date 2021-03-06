﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T> { // Uses interface IHeapItem as parameter in the class 

    private T[] items; // Create a T Array for items
    private int currentItemCount; // Initialise fields

    public Heap(int maxHeapSize) // Holder parameter for maxHeapSize
    {
        items = new T[maxHeapSize]; // Assign instance of T[maxHeapSize]
    }

    public void Add (T item) // Method with parameter T
    {
        item.HeapIndex = currentItemCount; // Uses interface 'HeapIndex' and assign currentItemCount to variable
        items[currentItemCount] = item; // Assigns item to items array with iterations of currentItemCount
        SortUp(item); // Call SortUp method with parameter item
        currentItemCount++; // Plus one to currentItemCount
    }

    public T RemoveFirstItem() // Method with no parameters, returns T
    {
        T firstItem = items[0]; // Assigns first item in items array to T firstItem
        currentItemCount--; // Take away one from currentItemCount

        items[0] = items[currentItemCount]; // Assign items[currentItemCount] to items Array
        items[0].HeapIndex = 0; // 
        SortDown(items[0]); // Call SortDown method with parameter items[0]
        return firstItem; // returns the firstItem variable
    }

    public void UpdateItemHeap(T item) // Method with the parameter T
    {
        SortUp(item); // Call SortUp method with parameter item
    }

    public int Count // Constructor Count
    {
        get
        {
            return currentItemCount; // returns currentItemCount
        }
    }

    public bool Contains (T item) // Method with the parameter T
    {
        return Equals(items[item.HeapIndex], item); // Returns method Equals with 2 parameters
    }

    private void SortDown (T item) // Method with the parameter T
    {
        while(true) // While this is running
        {
            int childIndexLeft = item.HeapIndex * 2 + 1; // Add to childIndexLeft (LeftNode of Procedual Tree) 
            int childIndexRight = item.HeapIndex * 2 + 2; // Add to childIndexRight (RightNode of Procedual Tree) 
            int swapIndex = 0; // Set variable swapIndex to 0

            if (childIndexLeft < currentItemCount) // Check whether childLeft is less than the parent
            {
                swapIndex = childIndexLeft; // Assign the leftIndex to swapIndex
                if (childIndexRight < currentItemCount) // Check whether childRight is less than the parent
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) // Check if rightIndex is greater than leftIndex
                    {
                        swapIndex = childIndexRight; // Assign the rightIndex to swapIndex
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) // If the swapIndex is greater than 0 after being compared
                {
                    Swap(item, items[swapIndex]); // Call Swap and switch the Indexes around
                }
                else // if not true
                {
                    return; // break loop
                }
            }

            else // if not true
            {
                return; // break loop
            }
        }
    }

    private void SortUp (T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2; // Assigns value to parentIndex

        while (true) // While running 
        {
            T parentItem = items[parentIndex]; // Assign value to parentItem

            if (item.CompareTo(parentItem) > 0) // If  the item is greater than the parentItem
            {
                Swap(item, parentItem); // Call Swap and switch the Indexes around
            }
            else // If not true
            {
                break; // break loop
            }
            parentIndex = (item.HeapIndex - 1) / 2; // Assigns value to parentIndex
        }
    }

    private void Swap (T itemA, T itemB) // Swap Method
    {
        items[itemA.HeapIndex] = itemB; // Assigns value of item B to item A
        items[itemB.HeapIndex] = itemA; // Assigns value of item A to item B

        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex; // Switches itemA with itemB
        itemB.HeapIndex = itemAIndex; // Switches itemB with itemA
    }

}

public interface IHeapItem<T> : IComparable<T> // interface 
{
    int HeapIndex
    {
        get;
        set;
    }
}
