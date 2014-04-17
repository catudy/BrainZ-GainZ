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
	COLLECT, // Collect X Items
	SCAVENGER, // Find Special Item
	DAMAGE, // Take less than X damage
	TIME // Survive X seconds
}

public enum ObjectiveReward{
	NONE,
	BRAINZ,
	GAINZ
}

public class Objective {
	public ObjectiveType type;
	public ObjectiveReward reward;
	public float target; // how many of objective you need to do 
	public float current; // current objective progress
	public bool completed;
	public int reward_amount;
	public bool done = false;


	public Objective(){ // Default constructor
		type = ObjectiveType.NONE;
		reward = ObjectiveReward.NONE;
		current = 0;
		target = 1;
		completed = false;
		reward_amount = 0;
	}

	public Objective(ObjectiveType set_type, ObjectiveReward set_reward, int set_target, int amount){
		SetObjective (set_type, set_reward, set_target,amount);
	}

	public void SetObjective(ObjectiveType set_type, ObjectiveReward set_reward, int set_target, int amount){ 
		type = set_type;
		reward = set_reward;
		target = (float)set_target;
		current = 0;
		completed = false;
		reward_amount = amount;
	}
	public void UpdateObjective(float amount)
	{
		current += amount;
		if (current >= target && !completed) 
		{
			GrantReward();
			completed = true;
			done = true;
		} 
		else if (done)
		{
			current = 0;

		}
	}

	private void GrantReward(){
		GameState gameState = GameObject.Find("GameController").GetComponent<GameState>();
		if(reward == ObjectiveReward.BRAINZ){
			gameState.brainz += reward_amount;
		} else if (reward == ObjectiveReward.GAINZ){
			gameState.gainz += reward_amount;
		}
	}

	public string GetString(){
		string ret;
		if(type == ObjectiveType.KILL){
			ret = "Kill Zombies: ";
		} else if (type == ObjectiveType.TIME){
			ret = "Survive: ";
		} else if (type == ObjectiveType.COLLECT){
			ret = "Collect: ";
		} else if (type == ObjectiveType.DAMAGE){
			ret = "Avoid Damage: ";
		} else if (type == ObjectiveType.SCAVENGER){
			ret = "Collect Scavenge Items: ";
		} else {
			return "NO_OBJECTIVE";
		}

		ret = ret + current.ToString("F0") + " / " + target + " Reward: " + reward_amount;

		if(reward == ObjectiveReward.BRAINZ){
			ret += " Brainz";
		} else if (reward == ObjectiveReward.GAINZ){
			ret += " Gainz";
		}
		return ret;
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
	public int level = 1;
	public Objective primary_objective = new Objective();
	public Objective[] secondary_objectives;
	public int num_objectives = 5;
	private bool in_cutscene = true;
	Inventory inventory;
	private Camera cam;
	GameObject player;
	GameObject barrier;
	private UpgradeGUI upgradeMenu;
	public bool inUpgradeMenu = false;

	public GameObject spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4;

	// Use this for initialization
	void Start () {
		inventory.fire_extinguisher = 0.0f;
		inventory.flame_thrower = 10.0f;

		player = GameObject.Find ("Player");
		barrier = GameObject.Find ("Destroyed");
		cam = player.GetComponentInChildren<Camera> ();
		upgradeMenu = GameObject.Find("GUIController").GetComponentInChildren<UpgradeGUI>();

		// Primary Objective time for now
		primary_objective.SetObjective (ObjectiveType.TIME, ObjectiveReward.NONE, 10*level, 0);

		// Set secondary Objectives
		secondary_objectives = new Objective[num_objectives];

		for(int i=0; i<secondary_objectives.Length; i++){
			secondary_objectives[i] = new Objective((ObjectiveType)Random.Range(1,5),(ObjectiveReward)Random.Range(1,3), Random.Range(10,30), Random.Range(20,40));
		}

		player.transform.position = spawnPoint1.transform.position;
		brainz = 500;
		gainz = 500;
	}

	// Update is called once per frame
	void Update () {
		if (paused) {
			return;
		}
		/*
		if (Time.time > cutscene_length_seconds || Input.GetButtonDown("Sprint") || Input.GetButton("Start")) {
			in_cutscene = false;
			cam.enabled = true;
		} 

		else {

		}
		*/
	 
		if (game_over) 
		{
			// Call game over scene transition

			// Return player to main menu
			Application.LoadLevel("_MainMenu");
		}

		UpdateObjectives ();
		 
	}

	private void UpdateObjectives(){
		UpdateObjective (ObjectiveType.TIME, Time.deltaTime);
	}

	public void UpdateObjective(ObjectiveType type, float value){
		foreach (Objective objective in secondary_objectives) {
			if(objective.type == type){
				objective.UpdateObjective(value);
			}
		}
		if(primary_objective.type == type){
			primary_objective.UpdateObjective(value);
		}
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
			int count = 0;
			for(float i=-10; i < 10; i=i+0.1f){
				Vector3 ray = new Vector3(forward.x,forward.y,forward.z+i);
				if(Physics.Raycast(ray_start,ray,out hit)){
					Debug.DrawRay (ray_start,ray);
					if(hit.distance < 20.0f){
						if(item == Item.FIRE_EXTINGUISHER && hit.collider.gameObject.name == "FireDoor"){
							Destroy (hit.collider.gameObject);
						} else if(item == Item.FLAME_THROWER && hit.collider.gameObject.tag == "Deadly"){
							DestroyWithExplosion (hit.collider.gameObject);
							UpdateObjective(ObjectiveType.KILL,1.0f);
							count++;
						}
					}
				}
			}
			Debug.Log ("Killed " + count + "Zambies");
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

	public float GetLevelTimeRemaining(){
		return primary_objective.target - primary_objective.current;
	}
}

