/// <summary>
/// Handles objects that collide with players.  Namely enemies, BnG, and powerups.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
	private PlayerState playerState; 

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("Collided with " + collision.gameObject.name);
		if (collision.gameObject.name == "PowerupBlink") {
			playerState.power_up = PowerUp.BLINK;
			Destroy(collision.gameObject);
		}
	}
	// Use this for initialization
	void Start () {
		playerState = GetComponent<PlayerState>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
