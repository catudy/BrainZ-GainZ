/// <summary>
/// Controls Enemy AI.  Current Behavior is to just chase player.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public float aggro_range = 5.0f; // range in feet at which enemy will aggro
	public float max_speed = 2.0f; // maximum speed that enemy can have.
	public float acceleration = 1.0f; 
	private Vector3 velocity = new Vector3(0,0,0); // current enemy velocity.
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 player_pos = player.transform.position;
		Vector3 enemy_pos = transform.position;
		if (IsAggroed(player_pos,enemy_pos)) {
			velocity = velocity + ((player_pos - enemy_pos).normalized)*acceleration;
			if(velocity.magnitude > max_speed){ // if higher than max speed, set velocity to max speed
				velocity = velocity.normalized * max_speed;
			}
		} else {
			velocity.x = 0;
			velocity.z = 0;
		}
		if (velocity.magnitude > 0) {
			CharacterController cc = GetComponent<CharacterController> ();
			cc.SimpleMove (velocity);
		}
	}

	private bool IsAggroed(Vector3 player_pos, Vector3 enemy_pos){
		float mod_aggro_range = aggro_range;
		if (player.GetComponent<PlayerState> ().power_up == PowerUp.INVISIBILITY) {
			mod_aggro_range = 0.0f;
		}
		else if (player.GetComponent<PlayerState> ().GetSneaking()) {
			mod_aggro_range = aggro_range / 2;
		} else if (player.GetComponent<PlayerState> ().GetRunning()) {
			mod_aggro_range = aggro_range * 2;
		}
		return ((player_pos - enemy_pos).magnitude < mod_aggro_range);
	}
}
