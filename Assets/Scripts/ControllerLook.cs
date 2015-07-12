using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit")]

public class ControllerLook : MonoBehaviour {

	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = 0.0f;
	public float yMaxLimit = 0.0f;

	private float x = 0.0f;
	private float y = 0.0f;

	private PlayerState playerState;

	// Use this for initialization
	void Start () {

		playerState = GameObject.Find("Player").GetComponent<PlayerState>();
	
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		if(GetComponent<Rigidbody>()){
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void LateUpdate()
	{
		x += Input.GetAxis("LookX") * xSpeed * 0.02f;
		y -= Input.GetAxis("LookY") * ySpeed * 0.02f;

		y = ClampAngle(y,yMinLimit,yMaxLimit);

		Quaternion rotation = Quaternion.Euler(y,x,0);

		transform.rotation = rotation;

		if(Input.GetAxis("Run") >= .2f)
		{
			playerState.SetRunning();
		}

		if(Input.GetButton("Ability"))
		{

		}
	}

	float ClampAngle(float angle, float min, float max)
	{
		if(angle<-360)
			angle += 360;
		if(angle >360)
			angle -= 360;
		return Mathf.Clamp(angle,min,max);
	}

}
