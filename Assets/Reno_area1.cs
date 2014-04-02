using UnityEngine;
using System.Collections;

public class Reno_area1 : MonoBehaviour {

	public GameObject area1trigger;
	public bool haskey = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(haskey)
			area1trigger.renderer.material.color = Color.green;
		else
			area1trigger.renderer.material.color = Color.red;
	}
}
