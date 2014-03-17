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
	public bool sneaking = false;
	public bool running = false;
	private float cooldown  = 0.0f;
	// Use this for initialization
	void Start () {
		if (power_up != PowerUp.NONE) {
				power_up_time_remaining -= Time.deltaTime;
				if (power_up_time_remaining < 0.0f) {
						DeletePowerup();
				}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldown > 0.0f) {
			cooldown -= Time.deltaTime;
		} else {
				bool ability = Input.GetButton ("Ability");
				if (ability) {
					if (power_up == PowerUp.BLINK) {
						Vector3 Blink = new Vector3(0,0,50);
						Vector3 Temp = transform.position + transform.rotation * Blink;
						if(Temp.y > 0){
							transform.position = Temp;
							cooldown = 5.0f;
						}
					}
				}
		}
	}
	public void DeletePowerup(){
		if (power_up == PowerUp.INVISIBILITY) {
			GameObject.Find("char_ethan_body").GetComponent<SkinnedMeshRenderer>().enabled = true;
		}
		power_up = PowerUp.NONE;
		power_up_time_remaining = 0.0f;
	}

	public void SetPowerUp(PowerUp set){
		power_up = set;
		power_up_time_remaining = 15.0f;
		if (power_up == PowerUp.INVISIBILITY) {
			GameObject.Find("char_ethan_body").GetComponent<SkinnedMeshRenderer>().enabled = false;
		}
	}

	public void SetSneaking(){
		sneaking = true;
		running = false;
	}

	public void SetRunning(){
		running = true;
		sneaking = false;
	}
}
