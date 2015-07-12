using UnityEngine;
using System.Collections;

public class meteor : MonoBehaviour {

	public GameObject explosion;
	private GameObject player;
	private PlayerState playerState;
	private GameState gameState;
	private Animation cutsceneAnim;
	public AudioClip explosionSound;
	public AudioClip evilSound;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerState = GameObject.Find("Player").GetComponentInChildren<PlayerState>();
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y <= .077)
		{
			float dist =  Vector3.Distance(player.transform.position,gameObject.transform.position);
			Instantiate( explosion, gameObject.transform.position, new Quaternion());
			AudioSource.PlayClipAtPoint(explosionSound,transform.position);
			if((dist <= 5 && !gameState.playcutscene) && (dist <= 5 && !gameState.paused))
			{
				playerState.DealDamage(100);
				GetComponent<AudioSource>().Stop();
				AudioSource.PlayClipAtPoint(evilSound,transform.position);
			}
			Destroy(gameObject);
		}
	}
}
