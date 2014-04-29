using UnityEngine;
using System.Collections;

public class pulseAttack : MonoBehaviour {

	private GameState gameState;
	private UpgradeGUI upgradeGUI;

	void Start()
	{
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();
		upgradeGUI = GameObject.Find("GUIController").GetComponentInChildren<UpgradeGUI>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			upgradeGUI.collected_brains += 10;
			upgradeGUI.collected_gains += 10;
			gameState.brainz += 10;
			gameState.gainz += 10;
			Destroy(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			//gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			//gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

}
