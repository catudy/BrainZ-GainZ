/// <summary>
/// Handles objects that collide with players.  Namely enemies, BnG, and powerups.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour 
{
	private PlayerState playerState; 
	GameState gameState;

	void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "NoCollision") { // Don't care about the ground.
			return;
		} else if (collision.gameObject.tag == "Deadly") { 
			// Game over if you run into something deadly
			if(playerState.power_up == PowerUp.INVULNERABLE){ 
				// Fuck you Zambie
				gameState.RemoveObject(collision.gameObject);
			} else {
				playerState.DealDamage(1);
				Debug.Log ("Took Damage from " + collision.gameObject.name);
			}
		} else if (collision.gameObject.tag == "Projectile"){
			gameState.game_over = true;
			collision.gameObject.GetComponent<Projectile>().DestroyWithExplosion(collision.gameObject);
		}

		else if (collision.gameObject.tag == "pickup") {
			gameState.pickup_temp++;
			Destroy (collision.gameObject);

		}

		else if (collision.gameObject.tag == "Brainz") {
			gameState.brainz++;
			gameState.RemoveObject(collision.gameObject);
		} 
 
		else if (collision.gameObject.tag == "Gainz") {
			gameState.gainz++;
			Destroy (collision.gameObject);
		} 

		else if(collision.gameObject.tag == "ak")
		{
			Destroy(collision.gameObject);
			playerState.active[1] = true;
		}

		else if (collision.gameObject.tag == "Powerup") {
			// Takes current powerup away
			playerState.DeletePowerup();

			// Update based on what powerup
			if (collision.gameObject.name == "PowerupBlink(Clone)") {
				playerState.SetPowerUp(PowerUp.BLINK);
			} else if(collision.gameObject.name == "PowerupInvuln(Clone)") {
				playerState.SetPowerUp(PowerUp.INVULNERABLE);
			} else if(collision.gameObject.name == "PowerupInvis(Clone)") {
				playerState.SetPowerUp(PowerUp.INVISIBILITY);
			} else if(collision.gameObject.name == "PowerupSecondWind(Clone)") {
				playerState.SetPowerUp(PowerUp.SECOND_WIND);
			}

			// Delete Powerup Object
			gameState.RemoveObject(collision.gameObject);
		} 
		else if (collision.gameObject.tag == "SceneChanger"){
			gameState.ChangeScene(collision.gameObject.name);
		} 
		else if (collision.gameObject.tag == "Item"){
			if (collision.gameObject.name == "Extinguisher"){
				gameState.AddItem(Item.FIRE_EXTINGUISHER);
				Destroy (collision.gameObject);
			} else if (collision.gameObject.name == "Brain(Clone)"){
				gameState.AddItem (Item.BRAINZ);
				Destroy (collision.gameObject);
			}
			// Update Scavenger for now
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
		}
	}

	/*void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ak") 
		{
			Destroy (other.gameObject);
			playerState.active[1] = true;
		}
		if(other.gameObject.tag == "smg")
		{
			Destroy(other.gameObject);
			playerState.active[2] = true;
		}
		if(other.gameObject.tag == "rev")
		{
			Destroy(other.gameObject);
			playerState.active[3] = true;
		}
	}*/
	// Use this for initialization
	void Start () 
	{
		playerState = GetComponent<PlayerState>();
		gameState = GameObject.Find("GameController").GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
