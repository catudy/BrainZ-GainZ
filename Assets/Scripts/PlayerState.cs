using UnityEngine;
using System.Collections;

public enum PowerUp{
	NONE,
	BLINK,
	INVISIBILITY,
	STUN,
	SECOND_WIND,
	INVULNERABLE
};

public class PlayerState : MonoBehaviour {
	public PowerUp power_up = PowerUp.NONE;
	public float power_up_time_remaining = 0.0f;
	public float stamina_recovery_rate = 1.0f; // how much stamina you recover per WaitForSeconds.
	public float max_stamina = 5.0f; 
	public float stamina; // How much stamina you currently have
	
	private bool sneaking = false;
	public bool running = false;
	
	private float cooldown  = 0.0f;

	// Use this for initialization
	void Start () {
		stamina = max_stamina;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTimers ();
		UpdateCooldowns ();
		UpdateStamina ();

		if (cooldown <= 0.0f) {
			ProcessAbilityInput();
		}
	}

	void UpdateTimers(){
		UpdatePowerupTimer ();
		UpdateCooldowns ();

	}

	void UpdatePowerupTimer(){
		if (power_up != PowerUp.NONE) { // Update powerup timer
			power_up_time_remaining = power_up_time_remaining - Time.deltaTime;
			if (power_up_time_remaining < 0.0f) {
				DeletePowerup();
			}
		}
	}

	void UpdateCooldowns(){
		if (cooldown > 0.0f) { // Update Ability Cooldowns
				cooldown -= Time.deltaTime;
		}
	}

	void UpdateStamina(){
		if(running){
			stamina -= Time.deltaTime;
		} else {
			stamina += Time.deltaTime * stamina_recovery_rate;
		}
		if(stamina > max_stamina){
			stamina = max_stamina;
		}
	}

	void ProcessAbilityInput(){ // TODO: Move to input handler
		bool ability = Input.GetButton ("Ability");
		if (ability) {
			if (power_up == PowerUp.BLINK) {
				Vector3 Blink = new Vector3 (0, 0, 50);
				Vector3 Temp = transform.position + transform.rotation * Blink;
				if (Temp.y > 0) {
					transform.position = Temp;
					cooldown = 5.0f;
				}
			}
		}
	}

	public void DeletePowerup(){
		if (power_up == PowerUp.INVISIBILITY) {
			GameObject.Find("char_ethan_body").GetComponent<SkinnedMeshRenderer>().enabled = true;
		} else if (power_up == PowerUp.INVULNERABLE) {
			GetComponent<ParticleSystem>().enableEmission = false;
		}
		power_up = PowerUp.NONE;
		power_up_time_remaining = 0.0f;
	}

	public void SetPowerUp(PowerUp set){
		power_up = set;
		power_up_time_remaining = 15.0f;
		if (power_up == PowerUp.INVISIBILITY) {
			GameObject.Find ("char_ethan_body").GetComponent<SkinnedMeshRenderer> ().enabled = false;
		} else if (power_up == PowerUp.INVULNERABLE) {
			GetComponent<ParticleSystem>().enableEmission = true;
		} 
	}

	public void SetSneaking(){
		sneaking = true;
		running = false;
	}

	public bool GetSneaking(){
		return sneaking;
	}

	public void SetRunning(){
		running = stamina > 0.0f;
		sneaking = false;
	}

	public bool GetRunning(){
		return running;
	}

	public void SetWalking(){
		running = false;
		sneaking = false;
	}

	public bool GetWalking(){
		return (!running & !sneaking);
	}

	public float GetStaminaPercent(){
		return stamina / max_stamina;
	}
}
