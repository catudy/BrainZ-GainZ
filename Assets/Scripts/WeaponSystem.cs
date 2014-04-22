using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour {

	private int currentWeapon = 0;
	private int maxWeapon = 5;
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
	public float meleeAttackSpeed;
	public float meleeAttackSpeed_base;
	public int meleeAttackSpeed_level;
	public float meleeAttackSpeed_max;
	public int gunAmmo;
	public int gunAmmo_base;
	public int gunAmmo_level;
	public int gunAmmo_max;
	public float fireRate;
	public float fireRate_base;
	public int fireRate_level;
	public float fireRate_max;
	public int pulseAmmo;
	public int pulseAmmo_base;
	public int pulseAmmo_level;
	public int pulseAmmo_max;



	// Use this for initialization
	void Start () {

		//initialize base stats
		meleeAttackSpeed_base = 1.0f;
		gunAmmo_base = 10;
		fireRate_base = 0.5f;
		pulseAmmo_base = 3;

		//initialize stat levels
		meleeAttackSpeed_level = 1;
		gunAmmo_level = 1;
		fireRate_level = 1;
		pulseAmmo_level = 1;

		//set base as max
		meleeAttackSpeed_max = meleeAttackSpeed_base;
		gunAmmo_max = gunAmmo_base;
		fireRate_max = fireRate_base;
		pulseAmmo_max = pulseAmmo_base;

		//set max to actual
		meleeAttackSpeed = meleeAttackSpeed_max;
		gunAmmo = gunAmmo_max;
		fireRate = fireRate_max;
		pulseAmmo = pulseAmmo_max;

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
			if(currentWeapon == 0)
			{
				melee.animation.Play();
				if(melee.animation.isPlaying)
				{
					melee.collider.enabled = true;
				}
				StartCoroutine(WaitForAttack (meleeAttackSpeed));
			}
			if(currentWeapon == 1)
			{
				if(gunAmmo > 0)
				{
					Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
					gunAmmo--;
					StartCoroutine(WaitForAttack(fireRate));
				}
			}
			if(currentWeapon == 2)
			{
				if(pulseAmmo > 0)
				{
					pulse.animation.Play ();
					pulseAmmo--;
					if(pulse.animation.isPlaying)
					{
						pulse.collider.enabled = true;
					}
					StartCoroutine(WaitForAttack(1.0f));
				}
			}
		}
		DisableAttackCollider ();
	
	}

	void UpgradeMeleeAttackSpeed()
	{
		meleeAttackSpeed_max = meleeAttackSpeed_base - (meleeAttackSpeed_level-1 * 0.2f);
	}

	void UpgradeGunAmmo()
	{
		gunAmmo_max = gunAmmo_base + (gunAmmo_level - 1 * 5);
	}

	void UpgradeFireRate()
	{
		fireRate_max = fireRate_base - (fireRate_level - 1 * 0.1f);
	}

	void UpgradePulseAmmo()
	{
		pulseAmmo_max = pulseAmmo_base + (pulseAmmo_level - 1 * 1);
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
			gun.SetActive(false);
			pulse.SetActive(false);
			flamethrower.SetActive(false);
			extinguisher.SetActive(false);
		}
		if(currentWeapon == 1) {
			melee.SetActive(false);
			gun.SetActive(true);
			pulse.SetActive(false);
			flamethrower.SetActive(false);
			extinguisher.SetActive (false);
		}
		if(currentWeapon == 2) {
			melee.SetActive(false);
			gun.SetActive(false);
			pulse.SetActive(true);
			flamethrower.SetActive(false);
		}
		if(currentWeapon == 3) {
			melee.SetActive(false);
			gun.SetActive(false);
			pulse.SetActive(false);
			flamethrower.SetActive(true);
			extinguisher.SetActive(false);
		}
		if (currentWeapon == 4) {
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
}
