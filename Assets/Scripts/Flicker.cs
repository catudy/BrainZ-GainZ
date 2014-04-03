using UnityEngine;
using System.Collections;

public class Flicker : MonoBehaviour 
{
	private float random_wait;
	// Use this for initialization
	void Start () 
	{
		random_wait = Random.value * 5.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		random_wait -= Time.deltaTime;
		
		if (random_wait < 0.0f) 
		{
			light.intensity = Random.value * 2.0f;
		}

		random_wait = Random.value * 5.0f;
	}
}
