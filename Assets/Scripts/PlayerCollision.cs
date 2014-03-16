/// <summary>
/// Handles objects that collide with players.  Namely enemies, BnG, and powerups.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
		Debug.Log ("Crash");
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
