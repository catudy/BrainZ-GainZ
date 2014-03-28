using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour 
{

	private int levelBoxWidth = 100;
	private int levelBoxHeight = 50;
	private int levelBoxWPlace;
	private int levelBoxHPlace;
	
	private int renoButtonWPlace = 90;
	private int renoButtonHPlace = 25;

	void Start()
	{
		//Calculate the level select box location
		levelBoxWPlace = (Screen.width-levelBoxWidth)/2;
		levelBoxHPlace = (Screen.height-levelBoxHeight)-20;
	}
	void OnGUI () 
	{
		// Make a background box for level loaded
		GUI.Box(new Rect(levelBoxWPlace-5,levelBoxHPlace-20,levelBoxWidth,levelBoxHeight), "Level Select");
		
		//Make the reno button with scene transition
		if(GUI.Button(new Rect(levelBoxWPlace,levelBoxHPlace,renoButtonWPlace,renoButtonHPlace), "Reno"))
		{
			Application.LoadLevel("Sushil_Test");
		}
	}
}
