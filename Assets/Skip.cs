using UnityEngine;
using System.Collections;

public class Skip : MonoBehaviour {

	public GUIText guitext;

	// Use this for initialization
	void Start () {

		guitext.text = "Press 'ENTER' to Skip";
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey("enter"))
		{
			Destroy (guitext);
		}
	
	}
}
