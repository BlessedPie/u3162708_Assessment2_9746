using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector3 mousePosition; 
    public float moveSpeed; // Initialise fields

    void FixedUpdate () {    
        if (Input.GetMouseButton(0)) // If LeftMouseButton is pressed down, run this
        {
            mousePosition = Input.mousePosition; // Gets the current mouse position 
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // Translates the mousePosition to the view from the Camera         
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, moveSpeed); // Move towards the mousePosition in the gameWorld      
            GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 4f, GetComponent<Transform>().position.z); // Freezes the Y transform position for the player     
        }                   
	}
}
