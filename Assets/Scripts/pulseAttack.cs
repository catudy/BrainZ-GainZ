using UnityEngine;
using System.Collections;

public class pulseAttack : MonoBehaviour {

	private GameState gameState;
	private UpgradeGUI upgradeGUI;
	public AudioClip killSound;

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
			GetComponent<AudioSource>().PlayOneShot(killSound);
			upgradeGUI.collected_brains += 2;
			upgradeGUI.collected_gains += 2;
			gameState.brainz += 2;
			gameState.gainz += 2;
			Destroy(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			//gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			GetComponent<AudioSource>().PlayOneShot(killSound);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			//gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			GetComponent<AudioSource>().PlayOneShot(killSound);
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

}
