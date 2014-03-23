/// <summary>
/// Handles game state variables.
/// 
/// Author: Albert Wohletz
/// </summary>
using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public bool game_over = false;
	public int brainz = 0;
	public int gainz = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (game_over) {
			// Call game over here
		}
	}

	public void RemoveObject(GameObject destroyme){
		Spawner[] spawners = GetComponents<Spawner> ();
		foreach (Spawner spawner in spawners) {
			foreach(GameObject obj in spawner.objects){
				if(obj.name + "(Clone)" == destroyme.name){
					spawner.DestroyObject(destroyme);
				}
			}
		}
	}
}
