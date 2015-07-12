// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class chase : MonoBehaviour 
{
	enum CharacterState 
	{
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,
	}
	//THEIR STUFF...BE CAREFUL REMOVING OR CHANGING THESE
	// The speed when walking
	private float walkSpeed= 2.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	private float trotSpeed= 4.0f;
	// when pressing "Fire3" button (cmd) we start running
	private float runSpeed= 6.0f;
	private float inAirControlAcceleration= 3.0f;
	// How high do we jump when pressing jump and letting go immediately
	private float jumpHeight= 0.5f;
	// The gravity for the character
	private float gravity= 20.0f;
	// The gravity in controlled descent mode
	private float speedSmoothing= 10.0f;
	private float rotateSpeed= 1.0f;
	private float trotAfterSeconds= 3.0f;
	private bool canJump= false;
	private float jumpRepeatTime= 0.05f;
	private float jumpTimeout= 0.15f;
	private float groundedTimeout= 0.25f;
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer= 0.0f;
	// The current move direction in x-z
	private Vector3 moveDirection= Vector3.zero;
	// The current vertical speed
	private float verticalSpeed= 0.0f;
	// The current x-z move speed
	private float moveSpeed= 0.0f;
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags; 
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping= false;
	private bool jumpingReachedApex= false;
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack= false;
	// Is the user pressing any keys?
	private bool isMoving= false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart= 0.0f;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime= -10.0f;
	// Last time we performed a jump
	private float lastJumpTime= -1.0f;
	private Vector3 inAirVelocity= Vector3.zero;
	private float lastGroundedTime= 0.0f;
	private bool isControllable= true;
	
	//Our stuff
	private GameState gameState;
	private CharacterState _characterState;
	private PlayerState playerState;
	private GameObject player;
	private float ranDir = 0.0f;
	public float chaseSpeed = 1.0f;
	public float wanderSpeed = 1.0f;
	public float newWanderDirTime = 1.0f;
	public float aggro_range = 5.0f;
	private bool aggro;
	public bool playSound = true;
	public AudioClip chaseSound;

	private Animation _animation;
	
	void Start()
	{
		playerState = GameObject.Find("Player").GetComponent<PlayerState>();
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
		player = GameObject.Find("Player");
		InvokeRepeating("RandomDirection",0,newWanderDirTime);
	}
	void  Awake ()
	{
		moveDirection = transform.TransformDirection(Vector3.forward);
		_animation = GetComponent<Animation>();
	}
	
	
	void  UpdateSmoothedMovementDirection ()
	{
		Transform cameraTransform= Camera.main.transform;
		bool grounded= IsGrounded();
		
		// Forward vector relative to the camera along the x-z plane	
		Vector3 forward= new Vector3 (0,0,1); //cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		// Right vector relative to the camera
		// Always orthogonal to the forward vector
		Vector3 right= new Vector3(forward.z, 0, -forward.x);
		
		float v = 1;
		float h = ranDir;
		
		// Are we moving backwards or looking backwards
		if (v < -0.2f)
			movingBack = true;
		else
			movingBack = false;
		
		bool wasMoving= isMoving;

		isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;

		Vector3 targetDirection = new Vector3 (0,0,0);
		// Target direction relative to the camera
		if(aggro)
		{
			targetDirection = player.transform.position - transform.position;

		}
		else
		{
			targetDirection= h * right + v * forward;
		}

		//Debug.Log(targetDirection);
		// Grounded controls
		if (grounded)
		{
			// Lock camera for short period when transitioning moving & standing still
			lockCameraTimer += Time.deltaTime;
			if (isMoving != wasMoving)
				lockCameraTimer = 0.0f;
			
			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				// If we are really slow, just snap to the target direction
				if (moveSpeed < walkSpeed * 0.9f && grounded)
				{
					moveDirection = targetDirection.normalized;
				}
				// Otherwise smoothly turn towards it
				else
				{
					moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
					
					moveDirection = moveDirection.normalized;
				}
			}
			
			// Smooth the speed based on the current target direction
			float curSmooth= speedSmoothing * Time.deltaTime;
			
			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			float targetSpeed= Mathf.Min(targetDirection.magnitude, 1.0f);
			
			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);   
			
			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;
		}
	}

	void  ApplyJumping (){
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
		
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing		
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void  ApplyGravity ()
	{
		if (isControllable)	// don't move player at all if not controllable.
		{
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
			{
				jumpingReachedApex = true;
				SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
			
			if (IsGrounded ())
				verticalSpeed = 0.0f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}
	
	public float CalculateJumpVerticalSpeed ( float targetJumpHeight  )
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}
	
	public void DidJump ()
	{
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpButtonTime = -10;
		
		_characterState = CharacterState.Jumping;
	}
	
	void Update ()
	{
		//Pauses the player if pause is pressed
		if(gameState.paused)
		{
			_animation.enabled = false;
			return;
		}
		else
			_animation.enabled = true;
			
			if (!isControllable)
		{
			// kill all inputs if not controllable.
			Input.ResetInputAxes();
		}
		
		if (Input.GetButtonDown ("Jump"))
		{
			lastJumpButtonTime = Time.time;
		}
	
		if(aggro && playSound)
		{
			GetComponent<AudioSource>().clip = chaseSound;
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().volume = 0.65f;
			GetComponent<AudioSource>().Play();
			StartCoroutine(WaitForSound(3));
		}

		aggro = IsAggroed (player.transform.position, transform.position);
		UpdateSmoothedMovementDirection();
		
		// Apply gravity
		// - extra power jump modifies gravity
		// - controlledDescent mode modifies gravity
		ApplyGravity ();
		
		// Apply jumping logic
		ApplyJumping ();
		
		// Calculate actual motion

		Vector3 movement = new Vector3 (0,0,0);

		if(aggro)
		{
			movement= moveDirection * moveSpeed*chaseSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
		}
		else
		{
			movement= moveDirection * moveSpeed*wanderSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
		}

		movement *= Time.deltaTime;
		
		// Move the controller
		CharacterController controller = GetComponent<CharacterController>();
		collisionFlags = controller.Move(movement);
		
		// Set rotation to the move direction
		if (IsGrounded())
		{
			
			transform.rotation = Quaternion.LookRotation(moveDirection);
			
		}	
		else
		{
			Vector3 xzMove= movement;
			xzMove.y = 0;
			if (xzMove.sqrMagnitude > 0.001f)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}
		}	
		
		// We are in jump mode but just became grounded
		if (IsGrounded())
		{
			lastGroundedTime = Time.time;
			inAirVelocity = Vector3.zero;
			if (jumping)
			{
				jumping = false;
				SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(hit.gameObject.tag == "Player" && playerState.power_up != PowerUp.INVULNERABLE)
		{
			playerState.DealDamage(1);
		}
		if (hit.moveDirection.y > 0.01f) 
			return;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" && playerState.power_up != PowerUp.INVULNERABLE)
		{
			playerState.DealDamage(1);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player" && playerState.power_up != PowerUp.INVULNERABLE)
		{
			playerState.DealDamage(1);
		}
	}
	
	public float GetSpeed ()
	{
		return moveSpeed;
	}
	
	public bool IsJumping ()
	{
		return jumping;
	}
	
	public bool IsGrounded ()
	{
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	public Vector3 GetDirection ()
	{
		return moveDirection;
	}
	
	public bool IsMovingBackwards ()
	{
		return movingBack;
	}
	
	public float GetLockCameraTimer ()
	{
		return lockCameraTimer;
	}
	
	public bool IsMoving ()
	{
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}
	
	public bool HasJumpReachedApex ()
	{
		return jumpingReachedApex;
	}
	
	public bool IsGroundedWithTimeout ()
	{
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	public void Reset ()
	{
		gameObject.tag = "Player";
	}
	
	public void RandomDirection()
	{
		ranDir = Random.Range(-1,2);
		return;
	}

	private bool IsAggroed(Vector3 player_pos, Vector3 enemy_pos)
	{
		float mod_aggro_range = aggro_range;
		
		if (player.GetComponent<PlayerState> ().power_up == PowerUp.INVISIBILITY) 
		{
			mod_aggro_range = 0.0f;
		}
		return ((player_pos - enemy_pos).magnitude < mod_aggro_range);
	}

	IEnumerator WaitForSound(float waitTime) {
		playSound = false;
		yield return new WaitForSeconds(waitTime);
		playSound = true;
	}
	
}