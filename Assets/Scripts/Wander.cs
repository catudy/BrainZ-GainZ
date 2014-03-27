using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour {
	public float wander_distance = 10.0f;
	public float wander_speed = 1.0f;
	private float time_remaining;
	private Vector3 target_destination;
	private Vector3 movement_vector;
	CharacterController cc;

	// Use this for initialization
	void Start () {
		SetRandomDestination ();	
		cc = GetComponent<CharacterController> ();
		time_remaining = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		time_remaining -= Time.deltaTime;
		movement_vector = target_destination - transform.position;
		movement_vector.y = 0;
		if(time_remaining < 0.0f || movement_vector.magnitude < wander_speed * Time.deltaTime){
			SetRandomDestination();
		} else {
			movement_vector.Normalize();
			Vector3 velocity = movement_vector * wander_speed;
			cc.SimpleMove (velocity);
		}
	}

	// Sets the current_destination member variable for class.
	private void SetRandomDestination(){
		Vector2 dest = Random.insideUnitCircle * wander_distance;
		Vector3 current_pos = transform.position;
		target_destination.Set (current_pos.x + dest.x, current_pos.y, current_pos.z + dest.y);
	}
}
