using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Weapon{
	MELEE,
	GUN,
	PULSE,
	FLAMER,
	EXTINGUISHER
}

public class WeaponSystem : MonoBehaviour {

	private GameObject player;
	private GameState gameState;

	private GameObject flamer;
	private ParticleSystem fire_ex;
	

	public Weapon currentWeapon = Weapon.MELEE;
	private int maxWeapon = Weapon.GetNames(typeof(Weapon)).Length;
	public GameObject melee;
	public GameObject gun;
	public GameObject pulse;
	public GameObject flamethrower;
	public GameObject extinguisher;
	
	public GameObject bullet;
	public Transform bulletSpawn;
	
	public bool[] activeWeaponList;
	public bool canAttack = true;

	//upgrade variables
	public int weaponLevelCap = 5;

	//melee varaibles
	public float meleeAttackSpeed;
	public float meleeAttackSpeed_base;
	public int meleeAttackSpeed_level;
	public float meleeAttackSpeed_max;

	//gun variables
	public int gunAmmo;
	public int gunAmmo_base;
	public int gunAmmo_level;
	public int gunAmmo_max;
	public float fireRate;
	public float fireRate_base;
	public int fireRate_level;
	public float fireRate_max;

	//pulse variables
	public int pulseAmmo;
	public int pulseAmmo_base;
	public int pulseAmmo_level;
	public int pulseAmmo_max;
	public int pulse_radius;
	public int pulse_radius_base;
	public int pulse_radius_level;
	public int pulse_radius_max;

	//flamethrower variables
	public float flameAmmo;
	public float flameAmmo_base;
	public float flameAmmo_level;
	public float flameAmmo_max;

	//fire extinguisher variables
	public float feAmmo;
	public float feAmmo_base;
	public float feAmmo_level;
	public float feAmmo_max;



	// Use this for initialization
	void Start () {

		player = GameObject.Find ("Player");
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();
		flamer = GameObject.Find ("FlameEmission");
		fire_ex = GameObject.Find ("FireExtinguisherParticleEffect").GetComponent<ParticleSystem> ();

		//initialize base stats
		meleeAttackSpeed_base = 1.0f;
		gunAmmo_base = 10;
		fireRate_base = 0.5f;
		pulseAmmo_base = 3;
		flameAmmo_base = 5.0f;
		feAmmo_base = 5.0f;
		pulse_radius_base = 50;

		//initialize stat levels
		meleeAttackSpeed_level = 1;
		gunAmmo_level = 1;
		fireRate_level = 1;
		pulseAmmo_level = 1;
		flameAmmo_level = 1;
		feAmmo_level = 1;
		pulse_radius_level = 1;

		//set base as max
		meleeAttackSpeed_max = meleeAttackSpeed_base;
		gunAmmo_max = gunAmmo_base;
		fireRate_max = fireRate_base;
		pulseAmmo_max = pulseAmmo_base;
		flameAmmo_max = flameAmmo_base;
		feAmmo_max = feAmmo_base;
		pulse_radius_max = pulse_radius_base;

		//set max to actual
		meleeAttackSpeed = meleeAttackSpeed_max;
		gunAmmo = gunAmmo_max;
		fireRate = fireRate_max;
		pulseAmmo = pulseAmmo_max;
		flameAmmo = flameAmmo_max;
		feAmmo = feAmmo_max;
		pulse_radius = pulse_radius_max;

		activeWeaponList [0] = true;
		melee.collider.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		SetWeapon ();

		if(Input.GetKeyDown("s"))
		{
			Swap ();
			Debug.Log (currentWeapon);
		}

		//melee attack
		if (Input.GetKey("a") && canAttack) 
		{
			//melee attack
			if(currentWeapon == Weapon.MELEE)
			{
				melee.animation.Play();
				if(melee.animation.isPlaying)
				{
					melee.collider.enabled = true;
				}
				StartCoroutine(WaitForAttack (meleeAttackSpeed));
			}

			//gun attack
			if(currentWeapon == Weapon.GUN)
			{
				if(gunAmmo > 0)
				{
					Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
					gunAmmo--;
					StartCoroutine(WaitForAttack(fireRate));
				}
			}

			//pulse attack
			if(currentWeapon == Weapon.PULSE)
			{
				if(pulseAmmo > 0)
				{
					if(pulse_radius_level == 1)
					{
						pulse.animation.Play ("pulseAttack");
					}
					else if(pulse_radius_level == 2)
					{
						pulse.animation.Play ("pulseAttack_75");
					}
					else if(pulse_radius_level == 3)
					{
						pulse.animation.Play ("pulseAttack_100");
					}
					else if(pulse_radius_level == 4)
					{
						pulse.animation.Play ("pulseAttack_125");
					}
					else if(pulse_radius_level == 5)
					{
						pulse.animation.Play ("pulseAttack_150");
					}

					pulseAmmo--;
					if(pulse.animation.isPlaying)
					{
						pulse.collider.enabled = true;
					}
					StartCoroutine(WaitForAttack(1.0f));
				}
			}

			//flamethrower attack
			if(currentWeapon == Weapon.FLAMER)
			{
				if(flameAmmo > 0.0f)
				{
					RaycastHit hit;
					Vector3 forward = flamethrower.transform.forward;
					Vector3 ray_start = flamethrower.transform.position;
					//ray_start.y += 1.0f;
					//ray_start.z -= 0.1f;
					//ray_start.x += 0.5f;


					flameAmmo -= Time.deltaTime;

					for(float i=-1; i < 1; i=i+0.1f)
					{
						Vector3 ray = new Vector3(forward.x+i,forward.y,forward.z);

						Vector3 temp = flamethrower.transform.position + player.transform.forward.normalized * 2.0f;
						temp.y = 0.5f; 
						flamer.transform.position = temp;
						flamer.transform.rotation = flamethrower.transform.rotation;
						flamer.SetActive(true);// = true;
						if(Physics.Raycast(ray_start,ray,out hit))
						{
							Debug.DrawRay (ray_start,ray);

							if(hit.distance < 5.0f)
							{
								if(hit.collider.gameObject.tag == "Deadly"){
									Destroy (hit.collider.gameObject);
									gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
								}

							}
						}
					}
					if(flameAmmo < 0.0f)
					{
						flameAmmo = 0.0f;
					}
				}
			}
		
			//fire extinguisher attack
			if(currentWeapon == Weapon.EXTINGUISHER)
			{
				if(feAmmo > 0.0f)
				{
					RaycastHit hit;
					Vector3 forward = player.transform.forward;
					Vector3 ray_start = player.transform.position;
					//ray_start.y += 0.0f;
					//ray_start.z -= 0.0f;
					//ray_start.x -= 5.0f;
					feAmmo -= Time.deltaTime;
					
					for(float i=-1; i < 1; i=i+0.1f)
					{
						Vector3 ray = new Vector3(forward.x+i,forward.y,forward.z);
						if(Physics.Raycast(ray_start,ray,out hit))
						{
							Debug.DrawRay (ray_start,ray);
							Vector3 temp = extinguisher.transform.position + player.transform.forward.normalized * 2.0f;
							temp.y = 0.5f;
							fire_ex.transform.position = temp;
							fire_ex.transform.rotation = extinguisher.transform.rotation;
							fire_ex.enableEmission = true;
							if(hit.distance < 5.0f)
							{
								if(hit.collider.gameObject.tag == "Fire"){
									Destroy (hit.collider.gameObject);
									gameState.UpdateObjective(ObjectiveType.FIRE,1.0f);
								}
							}
						}
					}
					if(feAmmo < 0.0f)
					{
						feAmmo = 0.0f;
					}
				}
			}

		}
		else
		{
			fire_ex.enableEmission = false;
			flamer.SetActive(false);
		}
		DisableAttackCollider ();
	
	}

	public void UpgradeMeleeAttackSpeed()
	{
		meleeAttackSpeed_max = meleeAttackSpeed_base - ((meleeAttackSpeed_level-1) * 0.25f);
	}

	public void UpgradeGunAmmo()
	{
		gunAmmo_max = gunAmmo_base + ((gunAmmo_level - 1) * 5);
	}

	public void UpgradeFireRate()
	{
		fireRate_max = fireRate_base - ((fireRate_level - 1) * 0.1f);
	}

	public void UpgradePulseAmmo()
	{
		pulseAmmo_max = pulseAmmo_base + ((pulseAmmo_level - 1) * 1);
	}

	public void UpgradeFlameAmmo()
	{
		flameAmmo_max = flameAmmo_base + ((flameAmmo_level - 1) * 1.0f);
	}

	public void UpgradeFEAmmo()
	{
		feAmmo_max = feAmmo_base + ((feAmmo_level - 1) * 1.0f);
	}

	public void UpgradePulseRadius()
	{
		pulse_radius_max = pulse_radius_base + ((pulse_radius_level - 1) * 25);
	}

	void DisableAttackCollider()
	{
		if(!melee.animation.isPlaying)
		{
			melee.collider.enabled = false;
		}
		if(!pulse.animation.isPlaying)
		{
			pulse.collider.enabled = false;
		}
	}

	void Swap() { 
		Weapon current = currentWeapon;
		
		if ((int)current == maxWeapon) {
			currentWeapon = (Weapon)0;
		} else {
			while((int)current +1 < maxWeapon) {
				current++;
				if(activeWeaponList[(int)current] == true) {
					currentWeapon = current;
					break;
				}
				else if((int)current+1 == maxWeapon) {
					currentWeapon = 0;
					break;
				}
			}
		}
	}
	
	void SetWeapon() {
		if(currentWeapon == Weapon.MELEE) {
			melee.SetActive(true);
			gun.SetActive(false);
			pulse.SetActive(false);
			flamethrower.SetActive(false);
			extinguisher.SetActive(false);
		}
		if(currentWeapon == Weapon.GUN) {
			melee.SetActive(false);
			gun.SetActive(true);
			pulse.SetActive(false);
			flamethrower.SetActive(false);
			extinguisher.SetActive (false);
		}
		if(currentWeapon == Weapon.PULSE) {
			melee.SetActive(false);
			gun.SetActive(false);
			pulse.SetActive(true);
			flamethrower.SetActive(false);
		}
		if(currentWeapon == Weapon.FLAMER) {
			melee.SetActive(false);
			gun.SetActive(false);
			pulse.SetActive(false);
			flamethrower.SetActive(true);
			extinguisher.SetActive(false);
		}
		if (currentWeapon == Weapon.EXTINGUISHER) {
			melee.SetActive(false);
			gun.SetActive(false);
			pulse.SetActive(false);
			flamethrower.SetActive(false);
			extinguisher.SetActive(true);
		}
	}

	IEnumerator WaitForAttack(float waitTime) {
		canAttack = false;
		yield return new WaitForSeconds(waitTime);
		canAttack = true;
	}

	public bool isAmmoMaxed()
	{
		if(gunAmmo==gunAmmo_max && pulseAmmo==pulseAmmo_max && feAmmo==feAmmo_max && flameAmmo==flameAmmo_max)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void maxAmmo()
	{
		gunAmmo = gunAmmo_max;
		pulseAmmo = pulseAmmo_max;
		feAmmo = feAmmo_max;
		flameAmmo = flameAmmo_max;
		return;
	}
}
