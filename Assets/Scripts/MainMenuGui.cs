using UnityEngine;
using System.Collections;

public class MainMenuGui : MonoBehaviour 
{
	//Reno box variables
	private int levelBoxWidth = 100;
	private int levelBoxHeight = 50;
	private int levelBoxWPlace;
	private int levelBoxHPlace;

	//Reno button variables
	private int renoButtonWPlace = 90;
	private int renoButtonHPlace = 25;

	//Quit button variables
	private int gameQuitW = 100;
	private int gameQuitH = 25;
	private int placeGameQuitW;
	private int placeGameQuitH;

	//Button check variables
	private bool renoButton;
	private bool quitButton;

	//Scene loading variables
	private string renoLevel;

	void Start()
	{
		//Calculate various box and button placement and size values
		levelBoxWPlace = (Screen.width-levelBoxWidth)/2;
		levelBoxHPlace = (Screen.height-levelBoxHeight)-20;
		placeGameQuitW = (Screen.width-gameQuitW)/2;
		placeGameQuitH = 0;

		//Possible scenes to be loaded
		renoLevel = "Sushil_Test";
	}
	void OnGUI () 
	{
		//Creating reno box and button for level select
		createBox (levelBoxWPlace-5,levelBoxHPlace-20,levelBoxWidth,levelBoxHeight, "Level Select");
		renoButton = createButton(levelBoxWPlace,levelBoxHPlace,renoButtonWPlace,renoButtonHPlace,"Reno");

		//Creating quit button for game
		quitButton = createButton(placeGameQuitW,placeGameQuitH,gameQuitW,gameQuitH, "QUIT");

		//If the reno button is pressed, load the reno level
		if(renoButton)
		{
			Application.LoadLevel (renoLevel);
		}

		//If the quit button is pressed, close the application
		if(quitButton)
		{
			Application.Quit();
		}
	}

	//Function that creates a button and places it and returns its boolean value if pressed or not
	bool createButton(int widthPlacement, int heightPlacement, int buttonWidth, int buttonHeight, string displayText)
	{
		return GUI.Button(new Rect(widthPlacement,heightPlacement,buttonWidth,buttonHeight), displayText);

	}

	//Function that creates a box and places it
	void createBox(int widthPlacement, int heightPlacement, int boxWidth, int boxHeight, string displayText)
	{
		GUI.Box(new Rect(widthPlacement,heightPlacement,boxWidth,boxHeight), displayText);
	}

}
