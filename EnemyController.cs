﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{     
    public LayerMask sightMask;
    public bool haveSight = false;
    public Transform PlayerTransform = null;
    public Transform ThisTransform = null;

    private float fieldOfView = 180f; // Initialise fields
    private GameController gameController; // Get instance of GameController script

    private void Awake()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController"); // Find gameObject with tag "GameController" and assign it to gameControllerObject

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        ThisTransform = transform;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Find the players position and assign it
    }

    private void Update()
    {
        haveSight = CanSeePlayer(PlayerTransform); // haveSight becomes true if CanSeePlayer is true
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") // if this collides with an object with the tag of "Player", run
        {
            other.gameObject.SetActive(false); // Set player to false
            gameController.GameOver(); // Call gameOver method in GameController
        }
    }

    public bool CanSeePlayer(Transform Player)
    {
        float Angle = Mathf.Abs(Vector3.Angle(ThisTransform.forward, (Player.position - ThisTransform.position).normalized)); // Creates an angle between the player and enemy (Max 180)
        if (Angle > fieldOfView) // if this angle is greater than the field of view, the player cannot be seen
        {
            return false;
        }

        if (Physics.Linecast(ThisTransform.position, Player.position, sightMask)) // if the linecast between the player and enemy collide with an obstacle, the player cannot be seen
        {
            return false;
        }
        return true; // the player is seen
    }

}