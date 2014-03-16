using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public GameObject enemy;
	public float aggro_range = 5.0f;
	public float max_speed = 2.0f;
	public float acceleration = 1.0f;
	public Vector3 velocity = new Vector3(0,0,0);
	private GameObject player = GameObject.Find("Player");
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 player_pos = player.transform.position;
		Vector3 enemy_pos = enemy.transform.position;
		if ((player_pos - enemy_pos).magnitude < aggro_range) {
			velocity = velocity + ((player_pos - enemy_pos).normalized)*acceleration;
			if(velocity.magnitude > max_speed){ // if higher than max speed, set velocity to max speed
				velocity = velocity.normalized * max_speed;
			}
		} else {
			velocity.Set(0,0,0);
		}
		if (velocity.magnitude > 0) {
			CharacterController cc = GetComponent<CharacterController> ();
			cc.SimpleMove (velocity);
		}
	}
}
