using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public AudioClip shoutingClip;      // Audio clip of the player shouting.
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.1f;  // The damping for the speed parameter
	public float base_speed = 5.0f;
	private PlayerState playerState;
	private Animator anim;              // Reference to the animator component.
	private HashIDs hash;               // Reference to the HashIDs.

	void Start() {
		playerState = GetComponent<PlayerState>();
	}
	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		
		// Set the weight of the shouting layer to 1.
		anim.SetLayerWeight(1, 1f);
	}
	
	
	void FixedUpdate ()
	{
		// Cache the inputs.
		bool sneak = Input.GetButton("Sneak");
		bool sprint = Input.GetButton ("Sprint");
		MovementManagement(sneak, sprint);
	}
	
	
	void Update ()
	{
		// Cache the attention attracting input.
		bool shout = Input.GetButtonDown("Attract");
		
		// Set the animator shouting parameter.
		anim.SetBool(hash.shoutingBool, shout);
		
		AudioManagement(shout);
	}
	
	
	void MovementManagement (bool sneaking, bool sprinting)
	{
		float current_speed = base_speed;
		if (sneaking) {
			current_speed = base_speed / 2.0f;
			playerState.SetSneaking();
		} else if (sprinting) {
			current_speed = base_speed * 1.5f;
			playerState.SetRunning();
		} else {
			playerState.SetWalking();
		}
		// Set the sneaking parameter to the sneak input.
		anim.SetBool(hash.sneakingBool, sneaking);
		anim.SetFloat (hash.speedFloat, current_speed, speedDampTime, Time.deltaTime);
	}
	
	
	void AudioManagement (bool shout)
	{
		// If the player is currently in the run state...
		if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
		{
			// ... and if the footsteps are not playing...
			if(!audio.isPlaying)
				// ... play them.
				audio.Play();
		}
		else
			// Otherwise stop the footsteps.
			audio.Stop();
		
		// If the shout input has been pressed...
		if(shout)
			// ... play the shouting clip where we are.
			AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
	}
}