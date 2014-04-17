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
	public float max_speed = 2.0f; // maximum speed that enemy can have.
	public float acceleration = 1.0f; 
	public bool aggro;
	private Vector3 velocity = new Vector3(0,0,0); // current enemy velocity.
	private Vector3 target_pos;
	private GameObject player;
	private CharacterController cc;
	private Wander wander;
	private GameState gameState;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
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
			wander.enabled = true;
			velocity = velocity + ((player.transform.position - transform.position).normalized)*acceleration;
			velocity.y = 0;
			if(velocity.magnitude > max_speed ){
				velocity = velocity.normalized * max_speed;
			}
		} else {
			wander.enabled = false;
		}
	}

	// Moves and Updates zombie facing direction.
	private void MoveZombie(){
		if (velocity.magnitude > 0) 
		{
			// Face zambie in right direction
			Vector3 look = new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z);
			transform.LookAt(look);
			velocity.y = 0;
			// Move Zombies
			cc.SimpleMove (velocity);
		}
	}

	private bool IsAggroed(Vector3 player_pos, Vector3 enemy_pos)
	{
		float mod_aggro_range = aggro_range;

		if (player.GetComponent<PlayerState> ().power_up == PowerUp.INVISIBILITY) 
		{
			mod_aggro_range = 0.0f;
		}

		else if (player.GetComponent<PlayerState> ().GetSneaking()) 
		{
			mod_aggro_range = aggro_range / 2;
		} 

		else if (player.GetComponent<PlayerState> ().GetRunning()) 
		{
			mod_aggro_range = aggro_range * 2;
		}
		return ((player_pos - enemy_pos).magnitude < mod_aggro_range);
	}
}
