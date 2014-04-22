using UnityEngine;
using System.Collections;

public class GunAttack : MonoBehaviour {

	public float speed;
	private float delay = 1.0f;

	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * speed;
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, delay);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
		Destroy (other.gameObject);
		Destroy (gameObject);
			Debug.Log ("gun works");
		}
	}
}
