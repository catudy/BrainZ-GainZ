using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Vector3 speed;
	private Vector3 target;
	private float start_time;
	private Vector3 start; 

	// Use this for initialization
	void Start () {
		speed.Set (1, 1, 1);
		target = GameObject.Find("Player").transform.position;
		start = transform.position;
		start_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (start, target, (Time.time-start_time)/(5.0f));
	}
}
