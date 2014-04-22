 using UnityEngine;
using System.Collections;

public class UpgradeGUI : MonoBehaviour
{
	public Texture2D HeartTexture, StaminaTexture;
	public Texture2D LevelupB1, LevelupB2, LevelupB3;
	public Texture2D MeleeTexture, GunTexture, PulseTexture, FTTexture, FETexture;
	public Texture2D AmmoTexture, LifeupTexture , XTexture;

	public GUISkin upgrade_skin;

	private PlayerState playerState;
	private GameState gameState;
	private GameObject player;

	//Resolution variables
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	//Upgrade background plane
	public GameObject upgradeBG;
	public GameObject hud;

	public bool showUpgradeMenu = false;
	public bool startNextLevel = false;
	public bool gameCompleted = false;

	private int upgrade_health_brain_cost = 25;
	private int upgrade_health_gain_cost = 25;
	private int health_cost_increase_rate = 25;

	private int upgrade_stamina_brain_cost = 25;
	private int upgrade_stamina_gain_cost = 25;
	private int stamina_cost_increase_rate = 25;

	private int melee_speed_brain_cost = 25;
	private int melee_speed_gain_cost = 25;
	private int melee_speed_increase_rate = 25;

	public int melee_range_brain_cost = 25;
	public int melee_range_gain_cost = 25;

	public int a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z = 0;

	
	void Start()
	{
		player = GameObject.Find("Player");
		playerState = GameObject.Find("Player").GetComponentInChildren<PlayerState>();
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
	}

	void Update()
	{
		//Get current resonlution
		originalWidth = Screen.resolutions[0].width;
		originalHeight = Screen.resolutions[0].height;

		if(gameState.primary_objective.completed)
		{
			//Toggle background plane for upgrade menu
			upgradeBG.SetActive(true);
			//Code to disable GUI
			hud.SetActive(false);

		}
		else if(gameState.game_over)
		{
			hud.SetActive(false);
			upgradeBG.SetActive(true);
		}
		else if(!gameState.primary_objective.completed && gameCompleted)
		{
			upgradeBG.SetActive(false);
			hud.SetActive(false);
		}
		else if(!gameState.primary_objective.completed)
		{
			//Hide background plane for upgrade menu
			upgradeBG.SetActive(false);
			//Code to activate GUI
			hud.SetActive(true);
		}
	}
	void OnGUI()
	{
		//Resize GUI based on resolution
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z = 1;

		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);

		GUI.skin = upgrade_skin;

		if(gameState.primary_objective.completed)
		{
			gameState.paused = true;

			//Change to 7 or 8 later
			if(gameState.level == 8)//gameState.level == 2 || gameState.level == 8)
			{
				GUI.Label( new Rect(85f,100f,400f,100f), new GUIContent("You beat the game! Your gains are superior!", null, "Basic"));
				//Insert score screen code (completed objectives and current brainZ and gainZ acquired
				if(GUI.Button( new Rect(250,300,100,100),"Main Menu"))
				{
					Application.LoadLevel("_MainMenu");
				}
			}

			else if(!showUpgradeMenu)
			{
				//Level X completed message
				GUI.Label(new Rect(240,20,300,100), new GUIContent("Level "+gameState.level.ToString()+" Complete", null, "Basic"));
				GUI.Label(new Rect(115,115,400,100), new GUIContent("SECONDARY OBJECTIVES COMPLETED",null,"Basic"));

				//Displaying secondary objectives
				int yy = 0;
				int offset_y = 30;
				int gainz_earned = 0;
				int brainz_earned = 0;
				int sec_obj_comp = 0;

				foreach(Objective objective in gameState.secondary_objectives)
				{
					if(objective.completed)
					{
						if(objective.type == ObjectiveType.KILL)
						{
							//GUI.DrawTexture( new Rect(528,20+yy,15,15), obj_kill);

							GUI.Label(new Rect(115,170+yy,100,100),"Complete: "+objective.target.ToString()+"/"+objective.target.ToString()+" ZOMBIES KILLED","Basic");
							GUI.Label(new Rect(405,170+yy,400,100),"Reward: "+objective.reward_amount.ToString()+" "+objective.reward.ToString(),"Basic");
							gainz_earned += objective.reward_amount;

							yy += offset_y;   
							sec_obj_comp +=1;
						}
							          
						else if(objective.type == ObjectiveType.FIRE)
						{
							GUI.Label(new Rect(115,170+yy,100,100),"Complete: "+objective.target.ToString()+"/"+objective.target.ToString()+" FIRES PUT OUT","Basic");
							GUI.Label(new Rect(405,170+yy,400,100),"Reward: "+objective.reward_amount.ToString()+" "+objective.reward.ToString(),"Basic");
							brainz_earned += objective.reward_amount;
							
							yy += offset_y;
							sec_obj_comp +=1;						
						}
						else if(objective.type == ObjectiveType.SCAVENGER)
						{
							GUI.Label(new Rect(115,170+yy,100,100),"Complete: "+objective.target.ToString()+"/"+objective.target.ToString()+" SECRET ITEMS FOUND","Basic");
							GUI.Label(new Rect(405,170+yy,400,100),"Reward: "+objective.reward_amount.ToString()+" "+objective.reward.ToString(),"Basic");
							brainz_earned += objective.reward_amount;

							yy += offset_y;
							sec_obj_comp +=1;	
						}
						else if(objective.type == ObjectiveType.DAMAGE)
						{

							GUI.Label(new Rect(115,170+yy,100,100),"Complete: "+objective.target.ToString()+"/"+objective.target.ToString()+" DAMAGE TAKEN","Basic");
							GUI.Label(new Rect(405,170+yy,400,100),"Reward: "+objective.reward_amount.ToString()+" "+objective.reward.ToString(),"Basic");
							gainz_earned += objective.reward_amount;

							yy += offset_y;
							sec_obj_comp +=1;
						}
					}
				}

				if(sec_obj_comp == 0)
				{
					GUI.Label(new Rect(115,170,400,100), new GUIContent("NONE COMPLETED",null,"Basic"));
				}

				GUI.Label(new Rect(115,400,100,100),"GainZ earned: "+gainz_earned.ToString(),"Big");
				GUI.Label(new Rect(115,425,100,100),"BrainZ earned: "+brainz_earned.ToString(),"Big");
				
				
				//Insert score screen code (completed objectives and current brainZ and gainZ acquired
				if(GUI.Button( new Rect(556,441,77,30),"Continue")) 
				{
					showUpgradeMenu = true;
				}
			}

			if(showUpgradeMenu)
			{
				//Upgrade menu
				GUI.Label( new Rect(265,10,0,0),"Upgrades","Big");

				//Drawing textures for stats
				GUI.DrawTexture( new Rect(25,50,20,20), HeartTexture);
				GUI.DrawTexture( new Rect(25,96,20,20), StaminaTexture);
				GUI.DrawTexture( new Rect(25,139,20,20), MeleeTexture);
				//GUI.DrawTexture( new Rect(0,0,100,100), GunTexture);
				//GUI.DrawTexture( new Rect(0,0,100,100), PulseTexture);
				//GUI.DrawTexture( new Rect(0,0,100,100), FTTexture);
				//GUI.DrawTexture( new Rect(0,0,100,100), FETexture);
				//GUI.DrawTexture( new Rect(0,0,100,100), AmmoTexture);
				//GUI.DrawTexture( new Rect(0,0,100,100), LifeupTexture);

				//Calculating current and next level stats to be displayed
				float current_health = playerState.playerStats.max_health;
				float next_health = playerState.playerStats.base_health + (playerState.playerStats.health_level);
				float current_stamina = playerState.playerStats.max_stamina;
				float next_stamina = playerState.playerStats.base_stamina + (playerState.playerStats.stamina_level);

				//Draw current and next level stats
				GUI.Label( new Rect(168,33,0,0), current_health+" => "+next_health+" Max Health", "Small");
				GUI.Label( new Rect(171,81,0,0),current_stamina+" => "+next_stamina+" Max Stamina" ,"Small");
				GUI.Label( new Rect(171,129,0,0),"XX => YY Attack"+" Speed","Small"); //Print current and next stat for melee speed

				//Draw current amount of brainZ and gainZ
				GUI.Label( new Rect(5,456,0,0),"Current total BrainZ: "+gameState.brainz.ToString(), "Big");
				GUI.Label( new Rect(5,427,0,0),"Current total GainZ: "+gameState.gainz.ToString(), "Big");

				//Draw costs of each upgrade
				GUI.Label( new Rect(54,50,0,0), "BrainZ needed: "+upgrade_health_brain_cost, "Small");
				GUI.Label( new Rect(54,64,0,0), "GainZ needed: "+upgrade_health_gain_cost, "Small");
				GUI.Label( new Rect(54,109,0,0),"BrainZ needed: "+upgrade_stamina_brain_cost, "Small");
				GUI.Label( new Rect(54,95,0,0),"GainZ needed: "+upgrade_stamina_gain_cost, "Small");
				GUI.Label( new Rect(54,140,0,0),"BrainZ needed: "+melee_speed_brain_cost,"Small");
				GUI.Label( new Rect(54,154,0,0),"GainZ needed: "+melee_speed_gain_cost,"Small");

				// Health upgrade pressed
				if(gameState.brainz >= upgrade_health_brain_cost && gameState.gainz >= upgrade_health_gain_cost)
				{
					if(GUI.Button( new Rect(168,49,25,25), new GUIContent("", LevelupB3, "")))
					{
						gameState.SpendBrainzNGainz(upgrade_health_brain_cost, upgrade_health_gain_cost);
						playerState.playerStats.health_level++;
						playerState.UpdateMaxHealth();
						upgrade_health_brain_cost += health_cost_increase_rate;
						upgrade_health_gain_cost += health_cost_increase_rate;
					}
				}
				else
				{
					//Show something instead of the button?
					GUI.Button( new Rect(168,49,25,25), new GUIContent("", XTexture, ""));
				}

				//Stamina upgrade pressed
				if(gameState.brainz >= upgrade_stamina_brain_cost && gameState.gainz >= upgrade_stamina_gain_cost)
				{
					if(GUI.Button( new Rect(168,95,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(upgrade_stamina_brain_cost, upgrade_stamina_gain_cost);
						upgrade_stamina_brain_cost += stamina_cost_increase_rate;
						upgrade_stamina_gain_cost += stamina_cost_increase_rate;
						playerState.playerStats.stamina_level++;
						playerState.UpdateMaxStamina();
					}
				}
				else
				{
					GUI.Button( new Rect(168,95,25,25), new GUIContent("", XTexture, ""));
				}

				//Melee attack speed upgrade pressed
				if(gameState.brainz >= melee_speed_brain_cost && gameState.gainz >= melee_speed_gain_cost)
				{
					if(GUI.Button( new Rect(168,141,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(melee_speed_brain_cost, melee_speed_gain_cost);
						melee_speed_brain_cost += melee_speed_increase_rate;
						melee_speed_gain_cost += melee_speed_increase_rate;
						//Increment melee speed level
						//Call updatemaxmeleespeed()

					}
				}
				else
				{
					GUI.Button( new Rect(168,141,25,25), new GUIContent("", XTexture, ""));
				}

				//Add other upgrades here but follow format above. 














				//User is done applying upgrades
				if(GUI.Button( new Rect(589f,440f,45f,32f), new GUIContent("Done", null, "")))
				{
					//Increment game level and set new primary objective
					gameState.level++;
					playerState.power_up = PowerUp.NONE;
					playerState.power_up_time_remaining = 0.0f;
					GameObject.Find("baseMale").GetComponent<SkinnedMeshRenderer>().enabled = true;
					gameState.InitializeLevel();
					showUpgradeMenu = false;
				}
			}
		}

		else if(!gameState.primary_objective.completed && gameState.game_over)
		{
			gameState.paused = true;

			GUI.Label( new Rect(85f,100f,400f,100f), new GUIContent("You lose!", null, ""));
			//Insert score screen code (completed objectives and current brainZ and gainZ acquired
			if(GUI.Button( new Rect(250,300,100,100),"Main Menu"))
			{
				Application.LoadLevel("_MainMenu");
			}
		}

		GUI.matrix = svMat;
	}
}