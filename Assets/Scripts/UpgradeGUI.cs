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

	public bool showUpgradeMenu = false;

	private PlayerState playerState;
	private GameState gameState;
	private ObjectiveType objectiveType;

	//Resolution variables
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	//Upgrade background plane
	public GameObject upgradeBG;
	public GameObject hud;

	//test vars
	public float a,b,c,d = 0;

	//Placement test variables
	//public float x = 0;
	//public float y = 0;
	//public float w = 0;
	//public float h = 0;

	void Start()
	{
		playerState = GameObject.Find("Player").GetComponentInChildren<PlayerState>();
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
		//objectiveType = GameObject.Find ("GameController").GetComponentInChildren<ObjectiveType>();
	}

	void Update()
	{
		//Get current resonlution
		originalWidth = Screen.resolutions[0].width;
		originalHeight = Screen.resolutions[0].height;

		if(gameState.primary_objective.completed)//showUpgradeMenu)
		{
			//Toggle background plane for upgrade menu
			upgradeBG.SetActive(true);
			//Code to disable GUI
			hud.SetActive(false);

		}
		else
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

		if(gameState.primary_objective.completed)//showUpgradeMenu)
		{
			//Draw upgrade texture logos

			GUI.DrawTexture( new Rect(75f, 254f, 100f, 100f), HeartTexture);
			GUI.DrawTexture( new Rect(250f, 254f, 100f, 100f), StaminaTexture);
			GUI.DrawTexture( new Rect(350f, 185f, 250f, 250f), WeaponLogo);

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

			GUI.Label( new Rect(66f,429f,200f,100f), new GUIContent("BrainZ needed: "+playerState.health_up_bcost, null, ""));
			GUI.Label( new Rect(66f,448f,200f,100f), new GUIContent("GainZ needed: "+playerState.health_up_gcost, null, ""));

			GUI.Label( new Rect(241f,429f,200f,100f), new GUIContent("BrainZ needed: "+playerState.stamina_up_bcost, null, ""));
			GUI.Label( new Rect(241f,448f,200f,100f), new GUIContent("GainZ needed: "+playerState.stamina_up_gcost, null, ""));

			GUI.Label( new Rect(432f,429f,200f,100f), new GUIContent("BrainZ needed: "+playerState.weapon_up_bcost, null, ""));
			GUI.Label( new Rect(432f,448f,200f,100f), new GUIContent("GainZ needed: "+playerState.weapon_up_gcost, null, ""));

			//Draw upgrade button logos 

			// Weapon upgrade pressed
			if(gameState.brainz >= playerState.weapon_up_bcost && gameState.gainz >= playerState.weapon_up_gcost)
			{
				if(GUI.Button( new Rect(452,379, 50f, 50f), new GUIContent("", LevelupB1, "")))
				{
					//Call code that upgrades the players weapon damage modifier
					gameState.brainz = gameState.brainz - playerState.weapon_up_bcost;
					gameState.gainz = gameState.gainz - playerState.weapon_up_gcost;
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
			if(gameState.brainz >= playerState.stamina_up_bcost && gameState.gainz >= playerState.stamina_up_gcost)
			{
				if(GUI.Button( new Rect(272f,379f,50f,50f), new GUIContent("", LevelupB2, "")))
				{
					//Call code that upgrades the players stamina duration or speed modifier
					gameState.brainz = gameState.brainz - playerState.stamina_up_bcost;
					gameState.gainz = gameState.gainz - playerState.stamina_up_gcost;
					playerState.playerStats.stamina_level++;
					playerState.UpdateMaxStamina();
				}
			}
			else
			{
				//Show something instead of the button?
			}

			// Health upgrade pressed
			if(gameState.brainz >= playerState.health_up_bcost && gameState.gainz >= playerState.health_up_gcost)
			{
				if(GUI.Button( new Rect(98f,379f,50f,50f), new GUIContent("", LevelupB3, "")))
				{
					//Call the code that upgrades the players maximum health
					gameState.brainz = gameState.brainz - playerState.health_up_bcost;
					gameState.gainz = gameState.gainz - playerState.health_up_gcost;
					playerState.playerStats.health_level++;
					playerState.UpdateMaxHealth();
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
				gameState.primary_objective.SetObjective(ObjectiveType.TIME, ObjectiveReward.NONE, 30*gameState.level, 0);


				gameState.primary_objective.completed = false;
				gameState.primary_objective.done = false;

				//showUpgradeMenu = false;
				//Call code that scene transitions and set apporpriate variables for next level
			}
		}

		else
		{
		}

		GUI.matrix = svMat;
	}
}