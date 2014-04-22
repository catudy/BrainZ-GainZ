using UnityEngine;
using System.Collections;

public class pulseAttack : MonoBehaviour {
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Deadly")
		{
			Destroy(other.gameObject);
			Debug.Log ("pulse works");
		}
	}

}
