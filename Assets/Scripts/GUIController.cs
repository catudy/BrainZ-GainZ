using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour 
{
	//Variable for the current gamestate
	private GameState gameState;

	//Variable for the current player
	private PlayerState playerState;

	//Texture variables
	public Texture2D brainz_icon;
	public Texture2D gainz_icon;

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
	private bool pauseButton;

	//Scene loading variables
	private string currentScene = null;
	//private string companyLogo = "_CompanyLogo";
	private string mainMenu = "_MainMenu";
	private string reno = "_Reno";

	//Pause button variables
	private int gamePauseW = 100;
	private int gamePauseH = 25;
	private int placeGamePauseW;
	private int placeGamePauseH; 

	void Start()
	{
		//Get the current scene and save it in the gamestate
		currentScene = Application.loadedLevelName;

		//Get main menus start variables
		if(currentScene == mainMenu)
		{
			//Calculate various box and button placement and size values
			levelBoxWPlace = (Screen.width-levelBoxWidth)/2;
			levelBoxHPlace = (Screen.height-levelBoxHeight)-20;
			placeGameQuitW = (Screen.width-gameQuitW)/2;
			placeGameQuitH = 0;
			//
		}

		//Get reno level start variables
		if(currentScene == reno)
		{
			//Getting gamestate and player state and gui variables
			gameState = GameObject.Find("GameController").GetComponent<GameState>();
			playerState = GameObject.Find("Player").GetComponent<PlayerState>();
			placeGamePauseW = (Screen.width-gamePauseW)/2;
			placeGamePauseH = 0;
		}
	}
	
	void OnGUI () 
	{
		//Load GUI for main menu
		if(currentScene == mainMenu)
		{
			//Creating reno box and button for level select
			createBox (levelBoxWPlace-5,levelBoxHPlace-20,levelBoxWidth,levelBoxHeight, "Level Select");
			renoButton = createButton(levelBoxWPlace,levelBoxHPlace,renoButtonWPlace,renoButtonHPlace,"Reno");

			//Creating quit button for game
			quitButton = createButton(placeGameQuitW,placeGameQuitH,gameQuitW,gameQuitH, "QUIT");

			//If the reno button is pressed, load the reno level
			if(renoButton)
			{
				Application.LoadLevel(reno);
			}

			//If the quit button is pressed, close the application
			if(quitButton)
			{
				Application.Quit();
			}
		}

		//Load GUI for reno level
		else if(currentScene == reno)
		{
			//Create pause button
			pauseButton = createButton(placeGamePauseW,placeGamePauseH,gamePauseW,gamePauseH, "PAUSE");

			//If the pause button is pressed open the pause menu (but for now just returns to main menu)
			if(pauseButton)
			{
				Application.LoadLevel(mainMenu);
			}

			//Displaying brainz and gainz score on HUD
			createImage(0,0,25,25, brainz_icon);
			createText(25,0,25,25, gameState.brainz.ToString());
			createImage(Screen.width - 55, 0, Screen.width - 30, 25, gainz_icon);
			createText(Screen.width - 30, 0, Screen.width, 25, gameState.gainz.ToString());

			//Displaying stamina bar on HUD
			createBox(0, Screen.height - 30, playerState.GetStaminaPercent() * 50, 20, "Stamina");

			float powerup_width = playerState.power_up_time_remaining *10;

			//Displaying power-up bar on HUD if player has one
			if(powerup_width > 0.0f)
			{
				createBox(Screen.width - powerup_width, Screen.height - 30, powerup_width, 20, playerState.power_up.ToString());
			}
		}
		
	  }


	//Function that creates a button and places it and returns its boolean value if pressed or not
	bool createButton(int widthPlacement, int heightPlacement, int buttonWidth, int buttonHeight, string displayText)
	{
		return GUI.Button(new Rect(widthPlacement,heightPlacement,buttonWidth,buttonHeight), displayText);
	}

	//Function that creates a box and places it on the screen
	void createBox(float widthPlacement, float heightPlacement, float boxWidth, float boxHeight, string displayText)
	{
		GUI.Box(new Rect(widthPlacement,heightPlacement,boxWidth,boxHeight), displayText);
	}

	//Function that creates a image label and prints it to the screen
	void createText(int widthPlacement, int heightPlacement, int imageWidth, int imageHeight, string text)
	{
		GUI.Label(new Rect(widthPlacement, heightPlacement, imageWidth, imageHeight), text);
	}

	//Function that creates a text label and prints it to the screen
	void createImage(int widthPlacement, int heightPlacement, int imageWidth, int imageHeight, Texture2D image)
	{
		GUI.Label(new Rect(widthPlacement, heightPlacement, imageWidth, imageHeight), image);
	}
}
