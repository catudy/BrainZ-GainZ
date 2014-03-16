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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetPowerUp(PowerUp set){
		power_up = set;
	}
}
