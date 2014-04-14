using UnityEngine;
using System.Collections;

public class ShooterAI : MonoBehaviour {
	public GameObject projectile;
	public float cooldown = 5.0f;
	private float fire_next_rocket = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > fire_next_rocket){
			Vector3 SpawnAt = new Vector3(transform.position.x, 0.8f, transform.position.z);
			GameObject rocket = (GameObject)Instantiate(projectile, SpawnAt, transform.rotation);
			Destroy (rocket, cooldown*15.0f);
			fire_next_rocket = Time.time + cooldown;
		}
	}
}
