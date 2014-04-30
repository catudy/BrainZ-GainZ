using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {
	public float time = 3.0f;
	private float target_time;
	// Use this for initialization
	void Start () {
		target_time = Time.time + time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > target_time){
			Destroy(this.gameObject);
		}
	}
}
