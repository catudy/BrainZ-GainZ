using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour 
{
	private int levelBoxWidth = 100;
	private int levelBoxHeight = 50;
	private int levelBoxWPlace;
	private int levelBoxHPlace;


	void Start()
	{
		levelBoxWPlace = (Screen.width-levelBoxWidth)/2;
		levelBoxHPlace = (Screen.height-levelBoxHeight)/2;
	}
	void OnGUI () 
	{
		// Make a background box for level loaded
		GUI.Box(new Rect(levelBoxWPlace,levelBoxHPlace,levelBoxWidth,levelBoxHeight), "Level Select");
		
		//Make one button to load the game
		if(GUI.Button(new Rect(20,40,80,20), "Reno")) {
			Application.LoadLevel("Sushil_Test");
		}
	}
}
