using UnityEngine;
using System.Collections;

public class ZombieCollider : MonoBehaviour {
	
	void OnControllerColliderHit(ControllerColliderHit collision) 
	{
		if (collision.gameObject.tag == "Player") { // Don't care about the ground.
			Debug.Log ("Zambie" + collision.gameObject.name);
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
