﻿using UnityEngine;
using System.Collections;

public enum PowerUp
{
	NONE,
	BLINK,
	INVISIBILITY,
	STUN,
	SECOND_WIND,
	INVULNERABLE
};

public struct PlayerStats{
	public int base_health;
	public float base_wepon_damage;
	public float base_stamina;
	public int max_health;
	public float weapon_damage;
	public float max_stamina;
	public int health_level;
	public int stamina_level;
	public int weapon_level;
};

public class PlayerState : MonoBehaviour 
{
	public PowerUp power_up = PowerUp.NONE;
	public float power_up_time_remaining = 0.0f;
	public float stamina_recovery_rate = 1.0f; // how much stamina you recover per WaitForSeconds.
	public float stamina; // How much stamina you currently have
	public int health;
	private bool sneaking = false;
	public bool running = false;
	public PlayerStats playerStats;
	
	private float cooldown  = 0.0f;
	private GameState gameState;

	private int weapon = 0;

	public GameObject melee;
	public GameObject gun;

	//Make private later
	public int health_up_bcost = 25;
	public int health_up_gcost = 25;
	public int stamina_up_bcost = 25;
	public int stamina_up_gcost = 25;
	public int weapon_up_bcost = 25;
	public int weapon_up_gcost = 25;

	// Use this for initialization
	void Start () 
	{
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();

		playerStats.base_health = 10;
		playerStats.base_wepon_damage = 1;
		playerStats.base_stamina = 5.0f;

		playerStats.max_health = playerStats.base_health;
		playerStats.max_stamina = playerStats.base_stamina;
		playerStats.weapon_damage = playerStats.base_wepon_damage;

		playerStats.health_level = 1;
		playerStats.stamina_level = 1;
		playerStats.weapon_level = 1;

		stamina = playerStats.max_stamina;
		health = playerStats.max_health;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTimers ();
		UpdateCooldowns ();
		UpdateStamina ();

		if (cooldown <= 0.0f) 
		{
			ProcessAbilityInput();
		}

		if(Input.GetButton("Switch"))
		{
			switch(weapon)
			{
			case 0:
				weapon = 1;
				melee.SetActive(false);
				gun.SetActive(true);
				Debug.Log (weapon);
				break;
			case 1:
				weapon = 0;
				gun.SetActive(false);
				melee.SetActive(true);
				Debug.Log (weapon);
				break;
			}
		}
	}

	void UpdateTimers()
	{
		UpdatePowerupTimer ();
		UpdateCooldowns ();
	}

	void UpdatePowerupTimer()
	{
		if (power_up != PowerUp.NONE) 
		{ 	// Update powerup timer
			power_up_time_remaining = power_up_time_remaining - Time.deltaTime;

			if (power_up_time_remaining < 0.0f) 
			{
				DeletePowerup();
			}
		}
	}

	void UpdateCooldowns()
	{
		if (cooldown > 0.0f) 
		{ // Update Ability Cooldowns
			cooldown -= Time.deltaTime;
		}
	}

	void UpdateStamina()
	{
		if(running)
		{
			stamina -= Time.deltaTime;
			if( stamina < 0.5f)
			{
				stamina = 0.0f;
				running = false;
			}
		} 
		else 
		{
			if(power_up == PowerUp.SECOND_WIND)
			{
				stamina += Time.deltaTime * stamina_recovery_rate * 3;
			} 
			else 
			{
				stamina += Time.deltaTime * stamina_recovery_rate;
			}

			if(stamina > playerStats.max_stamina)
			{
				stamina = playerStats.max_stamina;
			}
		}
	}

	void ProcessAbilityInput()
	{ // TODO: Move to input handler
		bool ability = Input.GetButton ("Ability");
		if (ability) 
		{
			if (power_up == PowerUp.BLINK) 
			{
				Vector3 Blink = new Vector3 (0, 0, 50);
				Vector3 Temp = transform.position + transform.rotation * Blink;

				if (Temp.y > 0) 
				{
					transform.position = Temp;
					cooldown = 5.0f;
				}
			}
		}
	}

	public void DeletePowerup()
	{
		if (power_up == PowerUp.INVISIBILITY) 
		{
			GameObject.Find("char_ethan_body").GetComponent<SkinnedMeshRenderer>().enabled = true;
		} 
		else if (power_up == PowerUp.INVULNERABLE) 
		{
			GetComponent<ParticleSystem>().enableEmission = false;
		}
		power_up = PowerUp.NONE;
		power_up_time_remaining = 0.0f;
	}

	public void SetPowerUp(PowerUp set)
	{
		power_up = set;
		power_up_time_remaining = 15.0f;
		if (power_up == PowerUp.INVISIBILITY) 
		{
			GameObject.Find ("char_ethan_body").GetComponent<SkinnedMeshRenderer> ().enabled = false;
		} 
		else if (power_up == PowerUp.INVULNERABLE) 
		{
			GetComponent<ParticleSystem>().enableEmission = true;
		} 
	}

	public void SetSneaking()
	{
		sneaking = true;
		running = false;
	}

	public bool GetSneaking()
	{
		return sneaking || stamina < 0.5f;
	}

	public void SetRunning()
	{
		running = stamina > 0.5f;
		sneaking = false;
	}

	public bool GetRunning()
	{
		return running;
	}

	public void SetWalking()
	{
		running = false;
		sneaking = false;
	}

	public bool GetWalking()
	{
		return (!running & !sneaking);
	}

	public float GetStaminaPercent()
	{
		return stamina / playerStats.max_stamina;
	}

	public void DealDamage(int damage){
		health -= damage;
		if(health < 0){
			gameState.game_over = true;
		}
		gameState.UpdateObjective (ObjectiveType.DAMAGE, 1.0f);
	}

	public void HealDamage(int damage){
		health += damage;
	}

	public void UpdateMaxHealth()
	{
		playerStats.max_health = playerStats.base_health + (playerStats.health_level-1);
		health_up_bcost = health_up_bcost + (25*playerStats.health_level);
		health_up_gcost = health_up_gcost + (25*playerStats.health_level);
	}
	public void UpdateMaxStamina()
	{
		playerStats.max_stamina = playerStats.base_stamina + (playerStats.stamina_level-1);
		stamina_up_bcost = stamina_up_bcost + (25*playerStats.stamina_level);
		stamina_up_gcost = stamina_up_gcost + (25*playerStats.stamina_level);
	}
	public void UpdateMaxWeaponDamage()
	{
		playerStats.weapon_damage = playerStats.base_wepon_damage + (0.5f*playerStats.weapon_level-0.5f);
		weapon_up_bcost = weapon_up_bcost + (25*playerStats.weapon_level);
		weapon_up_gcost = weapon_up_gcost + (25*playerStats.weapon_level);
	}
}
