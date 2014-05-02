using UnityEngine;
using System.Collections;

public class GunAttack : MonoBehaviour {

	public float speed;
	private float delay = 2.0f;
	private GameState gameState;
	private UpgradeGUI upgradeGUI;
	public AudioClip killSound;


	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * speed;
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();
		upgradeGUI = GameObject.Find("GUIController").GetComponentInChildren<UpgradeGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, delay);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			AudioSource.PlayClipAtPoint(killSound,transform.position);
			gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
			upgradeGUI.collected_brains += 10;
			upgradeGUI.collected_gains += 10;
			gameState.brainz += 10;
			gameState.gainz += 10;
			Destroy (other.gameObject);
			Destroy (gameObject);

		}
	}
}
