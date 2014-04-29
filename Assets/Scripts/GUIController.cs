using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour 
{
	//Variable for the current gamestate
	private GameState gameState;
	
	//Variable for the current player
	private PlayerState playerState;

	private WeaponSystem weaponSystem;

	public GUISkin menu_skin;
	public Texture2D bng_logo;
	
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
	public Texture2D meleeTexture, gunTexture, pulseTexture, feTexture, ftTexture , infinityTexture;

	public GUISkin stat_skin;

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
	private string mainMenu = "_MainMenu";
	private string newReno = "New_reno";
	
	//Pause button variables
	//private int gamePauseW = 100;
	//private int gamePauseH = 25;
	private int placeGamePauseW;
	private int placeGamePauseH; 
	
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	private bool showInstructions = false;

	//TEST VARS
	public float a,b,c,d =0;
	public int w,x,y,z = 0;
	
	void Start()
	{
		//Get the current scene and save it in the gamestate
		currentScene = Application.loadedLevelName;

		//Get reno level start variables
		if(currentScene == newReno)
		{
			//Getting gamestate and player state and gui variables
			gameState = GameObject.Find("GameController").GetComponent<GameState>();
			playerState = GameObject.Find("Player").GetComponent<PlayerState>();
			weaponSystem = GameObject.Find("Player").GetComponent<WeaponSystem>();
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
		if(currentScene == mainMenu && showInstructions)
		{
			GUI.skin = menu_skin;
			int offset = 0;
			GUI.Label(new Rect(325,60+offset,0,0),"How to play","Big");
			offset+=40;
			GUI.Label(new Rect(325,60+offset,0,0),"You have awakened to a zombie apocalypse and you must","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"use your brainz and gainz to survive as long as possible.","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"NEVER STOP RUNNING IF YOU DO, YOU DIE!","Normal");
			offset += 80;
			GUI.Label(new Rect(325,60+offset,0,0),"CONTROLS","Big");
			offset+=40;
			GUI.Label(new Rect(325,60+offset,0,0),"Q - SPRINT","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"R - SWAP WEAPONS","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"SPACEBAR - ATTACK","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"LEFT KEY - TURN LEFT","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"RIGHT KEY - TURN RIGHT","Normal");

			if(GUI.Button( new Rect(277,378,100,68), "Play"))
			{
				Application.LoadLevel("New_reno");
			}
		}
		else if(currentScene == mainMenu)
		{
			GUI.skin = menu_skin;
			GUI.Label(new Rect(325,60,0,0),"BRAINZ & GAINZ","Huge");

			GUI.DrawTexture( new Rect(138,-90,400,400), bng_logo);
			int offset = 0;
			if(GUI.Button( new Rect(288,347+offset,100,20), "Start"))
			{
				showInstructions = true;
			}
			offset+=40;
			if(GUI.Button( new Rect(288,347+offset,100,20), "Instructions"))
			{
				Application.LoadLevel("_Instructions");
			}
			offset+=40;
			if(GUI.Button( new Rect(288,347+offset,100,20), "Quit"))
			{
				Application.Quit();
			}
			/*
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
			*/
		}
		
		//Load GUI for reno level
		else if(currentScene == newReno)
		{
			///////////////////////////
			////////DRAWING HUD////////
			///////////////////////////

			GUI.skin = stat_skin;

			//Drawing background box for stats and items
			GUI.DrawTexture( new Rect(7f,14f,179f,73f), statBox);

			//Drawing health bar and text for health
			if(playerState.GetHealth() >= 1)
			{
				GUI.DrawTexture( new Rect(79f,28,100*(playerState.GetHealthPercent()),8f), healthBar);
				createText(79,16,200,30,"Health: "+playerState.health.ToString()+"/"+playerState.playerStats.max_health.ToString());
			}

			//Drawing stamina bar and text for stamina
			if(playerState.GetStaminaPercent()*100 >=1)
			{
				GUI.DrawTexture( new Rect(79f,50,100*playerState.GetStaminaPercent(),8f), staminaBar);

			}

			createText(79,38,200,30,"Stamina: "+playerState.stamina.ToString("F1")+"/"+playerState.playerStats.max_stamina.ToString("F1"));
			
			//Drawing powerup bar and text for powerup
			if(playerState.GetPowerupPercent()*100 >=1)
			{
				GUI.DrawTexture( new Rect(79f,72,100*playerState.GetPowerupPercent(),8f), powerupBar);
				
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

			//Draw melee weapon and stats
			if(weaponSystem.currentWeapon == 0)
			{
				GUI.DrawTexture( new Rect(20,19,45,45), meleeTexture);
				//GUI.DrawTexture( new Rect(38,63,20,20), infinityTexture);
			}

			else if(weaponSystem.currentWeapon == Weapon.GUN)
			//Draw gun weapon and stats
			{
				GUI.DrawTexture( new Rect(21,19,45,45), gunTexture);
				createText(34,69,200,20,weaponSystem.gunAmmo.ToString()+"/"+weaponSystem.gunAmmo_max.ToString());
			}

			//Draw pulse weapon and stats
			else if(weaponSystem.currentWeapon == Weapon.PULSE)
			{
				GUI.DrawTexture( new Rect(23,19,45,45), pulseTexture);
				createText(34,69,200,20,weaponSystem.pulseAmmo.ToString()+"/"+weaponSystem.pulseAmmo_max.ToString());
			}

			//Draw flamethrower and stats
			else if(weaponSystem.currentWeapon == Weapon.FLAMER)
			{
				GUI.DrawTexture( new Rect(24,20,40,40), ftTexture);
				createText(34,69,200,20,weaponSystem.flameAmmo.ToString("F2")+"/"+weaponSystem.flameAmmo_max.ToString("F2"));
			}

			//Draw fireextinguisher and stats
			else if(weaponSystem.currentWeapon == Weapon.EXTINGUISHER)
			{
				GUI.DrawTexture( new Rect(34,22,20,40), feTexture);
				createText(34,69,200,20,weaponSystem.feAmmo.ToString("F2")+"/"+weaponSystem.feAmmo_max.ToString("F2"));
			}

			//Drawing background box for primary and secondary objectives
			GUI.DrawTexture( new Rect(454,14f,179f,85), statBox);

			//Displaying primary objective (TIME)
			GUI.Label(new Rect(462,22,100,100), "SURVIVE","obj_style");

			GUI.Label(new Rect(460,49,100,100),(gameState.primary_objective.target-gameState.primary_objective.current).ToString("F0"),"prim_style");

			GUI.Label(new Rect(591,22,100,100),"LVL","obj_style");
			GUI.Label(new Rect(554,3,100,100), gameState.level.ToString(),"lvl_style");

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
						GUI.Label(new Rect(548,21+yy,100,100),objective.current.ToString()+"/"+objective.target.ToString(),"obj_style");

						yy += offset_y;
						
					}
					else if(objective.type == ObjectiveType.FIRE)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_fire);
						GUI.Label(new Rect(548,21+yy,100,100),objective.current.ToString()+"/"+objective.target.ToString(),"obj_style");

						yy += offset_y;
							
					}
					else if(objective.type == ObjectiveType.SCAVENGER)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_scavange);
						GUI.Label(new Rect(548,21+yy,100,100),objective.current.ToString()+"/"+objective.target.ToString(),"obj_style");

						yy += offset_y;
							
					}
					else if(objective.type == ObjectiveType.DAMAGE)
					{
						GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_damage);
						GUI.Label(new Rect(548,21+yy,100,100),objective.current.ToString()+"/"+objective.target.ToString(),"obj_style");

						yy += offset_y;
							
					}
				}
			}

			///////////////////////////
			//////DRAWING BUTTONS//////
			///////////////////////////
			pauseButton = createButton(585,73,43,20, "PAUSE");
			
			//If the pause button is pressed open the pause menu (but for now just returns to main menu)
			if(pauseButton && !gameState.paused)
			{
				gameState.paused = true;
				//Application.LoadLevel(mainMenu);
			}
			else if (pauseButton && gameState.paused)
			{
				gameState.paused = false;
			}

			if(gameState.paused)
			{
				GUI.Box(new Rect(170,140,300,200),"PAUSED");
				if(GUI.Button( new Rect(270,240,100,50), "Quit"))
				{
					Application.LoadLevel("_MainMenu");
				}
				if(GUI.Button( new Rect(270,175,100,50), "Resume"))
				{
					gameState.paused = false;
				}
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