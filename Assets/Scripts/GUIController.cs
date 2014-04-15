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
	public Texture2D fire_extinguisher_icon;
	public Texture2D flamer_icon;
	
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
	private string newReno = "New_reno";
	
	//Pause button variables
	//private int gamePauseW = 100;
	//private int gamePauseH = 25;
	private int placeGamePauseW;
	private int placeGamePauseH; 
	
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	public float a,b;
	public int c,d;
	
	
	void Start()
	{
		//Get the current scene and save it in the gamestate
		currentScene = Application.loadedLevelName;

		//Get reno level start variables
		if(currentScene == reno || currentScene == newReno)
		{
			//Getting gamestate and player state and gui variables
			gameState = GameObject.Find("GameController").GetComponent<GameState>();
			playerState = GameObject.Find("Player").GetComponent<PlayerState>();
		}
	}
	
	void OnGUI () 
	{
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z = 1;
		
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);
		
		//Load GUI for main menu
		if(currentScene == mainMenu)
		{
			//Calculate various box and button placement and size values
			levelBoxWPlace = (Screen.width-levelBoxWidth)/2;
			levelBoxHPlace = (Screen.height-levelBoxHeight)-20;
			placeGameQuitW = (Screen.width-gameQuitW)/2;
			placeGameQuitH = 0;

			//Creating reno box and button for level select
			createBox (273,424,levelBoxWidth,levelBoxHeight, "Level Select");
			renoButton = createButton(278,445,renoButtonWPlace,renoButtonHPlace,"Reno");
			
			//Creating quit button for game
			quitButton = createButton(278,0,gameQuitW,gameQuitH, "QUIT");
			
			//If the reno button is pressed, load the reno level
			if(renoButton || Input.GetButton("Start"))
			{
				Application.LoadLevel(newReno);
			}
			
			//If the quit button is pressed, close the application
			if(quitButton || Input.GetButton("Back"))
			{
				Application.Quit();
			}
		}
		
		//Load GUI for reno level
		else if(currentScene == reno || currentScene == newReno)
		{
			//// Make me look purty
			createText (10, 100, 200,30, gameState.primary_objective.GetString());

			int offset_y = 35;
			foreach (Objective objective in gameState.secondary_objectives) {
				createText(10,100+offset_y,200,30,objective.GetString());
				offset_y += 35;
			}

			createText (0,Screen.height-30,30,30,"HP: " + playerState.health.ToString());
			// End Make me look purty


			//Create pause button
			pauseButton = createButton(578,6,59,20, "PAUSE");
			
			//If the pause button is pressed open the pause menu (but for now just returns to main menu)
			if(pauseButton || Input.GetButton("Back"))
			{
				Application.LoadLevel(mainMenu);
			}
			
			//Displaying brainz and gainz score on HUD
			createImage(59,50,48,23, brainz_icon);
			createText(72,64,8,25, gameState.brainz.ToString());
		
			createImage(98,48,20,27, gainz_icon);
			createText(104,64,8,25, gameState.gainz.ToString());


			//Displaying stamina bar on HUD
			if(playerState.GetStaminaPercent() *200 >=12)
			{
				createBox(224,18, playerState.GetStaminaPercent() *175, 27, "");
			}
			createText(284,22,133,35,"STAMINA");
			
			if(gameState.active_item == Item.FIRE_EXTINGUISHER){
				// Fire Extinguisher Status
				float extinguisher_ammo = gameState.GetItem(Item.FIRE_EXTINGUISHER);
				if(extinguisher_ammo > 0.0f){
					createText(18,52,54,31, extinguisher_ammo.ToString("F2"));
					createImage(12,3,42,80, fire_extinguisher_icon);
				}
			} else if (gameState.active_item == Item.FLAME_THROWER){
				// Fire Extinguisher Status
				float flamer_ammo = gameState.GetItem(Item.FLAME_THROWER);
				if(flamer_ammo > 0.0f){
					createText(15,52,54,31, flamer_ammo.ToString("F2"));
					createImage(10,8,132,51, flamer_icon);
				}
			}
			
			//Displaying power-up bar on HUD if player has one
			float powerup_width = playerState.power_up_time_remaining *8;
			if(playerState.power_up_time_remaining > 0.64f)
			{
				createBox(246,47,powerup_width,12,"");
				if(playerState.power_up.ToString() == "INVISIBILITY")
				{
					createText(248,43,90,50,"INVISIBILITY");
				}
				else if(playerState.power_up.ToString() == "BLINK")
				{
					createText(248,43,90,50,"BLINK");
				}
				
				else if(playerState.power_up.ToString() == "STUN")
				{
					createText(248,43,90,50,"STUN");
				}
				
				else if(playerState.power_up.ToString() == "SECOND_WIND")
				{
					createText(248,43,90,50,"SECOND WIND");
				}
				
				else if(playerState.power_up.ToString() == "INVULNERABLE")
				{
					createText(248,43,90,50,"INVULNERABLE");
				}
				//createBox(Screen.width - powerup_width, Screen.height - 30, powerup_width, 20, playerState.power_up.ToString());
			}

		}
		
		GUI.matrix = svMat;
		
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
	void createImage(int widthPlacement, int heightPlacement, float imageWidth, int imageHeight, Texture2D image)
	{
		GUI.Label(new Rect(widthPlacement, heightPlacement, imageWidth, imageHeight), image);
	}
	
	void Update()
	{
		originalWidth = Screen.resolutions[0].width;
		originalHeight = Screen.resolutions[0].height;
	}
}


/*using UnityEngine;
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
	public Texture2D fire_extinguisher_icon;
	public Texture2D flamer_icon;

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
	//private int gamePauseW = 100;
	//private int gamePauseH = 25;
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
			//placeGamePauseW = (Screen.width-gamePauseW)/2;
			//placeGamePauseH = 0;
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
			if(renoButton || Input.GetButton("Start"))
			{
				Application.LoadLevel(reno);
			}

			//If the quit button is pressed, close the application
			if(quitButton || Input.GetButton("Back"))
			{
				Application.Quit();
			}
		}

		//Load GUI for reno level
		else if(currentScene == reno)
		{
			//Create pause button
			pauseButton = createButton(660,5,65,20, "PAUSE");

			//If the pause button is pressed open the pause menu (but for now just returns to main menu)
			if(pauseButton || Input.GetButton("Back"))
			{
				Application.LoadLevel(mainMenu);
			}

			//Displaying brainz and gainz score on HUD
			createImage(71,39,30,30, brainz_icon);
			createText(82,53,30,30, gameState.brainz.ToString());

			createImage(111,36,30,30, gainz_icon);
			createText(119, 53, 30, 30, gameState.gainz.ToString());

			//Displaying stamina bar on HUD
			if(playerState.GetStaminaPercent() *200 >=12)
			{
				createBox(255, 16, playerState.GetStaminaPercent() *200, 23, "");
			}
			createText(328,17,109,35,"STAMINA");
		
			if(gameState.active_item == Item.FIRE_EXTINGUISHER){
				// Fire Extinguisher Status
				float extinguisher_ammo = gameState.GetItem(Item.FIRE_EXTINGUISHER);
				if(extinguisher_ammo > 0.0f){
					createText(23,45,30,25, extinguisher_ammo.ToString("F2"));
					createImage(20,5,45,42, fire_extinguisher_icon);
				}
			} else if (gameState.active_item == Item.FLAME_THROWER){
				// Fire Extinguisher Status
				float flamer_ammo = gameState.GetItem(Item.FLAME_THROWER);
				if(flamer_ammo > 0.0f){
					createText(23,45,30,25, flamer_ammo.ToString("F2"));
					createImage(20,5,45,42, flamer_icon);
				}
			}

			//Displaying power-up bar on HUD if player has one
			float powerup_width = playerState.power_up_time_remaining *10;
			if(playerState.power_up_time_remaining > 0.64f)
			{
				createBox(280,44,powerup_width,5,"");
				if(playerState.power_up.ToString() == "INVISIBILITY")
				{
					createText(280,48,120,67,"INVISIBILITY");
				}
				else if(playerState.power_up.ToString() == "BLINK")
				{
					createText(280,48,120,67,"BLINK");
				}

				else if(playerState.power_up.ToString() == "STUN")
				{
					createText(280,48,120,67,"STUN");
				}

				else if(playerState.power_up.ToString() == "SECOND_WIND")
				{
					createText(280,48,120,67,"SECOND WIND");
				}

				else if(playerState.power_up.ToString() == "INVULNERABLE")
				{
					createText(280,48,120,67,"INVULNERABLE");
				}
				//createBox(Screen.width - powerup_width, Screen.height - 30, powerup_width, 20, playerState.power_up.ToString());
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
	void createImage(int widthPlacement, int heightPlacement, float imageWidth, int imageHeight, Texture2D image)
	{
		GUI.Label(new Rect(widthPlacement, heightPlacement, imageWidth, imageHeight), image);
	}
}
*/