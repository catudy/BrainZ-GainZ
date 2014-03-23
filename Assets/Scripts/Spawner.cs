/// <summary>
/// Spawns game objects at a specific timer, and to a max count.
/// 
/// Author: Albert Wohletz
/// </summary>

using UnityEngine;
using System.Collections;


public class Spawner : MonoBehaviour {
	public int max_count = 100; // Max number of things to spawn.
	public float spawn_frequency = 1; // How often we will spawn
	public GameObject[] objects; // what we will spawn
	public float min_distance = 0; // How close can we spawn
	public float max_distance = 0; // How far away can we spawn
	public GameObject[] spawn_points; // Around What we are Spawning
	private int counter = 0;	
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
			int index = Random.Range(0,objects.Length);
			Spawn (GetValidRandomEnemySpawnPoint(),objects[index]);
			timer = Time.time + spawn_frequency;
		}
	}

	void Spawn(Vector3 position, GameObject obj){
		Quaternion rotation = new Quaternion ();
		Instantiate(obj, position, rotation);
	}

	// Returns a Valid Spawn Point based on level params
	Vector3 GetValidRandomEnemySpawnPoint(){
		// Choose A spawn point
		int index = Random.Range (0, spawn_points.Length);
		Vector3 spawn_pos = spawn_points[index].transform.position;

		// Pick a point within circle around player
		Vector3 enemy_pos;
		do { 
			enemy_pos = Random.onUnitSphere * Random.Range(min_distance, max_distance) + spawn_pos;
			enemy_pos.y = 1;
		} while ((enemy_pos - spawn_pos).magnitude < min_distance);

		return enemy_pos;
	}

	public void DestroyObject(GameObject obj){
		counter--;
		Debug.Log (counter);
		Destroy (obj);
	}
}
