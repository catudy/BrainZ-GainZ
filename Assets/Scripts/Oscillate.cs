/// <summary>
/// Oscillates Game Object around its start point
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class Oscillate : MonoBehaviour {
	public float oscillation_length = 1.0f;
	public float spin_frequency = 1.0f;
	private float start_y;
	// Use this for initialization
	void Start () {
		start_y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x,
		                                 start_y + Mathf.Sin (Time.time) * oscillation_length,
		                                 transform.position.z);
		transform.Rotate (0, 1, 0);
	}
}
