using UnityEngine;
using System.Collections;

public class ShooterAI : MonoBehaviour {
	public GameObject projectile;
	public float cooldown = 1.0f;
	private GameObject player;
	private float fire_next_rocket = 0.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > fire_next_rocket){
			GameObject rocket = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
			Destroy (rocket, cooldown*5);
			fire_next_rocket = Time.time + cooldown;
		}
	}
}
