using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public ParticleSystem explosion;
	public GameObject fire;
	private Vector3 target;
	private float start_time;
	private Vector3 start; 
	private Vector3 velocity;
	private GameState gameState;

	// Use this for initialization
	void Start () {
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
		target = GameObject.Find("Player").transform.position;
		start = transform.position;
		start_time = Time.time;
		transform.rotation = Quaternion.LookRotation(target - transform.position);
		transform.Rotate (90, 0, 0);

		velocity = (target - start).normalized * 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.paused)
		{
			return;
		}
		//transform.Translate (velocity * Time.deltaTime);
		rigidbody.AddForce (velocity);
		if(Time.time > start_time + 5.0f){
			DestroyWithExplosion(transform.gameObject);
		}
	}

	public void DestroyWithExplosion(GameObject obj){
		Quaternion rotation = new Quaternion ();
		Instantiate( explosion, obj.transform.position, rotation);
		GameObject f = (GameObject)Instantiate (fire, obj.transform.position, rotation);
		Destroy (f, 60.0f);

		Destroy (obj);
	}
	
	void OnTriggerEnter(Collider collision) {
		if(collision.gameObject.name == "Shooter(Clone)" || collision.gameObject.name == "SpawnInsideMe"){
			return;
		} else if(collision.gameObject.tag == "Deadly"){
			DestroyWithExplosion(collision.gameObject);
		}
		DestroyWithExplosion(transform.gameObject);
	}
}
