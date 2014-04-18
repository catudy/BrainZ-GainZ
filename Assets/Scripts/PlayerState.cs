using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float max_health;
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
	public float health;
	private bool sneaking = false;
	public bool running = false;
	public PlayerStats playerStats;
	private float cooldown  = 0.0f;
	private float damage_cooldown = 0.0f;
	private GameState gameState;

	//weapon vars TODO: Update this to be an enumerable array.
	private int currentWeapon = 0;
	private int maxWeapon = 4;
	public GameObject melee;
	public GameObject wep1;
	public GameObject wep2;
	public GameObject wep3;

	public bool[] activeWeaponList; // TODO: Rename and reimplement to remove warning.
	public bool canAttack = true;

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

		activeWeaponList [0] = true;
		//melee.collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//No updates if the game is paused
		if(gameState.paused)
			return;

		UpdateTimers ();
		UpdateCooldowns ();
		UpdateStamina ();

		if (cooldown <= 0.0f) {
			ProcessAbilityInput();
		}

		SetWeapon ();

		if(Input.GetKeyDown("v"))
		{
			Swap ();
			Debug.Log (currentWeapon);
		}

		//melee attack
		if (Input.GetKey("a") && canAttack) 
		{
			if(currentWeapon == 0)
			{
				melee.animation.Play();
				if(melee.animation.isPlaying)
				{
					melee.collider.enabled = true;
				}
				//StartCoroutine(Wait ());
			}
		}
		if(!melee.animation.isPlaying)
		{
			//melee.collider.enabled = false;
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
			GameObject.Find("Bip001 Pelvis").GetComponent<SkinnedMeshRenderer>().enabled = true;
		} 
		else if (power_up == PowerUp.INVULNERABLE) 
		{
//			GetComponent<ParticleSystem>().enableEmission = false;
		}
		power_up = PowerUp.NONE;
		power_up_time_remaining = 0.0f;
	}

	public void SetPowerUp(PowerUp set)
	{
		power_up = set;
		power_up_time_remaining = 10.0f;
		if (power_up == PowerUp.INVISIBILITY) {
			GameObject.Find ("Bip001 Pelvis").GetComponent<SkinnedMeshRenderer> ().enabled = false;
		} 
		else if (power_up == PowerUp.INVULNERABLE) {
	//		GetComponent<ParticleSystem>().enableEmission = true;
		} 
	}

	void Swap() { 
		int current = currentWeapon;
		
		if (current+1 == maxWeapon) {
			currentWeapon = 0;
		} else {
			while(current+1 < maxWeapon) {
				current++;
				if(activeWeaponList[current] == true) {
					currentWeapon = current;
					break;
				}
				else if(current+1 == maxWeapon) {
					currentWeapon = 0;
					break;
				}
			}
		}
	}
	
	void SetWeapon() {
		//TODO: Wep1,Wep2,Wep3 need to go into an array.
		if(currentWeapon == 0) {
			melee.SetActive(true);
			wep1.SetActive(false);
			wep2.SetActive(false);
			wep3.SetActive(false);
		}
		if(currentWeapon == 1) {
			melee.SetActive(false);
			wep1.SetActive(true);
			wep2.SetActive(false);
			wep3.SetActive(false);
		}
		if(currentWeapon == 2) {
			melee.SetActive(false);
			wep1.SetActive(false);
			wep2.SetActive(true);
			wep3.SetActive(false);
		}
		if(currentWeapon == 3) {
			melee.SetActive(false);
			wep1.SetActive(false);
			wep2.SetActive(false);
			wep3.SetActive(true);
		}
	}

	IEnumerator Wait() {
		canAttack = false;
		yield return new WaitForSeconds(1.0f);
		canAttack = true;
	}

	public void SetSneaking() {
		sneaking = true;
		running = false;
	}

	public bool GetSneaking() {
		return sneaking || stamina < 0.5f;
	}

	public void SetRunning() {
		running = stamina > 0.5f;
		sneaking = false;
	}

	public bool GetRunning() {
		return running;
	}

	public void SetWalking() {
		running = false;
		sneaking = false;
	}

	public bool GetWalking() {
		return (!running & !sneaking);
	}

	public float GetStaminaPercent() {
		return stamina / playerStats.max_stamina;
	}

	public float GetHealthPercent() {
		return health / playerStats.max_health;
	}

	public float GetPowerupPercent() {
		return power_up_time_remaining / 10f;
	}

	public float GetHealth() {
		return health;
	}

	public void DealDamage(int damage){
		if(Time.time > damage_cooldown){
			health -= damage;
			if(health <= 0){
				gameState.game_over = true;
			}
			gameState.UpdateObjective (ObjectiveType.DAMAGE, 1.0f);
			damage_cooldown = Time.time + 1.0f;
		}
	}

	public void HealDamage(int damage){
		health += damage;
	}

	public void UpdateMaxHealth() {
		playerStats.max_health = playerStats.base_health + (playerStats.health_level-1);
	}
	public void UpdateMaxStamina() {
		playerStats.max_stamina = playerStats.base_stamina + (playerStats.stamina_level-1);
	}
	public void UpdateMaxWeaponDamage(){
		playerStats.weapon_damage = playerStats.base_wepon_damage + (0.5f*playerStats.weapon_level-0.5f);
	}
}
