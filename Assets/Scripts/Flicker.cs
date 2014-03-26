using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour {
	private float random_offset;
	// Use this for initialization
	void Start () {
		random_offset = Random.value;
	}
	
	// Update is called once per frame
	void Update () {
		light.intensity = Mathf.Sin (Time.time + random_offset) + 3.0f;
	}
}
