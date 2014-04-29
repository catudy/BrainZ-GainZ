/// <summary>
/// Controls Enemy AI.  Current Behavior is to just chase player.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
	public float aggro_range = 5.0f; // range in feet at which enemy will aggro
	public float speed = 2.0f; // maximum speed that enemy can have.
	public float turn_rate = 15.0f; // degrees per second
	public bool aggro;
	public Vector3 velocity = new Vector3(0,0,0); // current enemy velocity.
	private Vector3 target_pos;
	private GameObject player;
	private CharacterController cc;
	private Wander wander;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		cc = GetComponent<CharacterController> ();
		wander = GetComponent<Wander> ();
	}
	
	// Update is called once per frame
	void Update () {
		aggro = IsAggroed (player.transform.position, transform.position);
		UpdateZombieMovement ();
		MoveZombie ();
	}

	// Updates velocity component
	private void UpdateZombieMovement(){
		if(aggro) {
			wander.enabled = false;
			Quaternion target_rotation = Quaternion.LookRotation(player.transform.position - transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, Time.deltaTime*turn_rate);
			velocity = transform.forward.normalized * speed;
			velocity.y = 0;
		} else {
			wander.enabled = true;
		}
	}

	// Moves and Updates zombie facing direction.
	private void MoveZombie(){
		if (velocity.magnitude > 0) 
		{
			// Face zambie in right direction and move
			cc.SimpleMove (velocity*Time.deltaTime);
		}
	}



	private bool IsAggroed(Vector3 player_pos, Vector3 enemy_pos)
	{
		float mod_aggro_range = aggro_range;

		if (player.GetComponent<PlayerState> ().power_up == PowerUp.INVISIBILITY) 
		{
			mod_aggro_range = 0.0f;
		}
		return ((player_pos - enemy_pos).magnitude < mod_aggro_range);
	}
}
