/// <summary>
/// Controls Brainz and Gainz AI.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class BnGAI : MonoBehaviour {
	public float flee_range = 5.0f;
	public float max_speed = 2.0f;
	public float acceleration = 1.0f;
	public Vector3 velocity = new Vector3(0,0,0);
	private GameObject player;
	public bool fleeing = false;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 player_pos = player.transform.position;
		Vector3 object_pos = transform.position;
		if ((player_pos - object_pos).magnitude < flee_range) {
			fleeing = true;
		}
		if(fleeing){
			velocity = velocity + ((object_pos-player_pos).normalized)*acceleration;
			if(velocity.magnitude > max_speed){ // if higher than max speed, set velocity to max speed
				velocity = velocity.normalized * max_speed;
			}
		}
		if (velocity.magnitude > 0) {
			CharacterController cc = GetComponent<CharacterController> ();
			cc.SimpleMove (velocity);
		}
	}
}
