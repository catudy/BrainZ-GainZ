using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public Texture2D brainz_icon;
	public Texture2D gainz_icon;

	private GameState gameState;
	private PlayerState playerState;

	// Use this for initialization
	void Start () {
		gameState = GetComponent<GameState>();
		playerState = GameObject.Find("Player").GetComponent<PlayerState> ();
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
		if (width > 0.0f) {
			GUI.Box (new Rect (Screen.width - width, Screen.height - 30, width, 20), playerState.power_up.ToString ());
		}
	}
}
