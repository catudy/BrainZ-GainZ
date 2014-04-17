using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {
	
	private GameState gameState;

	void Start()
	{
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Deadly")
		{
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			Destroy (other.gameObject);
			Debug.Log ("destroyed enemy");
		}
	}
}
