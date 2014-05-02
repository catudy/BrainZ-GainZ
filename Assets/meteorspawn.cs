using UnityEngine;
using System.Collections;

public class meteorspawn : MonoBehaviour {

	public GameObject meteor;
	public GameObject meteorSpawn;
	private Animation cutsceneAnim;
	// Use this for initialization

	void Start () {
		InvokeRepeating("MeteorSpawn",0,0.5f);
		cutsceneAnim = GameObject.Find ("CutsceneCam").GetComponentInChildren<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("y"))
		{
			CancelInvoke();
		}
	}

	public void MeteorSpawn()
	{
		Vector3 randomPos = new Vector3(Random.Range(-139,133),Random.Range(50,150),Random.Range(-142,213));
		Instantiate(meteor,randomPos,Quaternion.Euler(270,0,0));
	}
}
