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
	private UpgradeGUI upgradeGUI;
	GameState gameState;
	public AudioClip pickupSound;

	void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "NoCollision") 
		{ // Don't care about the ground.
			return;
		} 
		else if (collision.gameObject.tag == "Deadly" || collision.gameObject.tag == "Fire") 
		{ 
			if(playerState.power_up == PowerUp.INVULNERABLE)
			{ 
				// Fuck you Zambie
				gameState.RemoveObject(collision.gameObject);
				//gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			} 
			else 
			{
				playerState.DealDamage(1);
				//if(collision.gameObject.tag == "Fire")
				//{
				//	Destroy(collision.gameObject);
				//}
			}
		} 
		//else if (collision.gameObject.tag == "Projectile"){
			//gameState.game_over = true;
			//collision.gameObject.GetComponent<Projectile>().DestroyWithExplosion(collision.gameObject);
		//}


		else if(collision.gameObject.tag == "Brainz")
		{
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
			Destroy(collision.gameObject);
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			upgradeGUI.collected_brains+=10;
			gameState.brainz += 10;
			
		}

		else if(collision.gameObject.tag == "Gainz")
		{
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
			Destroy(collision.gameObject);
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			upgradeGUI.collected_gains+=10;
			gameState.gainz += 10;
		}

		else if(collision.gameObject.tag == "ammopack")
		{
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
			Destroy(collision.gameObject);
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			weaponSystem.gunAmmo = weaponSystem.gunAmmo_max;
			weaponSystem.pulseAmmo = weaponSystem.pulseAmmo_max;
			weaponSystem.feAmmo = weaponSystem.feAmmo_max;
			weaponSystem.flameAmmo = weaponSystem.flameAmmo_max;
		}

		else if(collision.gameObject.tag == "healthpack")
		{
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
			Destroy(collision.gameObject);
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			if(playerState.health >= playerState.playerStats.max_health)
			{
				playerState.health = playerState.playerStats.max_health;
			}
			else
			{
				playerState.health += 1;
			}
		}

		else if(collision.gameObject.tag == "gun")
		{
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
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
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
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
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
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
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
			Destroy(collision.gameObject);
			weaponSystem.activeWeaponList[4] = true;
			gameState.UpdateObjective(ObjectiveType.SCAVENGER,1.0f);
			if(weaponSystem.activeWeaponList[4] == true)
			{
				weaponSystem.feAmmo = weaponSystem.feAmmo_max;
			}
		}

		else if (collision.gameObject.tag == "Powerup") {
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
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

		else if(collision.gameObject.tag == "meteor")
		{
			playerState.DealDamage(100);
		}

			 
	}
	
	// Use this for initialization
	void Start () 
	{
		playerState = GetComponent<PlayerState>();
		weaponSystem = GetComponent<WeaponSystem> ();
		gameState = GameObject.Find("GameController").GetComponent<GameState>();
		upgradeGUI = GameObject.Find("GUIController").GetComponentInChildren<UpgradeGUI>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
