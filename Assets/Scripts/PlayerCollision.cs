/// <summary>
/// Handles objects that collide with players.  Namely enemies, BnG, and powerups.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
	private PlayerState playerState; 
	void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "NoCollision") { // Don't care about the ground.
			return;
		}

		// Log the collision
		Debug.Log ("Collided with " + collision.gameObject.name);

		// Filter through the collisions
		if (collision.gameObject.tag == "Deadly") { // Game over if you run into something deadly
			if(playerState.power_up == PowerUp.INVULNERABLE){ 
				// Fuck you Zambie
				Destroy (collision.gameObject);
			} else {
				GameState gameState = GameObject.Find("gameController").GetComponent<GameState>();
				gameState.game_over = true;
			}
		} else if (collision.gameObject.tag == "Brainz"){
			GameState gameState = GameObject.Find("gameController").GetComponent<GameState>();
			gameState.brainz++;
			Destroy (collision.gameObject);
		} else if (collision.gameObject.tag == "Gainz"){
			GameState gameState = GameObject.Find("gameController").GetComponent<GameState>();
			gameState.gainz++;
			Destroy (collision.gameObject);
		} else if (collision.gameObject.tag == "Powerup"){
			playerState.DeletePowerup();
			if (collision.gameObject.name == "PowerupBlink(Clone)") {
				playerState.SetPowerUp(PowerUp.BLINK);
				Destroy (collision.gameObject);
			} else if(collision.gameObject.name == "PowerupInvuln(Clone)") {
				playerState.SetPowerUp(PowerUp.INVULNERABLE);
				Destroy (collision.gameObject);
			} else if(collision.gameObject.name == "PowerupInvis(Clone)") {
				playerState.SetPowerUp(PowerUp.INVISIBILITY);
				Destroy (collision.gameObject);
			}
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
