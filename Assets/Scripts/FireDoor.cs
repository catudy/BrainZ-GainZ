using UnityEngine;
using System.Collections;

public class FireDoor : MonoBehaviour {

	private GameState gameState;
	private ParticleSystem flame;
	// Use this for initialization

	void Awake () {
		flame = GetComponent<ParticleSystem>();
	}
	void Start () {
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.paused)
		{
			flame.Pause();
		}
		else
		{
			flame.Play();
		}
	}
}
