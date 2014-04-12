using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {


	void OnTriggerEnter(Collider other)
	{
		Destroy (other.gameObject);
		Debug.Log ("destroy");
	}
}
