using UnityEngine;
using System.Collections;

public class ShooterAI : MonoBehaviour {
	public GameObject projectile;
	public float cooldown = 5.0f;
	private float fire_next_rocket = 0.0f;

	//ADDED
	public float max_speed = 2.0f; // maximum speed that enemy can have.
	public float acceleration = 1.0f; 
	public bool aggro;
	private Vector3 target_pos;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		fire_next_rocket = Time.time + cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		aggro = IsAggroed (player.transform.position, transform.position);

		if(Time.time > fire_next_rocket && aggro){
			Vector3 SpawnAt = new Vector3(transform.position.x, 0.8f, transform.position.z);
			GameObject rocket = (GameObject)Instantiate(projectile, SpawnAt, transform.rotation);
			Destroy (rocket, cooldown*15.0f);
			fire_next_rocket = Time.time + cooldown;
		}
	}

	private bool IsAggroed(Vector3 player_pos, Vector3 enemy_pos)
	{
		Vector3 diff = player_pos - enemy_pos;
		float distance = diff.magnitude;
		if(distance > 2.0f){
			if (Physics.Raycast (transform.position, diff, distance - 2.0f)) {
				return false;
			} 
		}
		return true;
	}
}
