/// <summary>
/// Handles game state variables.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public enum ObjectiveType
{
	NONE,
	KILL, // Kill X Zombies
	FIRE, // Put out X Fires
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
		done = false;
	}
	public void SetRandomObjective(int level){
		type = (ObjectiveType)Random.Range (1, 5);
		reward_amount = Random.Range (20 * level, 30 * level);
		if (type == ObjectiveType.DAMAGE){
			target = Random.Range(level, level*5);
			reward = ObjectiveReward.GAINZ;
		} else if (type == ObjectiveType.KILL){
			target = Random.Range(level * 10, level * 20);
			reward = ObjectiveReward.GAINZ;
		} else if (type == ObjectiveType.SCAVENGER){
			target = Random.Range(level, level*2);
			reward = ObjectiveReward.BRAINZ;
		} else if (type == ObjectiveType.FIRE){
			target = Random.Range (10*level, 20*level);
			reward = ObjectiveReward.BRAINZ;
		}
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
};

public class GameState : MonoBehaviour {
	public bool game_over = false;
	public int brainz = 0;
	public int gainz = 0;
	public bool paused = false;
	public ParticleSystem explosion;
	public int level = 1;
	public Objective primary_objective = new Objective();
	public Objective[] secondary_objectives;
	public int num_objectives = 4;
	public GameObject[] spawn_points;
	private GameObject player;
	private PlayerState playerState;

	// Use this for initialization
	void Start () {

		player = GameObject.Find ("Player");
		playerState = player.GetComponent<PlayerState> ();
		InitializeLevel ();
	}

	// Update is called once per frame
	void Update () {
		if (paused) {
			return;
		}

		UpdateObjectives ();
		 
	}

	public void InitializeLevel(){
		InitializeObjectives();
		InitializePlayerPosition();
		InitializePlayer ();
		paused = false;

	}

	public void InitializePlayer(){
		playerState.stamina = playerState.playerStats.max_stamina;
		playerState.health = playerState.playerStats.max_health;
	}

	public void InitializePlayerPosition(){
		int spawn_index = (level-1) % spawn_points.Length;
		player.transform.position = spawn_points[spawn_index].transform.position;
	}

	public void InitializeObjectives(){
		// Primary Objective time for now
		primary_objective.SetObjective (ObjectiveType.TIME, ObjectiveReward.NONE, 30*level, 0);


		// Set secondary Objectives
		secondary_objectives = new Objective[num_objectives];
		for(int i=0; i<num_objectives; i++){
			secondary_objectives[i] = new Objective();
		}
		secondary_objectives[0].SetObjective(ObjectiveType.DAMAGE, ObjectiveReward.GAINZ, Random.Range(level * 2, level * 5),Random.Range(level *20, level * 50));
		secondary_objectives[1].SetObjective(ObjectiveType.KILL, ObjectiveReward.BRAINZ, Random.Range (level * 5, level * 10),Random.Range(level *20, level * 50));
		secondary_objectives [2].SetObjective (ObjectiveType.FIRE, ObjectiveReward.BRAINZ, Random.Range (level * 5, level * 10), Random.Range (level * 20, level * 50));
		secondary_objectives [3].SetObjective (ObjectiveType.SCAVENGER, ObjectiveReward.BRAINZ, Random.Range (1, level), Random.Range (level * 20, level * 50));

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

	private void DestroyWithExplosion(GameObject obj){
		Quaternion rotation = new Quaternion ();
		Instantiate( explosion, obj.transform.position, rotation);
		Destroy (obj);
	}

	public float GetLevelTimeRemaining(){
		return primary_objective.target - primary_objective.current;
	}

	public void SpendBrainzNGainz(int b, int g){
		brainz = brainz - b;
		gainz = gainz - g;
	}
}

