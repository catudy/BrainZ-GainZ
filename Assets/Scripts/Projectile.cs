﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public ParticleSystem explosion;
	private Vector3 target;
	private float start_time;
	private Vector3 start; 
	private Vector3 velocity;
	CharacterController cc;

	// Use this for initialization
	void Start () {
		target = GameObject.Find("Player").transform.position;
		start = transform.position;
		start_time = Time.time;
		transform.Rotate (90, 0, 0);
		cc = GetComponent<CharacterController> ();
		velocity = (target - start).normalized * 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = Vector3.Lerp (start, target, (Time.time-start_time)/(5.0f));
		cc.Move (velocity * Time.deltaTime);
		if(Time.time > start_time + 5.0f){
			DestroyWithExplosion(transform.gameObject);
		}
	}

	public void DestroyWithExplosion(GameObject obj){
		Quaternion rotation = new Quaternion ();
		Object o = Instantiate( explosion, obj.transform.position, rotation);
		Destroy (obj);
	}
	
	void OnControllerColliderHit(ControllerColliderHit collision) {
		if(collision.gameObject.tag == "Deadly"){
			DestroyWithExplosion(collision.gameObject);
		}
		DestroyWithExplosion(transform.gameObject);
	}
}
