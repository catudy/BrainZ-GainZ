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
	private WeaponSystem weaponSystem;
	GameState gameState;

	void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "NoCollision") { // Don't care about the ground.
			return;
		} else if (collision.gameObject.tag == "Deadly" || collision.gameObject.tag == "Fire") { 
			// Game over if you run into something deadly
			if(playerState.power_up == PowerUp.INVULNERABLE){ 
				// Fuck you Zambie
				gameState.RemoveObject(collision.gameObject);
				gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			} else {
				playerState.DealDamage(1);
				Debug.Log ("Took Damage from " + collision.gameObject.name);
				if(collision.gameObject.tag == "Fire")
				{
					Destroy(collision.gameObject);
				}
			}
		} //else if (collision.gameObject.tag == "Projectile"){
			//gameState.game_over = true;
			//collision.gameObject.GetComponent<Projectile>().DestroyWithExplosion(collision.gameObject);
		//}


		else if(collision.gameObject.tag == "gun")
		{
			Destroy(collision.gameObject);
			weaponSystem.activeWeaponList[1] = true;
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			if(weaponSystem.activeWeaponList[1] == true)
			{
				weaponSystem.gunAmmo = weaponSystem.gunAmmo_max;
			}
		}

		else if(collision.gameObject.tag == "pulse")
		{
			Destroy(collision.gameObject);
			weaponSystem.activeWeaponList[2] = true;
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			if(weaponSystem.activeWeaponList[2] == true)
			{
				weaponSystem.pulseAmmo = weaponSystem.pulseAmmo_max;
			}
		}

		else if(collision.gameObject.tag == "flame")
		{
			Destroy(collision.gameObject);
			weaponSystem.activeWeaponList[3] = true;
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			if(weaponSystem.activeWeaponList[3] == true)
			{
				weaponSystem.flameAmmo = weaponSystem.flameAmmo_max;
			}
		}

		else if(collision.gameObject.tag == "extinguish")
		{
			Destroy(collision.gameObject);
			weaponSystem.activeWeaponList[4] = true;
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			if(weaponSystem.activeWeaponList[4] == true)
			{
				weaponSystem.feAmmo = weaponSystem.feAmmo_max;
			}
		}

		else if (collision.gameObject.tag == "Powerup") {
			// Takes current powerup away
			playerState.DeletePowerup();
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
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
	}
	
	// Use this for initialization
	void Start () 
	{
		playerState = GetComponent<PlayerState>();
		weaponSystem = GetComponent<WeaponSystem> ();
		gameState = GameObject.Find("GameController").GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
