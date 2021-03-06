﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroy : MonoBehaviour {

	public int scoreValue; 
	private GameController gameController; // Instance of GameController script

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController"); // Find gameObject with tag "GameController" and assign it to gameControllerObject
		if (gameControllerObject != null) // If it couldn't find the object with tag "GameController", run
		{
			gameController = gameControllerObject.GetComponent <GameController> ();
		}
	}

	void OnTriggerEnter(Collider other) 
	{		
        if (other.CompareTag ("Boundary") || other.CompareTag ("Obstacle") || other.CompareTag ("Enemy") || other.CompareTag ("Wall")) // If this object collides with any of these tags, ignore
        {
            return; 
        }

		gameController.AddScore(scoreValue); // Call Method AddScore and provide parameter (scoreValue)		
		Destroy(gameObject); // Destroy this gameObject 
	}     
    
}
