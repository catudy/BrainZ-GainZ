using UnityEngine;
using System.Collections;

public class ExplosionDamager : MonoBehaviour {
	private bool hit = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider collider){
		if(!hit){
			if(collider.gameObject.name == "Player"){
				GameObject.Find ("Player").GetComponent<PlayerState> ().DealDamage(1);
				hit = true;
			}
		}
	}
}
