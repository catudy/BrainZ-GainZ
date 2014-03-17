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
	public bool sneaking = false;
	public bool running = false;
	private float cooldown  = 0.0f;
	// Use this for initialization
	void Start () {
	
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
	public void SetPowerUp(PowerUp set){
		power_up = set;
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
