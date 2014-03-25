﻿/// <summary>
/// Handles game state variables.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public bool game_over = false;
	public int brainz = 0;
	public int gainz = 0;
	public bool in_cutscene = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > 30.0f) {
			in_cutscene = false;
		}
		if (game_over) {
			// Call game over here
		}
	}

	// Removes object from the scene.
	public void RemoveObject(GameObject destroyme){
		Spawner[] spawners = GetComponents<Spawner> ();
		foreach (Spawner spawner in spawners) {
			foreach(GameObject obj in spawner.objects){
				if(obj.name + "(Clone)" == destroyme.name){
					spawner.DestroyObject(destroyme);
				}
			}
		}
	}

	public void ChangeScene(string scene_name){
		if(scene_name == "TestBuilding"){
			// Manage scene change here.

			// Load Scene change
		}
	}
}
