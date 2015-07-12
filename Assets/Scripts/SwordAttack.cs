using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {
	
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
		if(other.tag == "Deadly")
		{
			GetComponent<AudioSource>().PlayOneShot(killSound);
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			upgradeGUI.collected_brains += 2;
			upgradeGUI.collected_gains += 2;
			gameState.brainz += 2;
			gameState.gainz += 2;
			Destroy (other.gameObject);
		}
	}
}
