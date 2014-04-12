using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private GameObject player;
	private PlayerState playerState;
	private ParticleSystem fireextinguisher;
	private ParticleSystem flamer;
	private Camera cam;
	private GameState gameState;
	private float next_swap = 0.0f;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		playerState = player.GetComponent<PlayerState>();
		fireextinguisher = GameObject.Find ("FireExtinguisherParticleEffect").GetComponent<ParticleSystem> ();
		flamer = GameObject.Find ("FlamethrowerParticleEffect").GetComponent<ParticleSystem> ();
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		gameState = GetComponent<GameState> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButton("Ability")){
			if(gameState.UseItem()){
				if(gameState.active_item == Item.FIRE_EXTINGUISHER){
					fireextinguisher.transform.position = player.transform.position;
					fireextinguisher.transform.rotation = cam.transform.rotation;
					fireextinguisher.enableEmission = true;
				} else if (gameState.active_item == Item.FLAME_THROWER){
					Vector3 temp = player.transform.position + cam.transform.forward * 4.0f;
					temp.y = 0.5f;
					flamer.transform.position = temp;
					flamer.transform.rotation = cam.transform.rotation;
					flamer.enableEmission = true;
				}
			} 
		} else {
			fireextinguisher.enableEmission = false;
			flamer.enableEmission = false;
		}

		if (Input.GetButton ("Swap Active")) {
			if(Time.time > next_swap){
				next_swap = Time.time + 0.7f;
				gameState.NextItem();
			}
		}

		if (Input.GetButton ("Sneak")) 
		{
			playerState.SetSneaking ();
		} 

		else if (Input.GetButton ("Sprint")) 
		{
			playerState.SetRunning();
		} 

		else 
		{
			playerState.SetWalking ();
		}
	}
}
