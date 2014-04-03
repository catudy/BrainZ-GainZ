﻿using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private GameObject player;
	private PlayerState playerState;
	private ParticleSystem fireextinguisher;
	private Camera cam;
	private GameState gameState;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		playerState = player.GetComponent<PlayerState>();
		fireextinguisher = GameObject.Find ("FireExtinguisherParticleEffect").GetComponent<ParticleSystem> ();
		cam = player.GetComponentInChildren<Camera> ();
		gameState = GetComponent<GameState> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButton("Ability")){
			if(gameState.UseFireExtinguisher()){
				fireextinguisher.transform.position = player.transform.position;
				fireextinguisher.transform.rotation = cam.transform.rotation;
				fireextinguisher.enableEmission = true;
			}
		} else {
			fireextinguisher.enableEmission = false;
		}

		if (Input.GetButton ("Swap Active")) {
			gameState.NextItem();
		}

		if (Input.GetButton ("Sneak")) 
		{
			playerState.SetSneaking ();
		} 

		else if (Input.GetButton ("Sprint")) 
		{
			playerState.SetRunning();
		} 

		else 
		{
			playerState.SetWalking ();
		}
	}
}
