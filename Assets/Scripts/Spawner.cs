using UnityEngine;
using System.Collections;


public class Spawner : MonoBehaviour {
	public int max_count = 100;
	public float spawn_frequency = 1;
	public GameObject enemy;
	public float min_distance = 0;
	public float max_distance = 0;
	public int counter = 0;
	public GameObject spawn_object;	
	private float timer = 0.0f;
	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () { 
		// If we are ready to spawn a new entity, due so
		if (timer < Time.time && counter < max_count) {
			counter++;
			Spawn (GetValidRandomEnemySpawnPoint(),enemy);
			timer = Time.time + spawn_frequency;
		}
	}

	void Spawn(Vector3 position, GameObject obj){
		Quaternion rotation = new Quaternion ();
		Instantiate(obj, position, rotation);
	}

	// Returns a Valid Spawn Point based on level params
	Vector3 GetValidRandomEnemySpawnPoint(){
		// Get Player Position
	//	GameObject player = GameObject.Find("Player");
		Vector3 spawn_pos = spawn_object.transform.position;

		// Pick a point within circle around player
		Vector3 enemy_pos;
		do { 
			enemy_pos = Random.onUnitSphere * Random.Range(min_distance, max_distance) + spawn_pos;
			enemy_pos.y = 1;
		} while ((enemy_pos - spawn_pos).magnitude < min_distance);

		return enemy_pos;
	}
}
