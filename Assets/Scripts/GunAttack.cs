using UnityEngine;
using System.Collections;

public class GunAttack : MonoBehaviour {

	public float speed;
	private float delay = 1.0f;
	private GameState gameState;

	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * speed;
		gameState = GameObject.Find ("GameController").GetComponentInChildren<GameState> ();
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, delay);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
		gameState.UpdateObjective(ObjectiveType.KILL,1.0f);
		Destroy (other.gameObject);
		Destroy (gameObject);
			Debug.Log ("gun works");
		}
	}
}
