using UnityEngine;
using System.Collections;

public class ShooterAI : MonoBehaviour {
	public GameObject projectile;
	public float cooldown = 5.0f;
	private float fire_next_rocket = 0.0f;

	//ADDED
	public float aggro_range = 15.0f; // range in feet at which enemy will aggro
	public float max_speed = 2.0f; // maximum speed that enemy can have.
	public float acceleration = 1.0f; 
	public bool aggro;
	private Vector3 velocity = new Vector3(0,0,0); // current enemy velocity.
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

		//ADDED
		aggro = IsAggroed (player.transform.position, transform.position);

		if(Time.time > fire_next_rocket && aggro){
			Vector3 SpawnAt = new Vector3(transform.position.x, 0.8f, transform.position.z);
			GameObject rocket = (GameObject)Instantiate(projectile, SpawnAt, transform.rotation);
			Destroy (rocket, cooldown*15.0f);
			fire_next_rocket = Time.time + cooldown;
		}
	}

	//ADDED
	private bool IsAggroed(Vector3 player_pos, Vector3 enemy_pos)
	{
		float mod_aggro_range = aggro_range;
		
		if (player.GetComponent<PlayerState> ().power_up == PowerUp.INVISIBILITY) 
		{
			mod_aggro_range = 0.0f;
		}
		
		//else if (player.GetComponent<PlayerState> ().GetSneaking()) 
		//{
		//	mod_aggro_range = aggro_range / 2;
	//} 
	
	//else if (player.GetComponent<PlayerState> ().GetRunning()) 
	//{
	//	mod_aggro_range = aggro_range * 2;
	//}
	return ((player_pos - enemy_pos).magnitude < mod_aggro_range);
	}
}
