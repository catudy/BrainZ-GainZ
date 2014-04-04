/// <summary>
/// Handles objects that collide with players.  Namely enemies, BnG, and powerups.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour 
{
	private PlayerState playerState; 
	GameState gameState;
	public AudioClip lift;


	void OnControllerColliderHit(ControllerColliderHit collision) 
	{
		if (collision.gameObject.tag == "NoCollision") 
		{ // Don't care about the ground.
			return;
		}

		// Log the collision
		Debug.Log ("Collided with " + collision.gameObject.name + " " + collision.gameObject.tag);

		// Filter through the collisions
		if (collision.gameObject.tag == "Deadly") 
		{ 
			// Game over if you run into something deadly
			if(playerState.power_up == PowerUp.INVULNERABLE)
			{ 
				// Fuck you Zambie
				gameState.RemoveObject(collision.gameObject);
			} 

			else 
			{
				gameState.game_over = true;
			}
		} 

		else if (collision.gameObject.tag == "pickup")
	    {
			gameState.pickup++;
			Destroy (collision.gameObject);

		}

		else if (collision.gameObject.tag == "Brainz")
		{
			gameState.brainz++;
			gameState.RemoveObject(collision.gameObject);
		} 
 
		else if (collision.gameObject.tag == "Gainz")
		{
			gameState.gainz++;
			Destroy (collision.gameObject);
		} 

		else if (collision.gameObject.tag == "Powerup")
		{
			playerState.DeletePowerup();

			if (collision.gameObject.name == "PowerupBlink(Clone)") 
			{
				playerState.SetPowerUp(PowerUp.BLINK);
			} 

			else if(collision.gameObject.name == "PowerupInvuln(Clone)") 
			{
				playerState.SetPowerUp(PowerUp.INVULNERABLE);
			} 

			else if(collision.gameObject.name == "PowerupInvis(Clone)") 
			{
				playerState.SetPowerUp(PowerUp.INVISIBILITY);
			} 

			else if(collision.gameObject.name == "PowerupSecondWind(Clone)")
			{
				playerState.SetPowerUp(PowerUp.SECOND_WIND);
			}

			gameState.RemoveObject(collision.gameObject);

		} 
		else if (collision.gameObject.tag == "SceneChanger")
		{
			gameState.ChangeScene(collision.gameObject.name);
		} 
		else if (collision.gameObject.tag == "Item"){
			if (collision.gameObject.name == "Extinguisher"){
				gameState.AddItem(Item.FIRE_EXTINGUISHER);
				Destroy (collision.gameObject);
			} else if (collision.gameObject.name == "Brain"){
				gameState.AddItem (Item.BRAINZ);
				Destroy (collision.gameObject);
			}
		}
	}
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
