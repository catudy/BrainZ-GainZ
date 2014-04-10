/// <summary>
/// Handles game state variables.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

struct Inventory{
	public float fire_extinguisher;
	public float flame_thrower;
};

public enum Item
{
	NONE,
	FIRE_EXTINGUISHER,
	FLAME_THROWER,
	BRAINZ,
	GAINZ
};

public enum ObjectiveType
{
	NONE,
	KILL, // Kill X Zombies
	SURVIVE, // Survive X seconds
	COLLECT, // Collect X Items
	SCAVENGER, // Find Special Item
	DAMAGE // Take less than X damage
}

public enum ObjectiveReward{
	NONE,
	BRAINZ,
	GAINZ
}

public class Objective{
	ObjectiveType type;
	ObjectiveReward reward;
	public float target; // how many of objective you need to do 
	public float current; // current objective progress
	public bool completed;
	Objective(){ // Default constructor
		type = ObjectiveType.NONE;
		reward = ObjectiveReward.NONE;
		current = 0;
		target = 1;
		completed = false;
	}
	Objective(ObjectiveType set_type, ObjectiveReward set_reward, float set_target){ // parametrized constructor
		type = set_type;
		reward = set_reward;
		target = set_target;
		current = 0;
	}
	void UpdateObjective(float amount){
		current += amount;
		if (current >= target) {
			completed = true;
		}
	}
};

public class GameState : MonoBehaviour {
	public bool game_over = false;
	public int brainz = 0;
	public int gainz = 0;
	public int pickup_temp = 0;
	public float cutscene_length_seconds = 35.0f;
	public bool paused = false;
	public Item active_item = Item.FLAME_THROWER;
	public ParticleSystem explosion;
	private Objective primary_objective;
	private Objective[] secondary_objectives;
	private float level_end_time = 0.0f;
	private bool in_cutscene = true;
	Inventory inventory;
	private Camera cam;
	GameObject player;
	GameObject barrier;

	// Use this for initialization
	void Start () {
		inventory.fire_extinguisher = 0.0f;
		inventory.flame_thrower = 10.0f;

		player = GameObject.Find ("Player");
		barrier = GameObject.Find ("Destroyed");
		cam = player.GetComponentInChildren<Camera> ();

	}

	// Update is called once per frame
	void Update () {

		if (pickup_temp == 5) {
			barrier.SetActive(false);
		}

		if (paused) {
			return;
		}
		if (Time.time > cutscene_length_seconds || Input.GetButtonDown("Sprint") || Input.GetButton("Start")) {
			in_cutscene = false;
			cam.enabled = true;
		} 
		else {

		}



		if (game_over) {
			// Call game over here
		}
		 
	}

	// Returns true if level is active
	public bool LevelTimerActive(){
		return (level_end_time > Time.time);
	}

	// Removes object from the scene.
	public void RemoveObject(GameObject destroyme)
	{
		Spawner[] spawners = GetComponents<Spawner> ();

		foreach (Spawner spawner in spawners) 	{
			foreach(GameObject obj in spawner.spawn_objects) {
				if(obj.name + "(Clone)" == destroyme.name) {
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
	
	public bool UseItem(){
		Item item = active_item;
		if(UseAmmo (item)){
			RaycastHit hit;
			Vector3 forward = player.transform.forward;
			Vector3 ray_start = player.transform.position;
			ray_start.y += 0.5f; // Shoot ray from head

			// Shoot rays in spread
			for(float i=-10; i < 10; i=i+0.1f){
				Vector3 ray = new Vector3(forward.x,forward.y,forward.z+i);
				if(Physics.Raycast(ray_start,ray,out hit)){
					Debug.DrawRay (ray_start,ray);
					if(hit.distance < 20.0f){
						if(item == Item.FIRE_EXTINGUISHER && hit.collider.gameObject.name == "FireDoor"){
							Destroy (hit.collider.gameObject);
						} else if(item == Item.FLAME_THROWER && hit.collider.gameObject.tag == "Deadly"){
							DestroyWithExplosion (hit.collider.gameObject);
						}
					}
				}
			}
			return true;
		} else {
			return false;
		}
	}

	private bool UseAmmo(Item item){
		if (item == Item.FIRE_EXTINGUISHER){
			inventory.fire_extinguisher -= Time.deltaTime;
			return (inventory.fire_extinguisher > 0.0f);
		} else if (item == Item.FLAME_THROWER){
			inventory.flame_thrower -= Time.deltaTime;
			return (inventory.flame_thrower > 0.0f);
		} else {
			return false;
		}
	}

	public void AddItem(Item item){
		if(item == Item.FIRE_EXTINGUISHER){
			inventory.fire_extinguisher += 5.0f;
		} else if (item == Item.BRAINZ){
			brainz =+ 10;
		} else if (item == Item.GAINZ){
			gainz =+ 10;
		}
	}

	public float GetItem(Item item){
		if(item == Item.FIRE_EXTINGUISHER){
			return inventory.fire_extinguisher;
		} else if (item == Item.FLAME_THROWER){ 
			return inventory.flame_thrower;
		} else {
			return 0.0f;
		}
	}

	public void NextItem(){
		if(active_item == Item.FIRE_EXTINGUISHER && inventory.flame_thrower > 0.0f){
			active_item = Item.FLAME_THROWER;
		} else if (active_item == Item.FLAME_THROWER && inventory.fire_extinguisher > 0.0f){
			active_item = Item.FIRE_EXTINGUISHER;
		}
	}

	private void DestroyWithExplosion(GameObject obj){
		Quaternion rotation = new Quaternion ();
		Instantiate( explosion, obj.transform.position, rotation);
		Destroy (obj);
	}

	public void SetLevelTime(float time){
		level_end_time = Time.time + time;
	}

	public float GetLevelTimeRemaining(){
		return level_end_time - Time.time;
	}
}

