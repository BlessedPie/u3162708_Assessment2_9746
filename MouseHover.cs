﻿using UnityEngine;
using System.Collections;

public class MouseHover : MonoBehaviour {    

	private void Start ()
    {
        GetComponent<Renderer>().material.color = Color.white; // On intial run, set buttons to white        
	}
	
	private void OnMouseEnter ()
    {
        GetComponent<Renderer>().material.color = Color.cyan; // When the mouse goes over the button, set it to Cyan
	}
    private void OnMouseExit ()
    {
        GetComponent<Renderer>().material.color = Color.white; // When the mouse moves away from the button, set it back to white
    }
}
