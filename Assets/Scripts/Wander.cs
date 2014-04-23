using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour 
{
	public float wander_speed = 10.0f; // how fast zombies wander
	public float wander_rate = 1.0f; // how often zombies wander
	private float time_remaining;
	private Vector3 movement_vector;
	private CharacterController cc;
	
	// Use this for initialization
	void Start () 
	{
		//SetRandomDestination ();	
		cc = GetComponent<CharacterController> ();
		time_remaining = 0.0f;
		movement_vector = transform.forward;
	}

	void Update(){
		time_remaining -= Time.deltaTime;

		if(time_remaining < 0.0f){
			time_remaining = wander_rate;
			SetRandomDestination();
		}
		movement_vector = transform.forward;
		Vector3 velocity = movement_vector.normalized * wander_speed;
		velocity.y = 0;
		cc.SimpleMove (velocity * Time.deltaTime);
	}

	void SetRandomDestination(){
		Vector3 yaxis = new Vector3 (0, 1, 0);
		float angle = Random.Range (-15.0f, 15.0f);
		transform.Rotate (yaxis, angle, Space.Self);
	}
}
