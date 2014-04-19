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
	public Texture2D staminaBar;
	public Texture2D healthBar;
	public Texture2D powerupBar;
	public Texture2D statBox;
	public Texture2D obj_kill;
	public Texture2D obj_fire;
	public Texture2D obj_scavange;
	public Texture2D obj_damage;

	//Reno box variables
	private int levelBoxWidth = 100;
	private int levelBoxHeight = 50;
	
	//Reno button variables
	private int renoButtonWPlace = 90;
	private int renoButtonHPlace = 25;
	
	//Quit button variables
	private int gameQuitW = 100;
	private int gameQuitH = 25;
	
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

	public bool showObjectives = true;

	public float a,b,c,d =0;
	public int w,x,y,z = 0;
	//public int ww,xx,yy,zz = 0;
	//public int offset_y = 0;
	
	
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
		else if(currentScene == newReno)
		{
			///////////////////////////
			////////DRAWING HUD////////
			///////////////////////////

			//Drawing background box for stats and items
			GUI.DrawTexture( new Rect(7f,14f,179f,73f), statBox);

			//Drawing health bar and text for health
			if(playerState.GetHealth() >= 1)
			{
				GUI.DrawTexture( new Rect(79f,35f,100*(playerState.GetHealthPercent()),8f), healthBar);
				createText(79,15,200,30,"Health: "+playerState.health.ToString()+"/"+playerState.playerStats.max_health.ToString());
			}

			//Drawing stamina bar and text for stamina
			if(playerState.GetStaminaPercent()*100 >=1)
			{
				GUI.DrawTexture( new Rect(79f,55f,100*playerState.GetStaminaPercent(),8f), staminaBar);
				createText(79,38,200,30,"Stamina: "+playerState.stamina.ToString("F1")+"/"+playerState.playerStats.max_stamina.ToString("F1"));
			}

			//Drawing powerup bar and text for powerup
			if(playerState.GetPowerupPercent()*100 >=1)
			{
				GUI.DrawTexture( new Rect(79f,75,100*playerState.GetPowerupPercent(),8f), powerupBar);
				
				if(playerState.power_up.ToString() == "INVISIBILITY")
				{
					createText(80,59,200,30,"INVISIBILITY");
				}
				else if(playerState.power_up.ToString() == "BLINK")
				{
					createText(80,59,200,30,"BLINK");
				}
				
				else if(playerState.power_up.ToString() == "STUN")
				{
					createText(80,59,200,30,"STUN");
				}
				
				else if(playerState.power_up.ToString() == "SECOND_WIND")
				{
					createText(80,59,200,30,"SECOND WIND");
				}
				
				else if(playerState.power_up.ToString() == "INVULNERABLE")
				{
					createText(80,59,200,30,"INVULNERABLE");
				}
			}
			else
			{
				createText(80,59,200,30,"NO POWERUP");
			}

			///////////////////////////
			/////DRAWING OBJECTIVES////
			///////////////////////////

			//Drawing background box for primary and secondary objectives
			GUI.DrawTexture( new Rect(454,14f,179f,85), statBox);

			//Displaying primary objective (TIME)
			createText(465,30,100,100,"SURVIVE");
			createText(490,49,100,100,(gameState.primary_objective.target-gameState.primary_objective.current).ToString("F0"));

			//Displaying secondary objectives
			int yy = 0;
			int offset_y = 15;
			foreach(Objective objective in gameState.secondary_objectives)
			{

				if(!objective.completed)
				{
					if(objective.type == ObjectiveType.KILL)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_kill);
						createText(548,17+yy,100,100,objective.current.ToString()+"/"+objective.target.ToString());

						yy += offset_y;
						
					}
					else if(objective.type == ObjectiveType.FIRE)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_fire);
						createText(548,17+yy,100,100,objective.current.ToString()+"/"+objective.target.ToString());
						yy += offset_y;
							
					}
					else if(objective.type == ObjectiveType.SCAVENGER)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_scavange);
						createText(548,17+yy,100,100,objective.current.ToString()+"/"+objective.target.ToString());
						yy += offset_y;
							
					}
					else if(objective.type == ObjectiveType.DAMAGE)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_damage);
						createText(548,17+yy,100,100,objective.current.ToString()+"/"+objective.target.ToString());
						yy += offset_y;
							
					}
				}
			}

			///////////////////////////
			//////DRAWING BUTTONS//////
			///////////////////////////
			pauseButton = createButton(568,41,58,32, "PAUSE");
			
			//If the pause button is pressed open the pause menu (but for now just returns to main menu)
			if(pauseButton || Input.GetButton("Back"))
			{
				Application.LoadLevel(mainMenu);
			}
			
			//Displaying brainz and gainz score on HUD
			//createImage(59,50,48,23, brainz_icon);
			//createText(72,64,20,25, gameState.brainz.ToString());
		
			//createImage(98,48,20,27, gainz_icon);
			//createText(104,64,20,25, gameState.gainz.ToString());

			/*
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
			*/
			//Displaying power-up bar on HUD if player has one
			//float powerup_width = playerState.power_up_time_remaining *8;



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