using UnityEngine;
using System.Collections;

/// <summary>
/// GUI controller.
/// 
/// Manages the HUD and updates it every frame.
/// Current Displayed HUD Items: Brainz, Gainz, Stamina Bar, Current Powerup and Cooldown.
/// 
/// Author: Albert Wohletz
/// </summary>
public class GUIController : MonoBehaviour {
	public Texture2D brainz_icon;
	public Texture2D gainz_icon;

	private GameState gameState;
	private PlayerState playerState;

	private int gamePauseW = 100;
	private int gamePauseH = 25;
	private int placeGamePauseW;
	private int placeGamePauseH;

	// Use this for initialization
	void Start () {
		gameState = GetComponent<GameState>();
		playerState = GameObject.Find("Player").GetComponent<PlayerState> ();

		placeGamePauseW = (Screen.width-gamePauseW)/2;
		placeGamePauseH = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}

	
	void OnGUI () {
		// Brainz
		GUI.Label (new Rect (0, 0, 25, 25), brainz_icon);
		GUI.Label (new Rect (25, 0, 25, 25), gameState.brainz.ToString());

		// Gainz
		GUI.Label (new Rect (Screen.width - 55, 0, Screen.width - 30, 25), gainz_icon);
		GUI.Label (new Rect (Screen.width - 30, 0, Screen.width, 25), gameState.gainz.ToString());

		// Stamina Box
		GUI.Box (new Rect (0, Screen.height - 30, playerState.GetStaminaPercent () * 50, 20), "Stamina");

	
		// Powerup
		float width = playerState.power_up_time_remaining * 10;
		if (width > 0.0f) { // without this it always draws a wonkey tiny box.
			GUI.Box (new Rect (Screen.width - width, Screen.height - 30, width, 20), playerState.power_up.ToString ());
		}

		if(GUI.Button(new Rect(placeGamePauseW,placeGamePauseH,gamePauseW,gamePauseH), "PAUSE"))
		{
			Application.LoadLevel("Main_Menu");
		}


	}
}
