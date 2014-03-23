using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public Texture2D brainz_icon;
	public Texture2D gainz_icon;
	private GameState gameState;
	// Use this for initialization
	void Start () {
		gameState = GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	
	void OnGUI () {
		GUI.Label (new Rect (0, 0, 25, 25), brainz_icon);
		GUI.Label (new Rect (30, 0, 55, 25), gameState.brainz.ToString());
		GUI.Label (new Rect (Screen.width - 55, 0, Screen.width - 30, 25), gainz_icon);
		GUI.Label (new Rect (Screen.width - 25, 0, Screen.width, 25), gameState.gainz.ToString());
	}
}
