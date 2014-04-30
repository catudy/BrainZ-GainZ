using UnityEngine;
using System.Collections;

public class Despawner : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Despawn(){
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Deadly")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Item")){
			Destroy(o);
		}
	}
}
