 using UnityEngine;
using System.Collections;

public class UpgradeGUI : MonoBehaviour
{
	public Texture2D HeartTexture;
	public Texture2D StaminaTexture;
	public Texture2D WeaponLogo;
	public Texture2D LevelupB1;
	public Texture2D LevelupB2;
	public Texture2D LevelupB3;

	private PlayerState playerState;
	private GameState gameState;
	public GameObject player;

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

	public int upgrade_health_brain_cost = 25;
	public int upgrade_health_gain_cost = 25;
	public int health_cost_increase_rate = 25;
	public int upgrade_stamina_brain_cost = 25;
	public int upgrade_stamina_gain_cost = 25;
	public int stamina_cost_increase_rate = 25;
	public int upgrade_weapon_brain_cost = 25;
	public int upgrade_weapon_gain_cost = 25;
	public int weapon_cost_increase_rate = 25;

	
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

		if(gameState.primary_objective.completed)
		{
			gameState.paused = true;

			/*if(gameState.level == 4)
			{
				GUI.Label( new Rect(85f,100f,400f,100f), new GUIContent("You beat the game! Your gains are superior!", null, ""));
				//Insert score screen code (completed objectives and current brainZ and gainZ acquired
				if(GUI.Button( new Rect(250,300,100,100),"Main Menu"))
				{
					Application.LoadLevel("_MainMenu");
				}
			}*/

			if(!showUpgradeMenu)
			{
				//Level X completed message
				GUI.Label( new Rect(85f,100f,400f,100f), new GUIContent("Congratulations! Level "+gameState.level.ToString()+" completed", null, ""));

				//Insert score screen code (completed objectives and current brainZ and gainZ acquired
				if(GUI.Button( new Rect(250,300,100,100),"Continue")) 
				{
					showUpgradeMenu = true;
				}
			}



			if(showUpgradeMenu)
			{
				//Fade out and fade in again and show the upgrade menu
				GUI.DrawTexture( new Rect(75f, 254f, 100f, 100f), HeartTexture);
				GUI.DrawTexture( new Rect(250f, 254f, 100f, 100f), StaminaTexture);
				GUI.DrawTexture( new Rect(350f, 185f, 250f, 250f), WeaponLogo);

				//GUI.Box(new Rect(a,b,c,d),"");

				float current_wepdmg = playerState.playerStats.weapon_damage;
				float next_wepdmg = playerState.playerStats.base_wepon_damage + (0.5f*(playerState.playerStats.weapon_level+1)-0.5f);
				float current_health = playerState.playerStats.max_health;
				float next_health = playerState.playerStats.base_health + (playerState.playerStats.health_level);
				float current_stamina = playerState.playerStats.max_stamina;
				float next_stamina = playerState.playerStats.base_stamina + (playerState.playerStats.stamina_level);

				//Draw upgrade text logos
				//Edit these to show the actual level values
				GUI.Label( new Rect(92f, 195f, 250f, 250f), new GUIContent(current_health+"->"+next_health, null, ""));
				GUI.Label( new Rect(262f, 195f, 250f, 250f), new GUIContent(current_stamina+"->"+next_stamina, null, ""));
				GUI.Label( new Rect(439, 195f, 250f, 250f), new GUIContent(current_wepdmg+"->"+next_wepdmg, null, ""));
				GUI.Label( new Rect(280f, 25f, 77f, 50f), new GUIContent("Upgrades", null, ""));

				GUI.Label( new Rect(85f,100f,400f,100f), new GUIContent("Current total BrainZ: "+gameState.brainz.ToString(), null, ""));
				GUI.Label( new Rect(400f,100f,400f,100f), new GUIContent("Current total GainZ: "+gameState.gainz.ToString(), null, ""));

				GUI.Label( new Rect(66f,429f,200f,100f), new GUIContent("BrainZ needed: "+upgrade_health_brain_cost, null, ""));
				GUI.Label( new Rect(66f,448f,200f,100f), new GUIContent("GainZ needed: "+upgrade_health_gain_cost, null, ""));

				GUI.Label( new Rect(241f,429f,200f,100f), new GUIContent("BrainZ needed: "+upgrade_stamina_brain_cost, null, ""));
				GUI.Label( new Rect(241f,448f,200f,100f), new GUIContent("GainZ needed: "+upgrade_stamina_gain_cost, null, ""));

				GUI.Label( new Rect(432f,429f,200f,100f), new GUIContent("BrainZ needed: "+upgrade_weapon_brain_cost, null, ""));
				GUI.Label( new Rect(432f,448f,200f,100f), new GUIContent("GainZ needed: "+upgrade_weapon_gain_cost, null, ""));

				//Draw upgrade button logos 

				// Weapon upgrade pressed
				if(gameState.brainz >= upgrade_weapon_brain_cost && gameState.gainz >= upgrade_weapon_gain_cost)
				{
					//GUI.Label( new Rect(418f,357f,117f,20f), new GUIContent("Press B to level up", null, ""));

					if(GUI.Button( new Rect(452,379, 50f, 50f), new GUIContent("", LevelupB1, "")))
					{
						//Call code that upgrades the players weapon damage modifier
						gameState.SpendBrainzNGainz(upgrade_weapon_brain_cost, upgrade_weapon_gain_cost);
						upgrade_weapon_brain_cost += weapon_cost_increase_rate;
						upgrade_weapon_gain_cost += weapon_cost_increase_rate;
						playerState.playerStats.weapon_level++;
						playerState.UpdateMaxWeaponDamage();
					}
				}
				else
				{
					//Show something instead of the button?
				}

				//Stamina upgrade pressed
				//Check to see if enough brainZ or gainZ aquired
				if(gameState.brainz >= upgrade_stamina_brain_cost && gameState.gainz >= upgrade_stamina_gain_cost)
				{
					//GUI.Label( new Rect(237f,357f,117f,20f), new GUIContent("Press A to level up", null, ""));

					if(GUI.Button( new Rect(272f,379f,50f,50f), new GUIContent("", LevelupB2, "")))
					{
						//Call code that upgrades the players stamina duration or speed modifier
						gameState.SpendBrainzNGainz(upgrade_stamina_brain_cost, upgrade_stamina_gain_cost);
						upgrade_stamina_brain_cost += stamina_cost_increase_rate;
						upgrade_stamina_gain_cost += stamina_cost_increase_rate;
						playerState.playerStats.stamina_level++;
						playerState.UpdateMaxStamina();
					}
				}
				else
				{
					//Show something instead of the button?
				}

				// Health upgrade pressed
				if(gameState.brainz >= upgrade_health_brain_cost && gameState.gainz >= upgrade_health_gain_cost)
				{
					//GUI.Label( new Rect(66f,357f,117f,20f), new GUIContent("Press X to level up", null, ""));

					if(GUI.Button( new Rect(98f,379f,50f,50f), new GUIContent("", LevelupB3, "")))
					{
						//Call the code that upgrades the players maximum health
						gameState.SpendBrainzNGainz(upgrade_health_brain_cost, upgrade_weapon_gain_cost);
						playerState.playerStats.health_level++;
						playerState.UpdateMaxHealth();
						upgrade_health_brain_cost += health_cost_increase_rate;
						upgrade_health_gain_cost += health_cost_increase_rate;
					}
				}
				else
				{
					//Show something instead of the button?
				}

				//User is done applying upgrades
				if(GUI.Button( new Rect(589f,440f,45f,32f), new GUIContent("Done", null, "")))
				{
					//Increment game level and set new primary objective
					gameState.level++;
					gameState.InitializeLevel();
					showUpgradeMenu = false;
				}
			}
		}

		else
		{
		}

		GUI.matrix = svMat;
	}
}