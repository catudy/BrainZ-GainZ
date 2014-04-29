using UnityEngine;
using System.Collections;

public class pulseAttack : MonoBehaviour {

	private GameState gameState;

	void Start()
	{
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

}
