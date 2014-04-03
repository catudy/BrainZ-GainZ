/// <summary>
/// Handles game state variables.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

struct Inventory{
	public float fire_extinguisher;
};

public class GameState : MonoBehaviour {
	public bool game_over = false;
	public int brainz = 0;
	public int gainz = 0;
	public float cutscene_length = 35.0f;
	public bool paused = false;
	private bool in_cutscene = true;
	Inventory inventory;
	private Camera cam;
	GameObject player;

	// Use this for initialization
	void Start () {
		inventory.fire_extinguisher = 10.0f;
		player = GameObject.Find ("Player");
		cam = player.GetComponentInChildren<Camera> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (paused) {
			return;
		}
		if (Time.time > cutscene_length || Input.GetButtonDown("Sprint")) 
		{
			in_cutscene = false;
			cam.enabled = true;
		} 
		else 
		{

		}

		if (game_over) 
		{
			// Call game over here
		}
		 
	}

	// Removes object from the scene.
	public void RemoveObject(GameObject destroyme)
	{
		Spawner[] spawners = GetComponents<Spawner> ();

		foreach (Spawner spawner in spawners) 
		{
			foreach(GameObject obj in spawner.spawn_objects)
			{
				if(obj.name + "(Clone)" == destroyme.name)
				{
					spawner.DestroyObject(destroyme);
				}
			}
		}
	}

	public void CheckDoor(GameObject door){
		if (door.name == "FireDoor") {
			if(inventory.fire_extinguisher > 0){
				Destroy (door);
			} else {
				game_over = true;
			}
		}
	}

	public void ChangeScene(string scene_name) {
		if(scene_name == "TestBuilding"){
			// Manage scene change here.

			// Load Scene change
		}
	}
	public bool RunGame(){
		return (!in_cutscene);
	}

	public bool UseFireExtinguisher(){
		inventory.fire_extinguisher -= Time.deltaTime;
		if (inventory.fire_extinguisher > 0.0f) {
			RaycastHit hit;
			if(Physics.Raycast(player.transform.position,player.transform.forward,out hit)){
				if(hit.distance < 10.0f){
					if(hit.collider.gameObject.name == "FireDoor"){
						Destroy (hit.collider.gameObject);
					}
				}
			}

			return true;
		} else {
			return false;
		}
	}
}
