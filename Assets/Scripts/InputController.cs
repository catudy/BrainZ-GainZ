using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private GameObject player;
	private PlayerState playerState;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerState = player.GetComponent<PlayerState>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Sneak")) {
			playerState.SetSneaking ();
		} else if (Input.GetButton ("Sprint")) {
			playerState.SetRunning();
		} else {
			playerState.SetWalking ();
		}
	}
}
