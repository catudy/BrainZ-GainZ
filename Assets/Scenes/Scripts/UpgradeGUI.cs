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
	private WeaponSystem weaponSystem;
	private ThirdPersonController thirdPersonController;

	//Resolution variables
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	//Upgrade background plane
	public GameObject upgradeBG;
	public GameObject hud;

	public int collected_brains = 0;
	public int collected_gains = 0;

	public bool showUpgradeMenu = false;
	public bool startNextLevel = false;
	public bool gameCompleted = false;

	public int upgrade_health_brain_cost = 25;
	public int upgrade_health_gain_cost = 25; 
	public int health_cost_increase_rate = 25;

	public int upgrade_stamina_brain_cost = 25;
	public int upgrade_stamina_gain_cost = 25;
	public int stamina_cost_increase_rate = 25;

	public int melee_speed_brain_cost = 25;
	public int melee_speed_gain_cost = 25;
	public int melee_speed_increase_rate = 25;

	public int gun_speed_brain_cost = 25;
	public int gun_speed_gain_cost = 25;
	public int gun_speed_increase_rate = 25;

	public int gun_ammo_brain_cost = 25;
	public int gun_ammo_gain_cost = 25;
	public int gun_ammo_increase_rate = 25;

	public int pulse_range_brain_cost = 25;
	public int pulse_range_gain_cost = 25;
	public int pulse_range_increase_rate = 25;

	public int pulse_ammo_brain_cost = 25;
	public int pulse_ammo_gain_cost = 25;
	public int pulse_ammo_increase_rate = 25;

	public int ft_ammo_brain_cost = 25;
	public int ft_ammo_gain_cost = 25;
	public int ft_ammo_increase_rate = 25;

	public int ft_range_brain_cost = 25;
	public int ft_range_gain_cost = 25;
	public int ft_range_increase_rate = 25;

	public int fe_ammo_brain_cost = 25;
	public int fe_ammo_gain_cost = 25;
	public int fe_ammo_increase_rate = 25;
	
	public int fe_range_brain_cost = 25;
	public int fe_range_gain_cost = 25;
	public int fe_range_increase_rate = 25;

	public int restorehp_brain_cost = 25;
	public int restorehp_gain_cost = 25;
	public int restoreammo_brain_cost = 25;
	public int restoreammo_gain_cost = 25;

	public int upgrade_speed_brain_cost = 25;
	public int upgrade_speed_gain_cost = 25;
	public int upgrade_speed_increast_rate = 25;

	public int a,b,c,d = 0;

	void Awake()
	{
		playerState = GameObject.Find("Player").GetComponentInChildren<PlayerState>();
		gameState = GameObject.Find("GameController").GetComponentInChildren<GameState>();
		weaponSystem = GameObject.Find("Player").GetComponentInChildren<WeaponSystem>();
		thirdPersonController = GameObject.Find("Player").GetComponentInChildren<ThirdPersonController>();
	}
	void Start()
	{

	}

	void Update()
	{
		//Get current resonlution
		originalWidth = Screen.resolutions[0].width;
		originalHeight = Screen.resolutions[0].height;

		if(gameState.primary_objective.completed)
		{
			//Despawn();

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
			/*
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
			*/
			if(!showUpgradeMenu)
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
							GUI.Label(new Rect(115,170+yy,100,100),"Complete: "+objective.target.ToString()+"/"+objective.target.ToString()+" ITEMS SCAVENGED","Basic");
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
					yy += offset_y;

				}

				GUI.Label(new Rect(115,170+yy,100,100),"Collected BrainZ: "+collected_brains,"Basic");
				yy+= offset_y;
				GUI.Label(new Rect(115,170+yy,100,100),"Collected GainZ: "+collected_gains,"Basic");

				GUI.Label(new Rect(115,400,100,100),"GainZ earned: "+(collected_gains+gainz_earned).ToString(),"Big");
				GUI.Label(new Rect(115,425,100,100),"BrainZ earned: "+(collected_brains+brainz_earned).ToString(),"Big");

				//Insert score screen code (completed objectives and current brainZ and gainZ acquired
				if(GUI.Button( new Rect(556,441,77,30),"Continue")) 
				{
					showUpgradeMenu = true;
				}
			}

			if(showUpgradeMenu)
			{
				collected_brains = 0;
				collected_gains = 0;

				//Upgrade menu
				GUI.Label( new Rect(265,10,0,0),"Upgrades","Big");

				//Draw current amount of brainZ and gainZ
				GUI.Label( new Rect(5,427,0,0),"Current total BrainZ: "+gameState.brainz.ToString(), "Big");
				GUI.Label( new Rect(5,456,0,0),"Current total GainZ: "+gameState.gainz.ToString(), "Big");

				//Drawing textures for stats
				GUI.DrawTexture( new Rect(25,96,20,20), HeartTexture);
				GUI.DrawTexture( new Rect(355,96,20,20), LifeupTexture);
				GUI.DrawTexture( new Rect(25,139,20,20), StaminaTexture);
				GUI.DrawTexture( new Rect(355,139,20,20), StaminaTexture);
				GUI.DrawTexture( new Rect(25,182,20,20), MeleeTexture);
				GUI.DrawTexture( new Rect(25,230,20,20), GunTexture);
				GUI.DrawTexture( new Rect(25,278,20,20), GunTexture);
				GUI.DrawTexture( new Rect(25,322,20,20), PulseTexture);
				GUI.DrawTexture( new Rect(25,370,20,20), PulseTexture);
				GUI.DrawTexture( new Rect(355,322,20,20), FTTexture);
				GUI.DrawTexture( new Rect(355,370,20,20), FTTexture);
				GUI.DrawTexture( new Rect(361,231,10,20), FETexture);
				GUI.DrawTexture( new Rect(361,273,10,20), FETexture);
				GUI.DrawTexture( new Rect(355,187,20,20), AmmoTexture);

				//Calculating current and next level stats to be displayed
				float current_health = playerState.playerStats.max_health;
				float next_health = playerState.playerStats.base_health + (playerState.playerStats.health_level);
				float current_stamina = playerState.playerStats.max_stamina;
				float next_stamina = playerState.playerStats.base_stamina + (playerState.playerStats.stamina_level);
				float current_m_atkspeed = weaponSystem.meleeAttackSpeed_max;
				float next_m_atkspeed = weaponSystem.meleeAttackSpeed_base - (weaponSystem.meleeAttackSpeed_level * 0.25f);
				float current_g_atkspeed = weaponSystem.fireRate_max;
				float next_g_atkspeed = weaponSystem.fireRate_base - weaponSystem.fireRate_level*0.1f;
				float current_g_ammo = weaponSystem.gunAmmo_max;
				float next_g_ammo = weaponSystem.gunAmmo_base + ((weaponSystem.gunAmmo_level) * 5); 
				float current_p_range = weaponSystem.pulse_radius_max;
				float next_p_range = weaponSystem.pulse_radius_base + ((weaponSystem.pulse_radius_level) * 25);
				float current_p_ammo = weaponSystem.pulseAmmo_max;
				float next_p_ammo = weaponSystem.pulseAmmo_base + ((weaponSystem.pulseAmmo_level) * 1);
				float current_ft_ammo = weaponSystem.flameAmmo_max;
				float next_ft_ammo = weaponSystem.flameAmmo_base + ((weaponSystem.flameAmmo_level) * 1.0f);
				float current_ft_range = weaponSystem.flamerange_max;
				float next_ft_range = weaponSystem.flamerange_base + ((weaponSystem.flamerange_level) * 1.0f);
				float current_fe_ammo = weaponSystem.feAmmo_max;
				float next_fe_ammo = weaponSystem.feAmmo_base + ((weaponSystem.feAmmo_level) * 1.0f);
				float current_fe_range = weaponSystem.ferange_max;
				float next_fe_range = weaponSystem.ferange_base + ((weaponSystem.ferange_level) * 1.0f);
				float current_walk = thirdPersonController.walkSpeed;
				float next_walk = thirdPersonController.walkSpeed + 0.2f;
				float current_run = thirdPersonController.runSpeed;
				float next_run = thirdPersonController.runSpeed + 0.3f;



				//Draw current and next level stats
				GUI.Label( new Rect(168,81,0,0), current_health+" => "+next_health+" Max Health", "Small");

				GUI.Label( new Rect(171,129,0,0),current_stamina+" => "+next_stamina+" Max Stamina" ,"Small");

				if(weaponSystem.meleeAttackSpeed_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,177,0,0),"MAX LEVEL: "+weaponSystem.meleeAttackSpeed_max.ToString("F2")+" Attack speed","Small");
				}
				else if (weaponSystem.meleeAttackSpeed_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,177,0,0),current_m_atkspeed+" => "+next_m_atkspeed.ToString("F2")+" Attack speed","Small"); 
				}

				if(weaponSystem.fireRate_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,225,0,0),"MAX LEVEL: "+weaponSystem.fireRate_max.ToString("F1")+" Attack speed","Small"); 
				}
				else if (weaponSystem.fireRate_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,225,0,0),current_g_atkspeed+" => "+next_g_atkspeed.ToString("F1")+" Attack speed","Small"); 
				}

				if(weaponSystem.gunAmmo_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,273,0,0),"MAX LEVEL: "+weaponSystem.gunAmmo_max.ToString()+" Ammo","Small"); 
				}
				else if (weaponSystem.gunAmmo_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,273,0,0),current_g_ammo+" => "+next_g_ammo.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}

				if(weaponSystem.pulse_radius_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,321,0,0),"MAX LEVEL: "+weaponSystem.pulse_radius_max.ToString()+" Range","Small"); //Print current and next stat for melee speed
				}
				else if (weaponSystem.pulse_radius_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,321,0,0),current_p_range+" => "+next_p_range.ToString()+" Range","Small"); //Print current and next stat for melee speed
				}

				if(weaponSystem.pulseAmmo_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,369,0,0),"MAX LEVEL: "+weaponSystem.pulseAmmo_max.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}
				else if (weaponSystem.pulseAmmo_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(171,369,0,0),current_p_ammo+" => "+next_p_ammo.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}

				if(weaponSystem.feAmmo_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,225,0,0),"MAX LEVEL: "+weaponSystem.feAmmo_max.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}
				else if (weaponSystem.feAmmo_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,225,0,0),current_fe_ammo+" => "+next_fe_ammo.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}
				//fe range
				if(weaponSystem.ferange_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,273,0,0),"MAX LEVEL: "+weaponSystem.ferange_max.ToString()+" Range","Small"); 
				}
				else if (weaponSystem.ferange_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,273,0,0),current_fe_range+" => "+next_fe_range.ToString()+" Range","Small"); //Print current and next stat for melee speed
				}

				if(weaponSystem.flameAmmo_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,321,0,0),"MAX LEVEL: "+weaponSystem.flameAmmo_max.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}
				else if (weaponSystem.flameAmmo_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,321,0,0),current_ft_ammo+" => "+next_ft_ammo.ToString()+" Ammo","Small"); //Print current and next stat for melee speed
				}
				//ftrange
				if(weaponSystem.flamerange_level == weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,369,0,0),"MAX LEVEL: "+weaponSystem.flamerange_max+" Range","Small"); 
				}
				else if (weaponSystem.flamerange_level < weaponSystem.weaponLevelCap)
				{
					GUI.Label( new Rect(500,369,0,0),current_ft_range+" => "+next_ft_range.ToString()+" Range","Small"); //Print current and next stat for melee speed
				}

				if(playerState.health < playerState.playerStats.max_health)
				{
					GUI.Label( new Rect(500,81,0,0), "RESTORE 1 HP: "+playerState.health+" => "+(playerState.health+1), "Small");
				}
				else
				{
					GUI.Label( new Rect(500,81,0,0), "FULL HEALTH: "+playerState.playerStats.max_health+" HP", "Small");
				}

				if(weaponSystem.isAmmoMaxed())
				{
					GUI.Label( new Rect(500,177,0,0),"AMMO IS MAXED OUT","Small");
				}
				else
				{
					GUI.Label( new Rect(500,177,0,0),"REFILL AMMO","Small"); 
				}

				GUI.Label( new Rect(442,129,0,0),"Walk: "+current_walk.ToString("F1")+" => "+next_walk.ToString("F1")+" | Run: "+current_run.ToString("F1")+" => "+next_run.ToString("F1") ,"Small");

				//Draw costs of each upgrade
				GUI.Label( new Rect(54,95,0,0),"BrainZ needed: "+upgrade_health_brain_cost, "Small");
				GUI.Label( new Rect(54,109,0,0),"GainZ needed: "+upgrade_health_gain_cost, "Small");
				GUI.Label( new Rect(54,140,0,0),"BrainZ needed: "+upgrade_stamina_brain_cost, "Small");
				GUI.Label( new Rect(54,154,0,0),"GainZ needed: "+upgrade_stamina_gain_cost, "Small");
				GUI.Label( new Rect(54,188,0,0),"BrainZ needed: "+melee_speed_brain_cost,"Small");
				GUI.Label( new Rect(54,202,0,0),"GainZ needed: "+melee_speed_gain_cost,"Small");
				GUI.Label( new Rect(54,230,0,0),"BrainZ needed: "+gun_speed_brain_cost,"Small");
				GUI.Label( new Rect(54,244,0,0),"GainZ needed: "+gun_speed_gain_cost,"Small");
				GUI.Label( new Rect(54,275,0,0),"BrainZ needed: "+gun_ammo_brain_cost,"Small");
				GUI.Label( new Rect(54,289,0,0),"GainZ needed: "+gun_ammo_gain_cost,"Small");
				GUI.Label( new Rect(54,320,0,0),"BrainZ needed: "+pulse_range_brain_cost,"Small");
				GUI.Label( new Rect(54,334,0,0),"GainZ needed: "+pulse_range_gain_cost,"Small");
				GUI.Label( new Rect(54,365,0,0),"BrainZ needed: "+pulse_ammo_brain_cost,"Small");
				GUI.Label( new Rect(54,379,0,0),"GainZ needed: "+pulse_ammo_gain_cost,"Small");
				GUI.Label( new Rect(386,320,0,0),"BrainZ needed: "+ft_ammo_brain_cost,"Small");
				GUI.Label( new Rect(386,334,0,0),"GainZ needed: "+ft_ammo_gain_cost,"Small");
				GUI.Label( new Rect(386,365,0,0),"BrainZ needed: "+ft_range_brain_cost,"Small");
				GUI.Label( new Rect(386,379,0,0),"GainZ needed: "+ft_range_gain_cost,"Small");
				GUI.Label( new Rect(386,230,0,0),"BrainZ needed: "+fe_ammo_brain_cost,"Small");
				GUI.Label( new Rect(386,244,0,0),"GainZ needed: "+fe_ammo_gain_cost,"Small");
				GUI.Label( new Rect(386,275,0,0),"BrainZ needed: "+fe_range_brain_cost,"Small");
				GUI.Label( new Rect(386,289,0,0),"GainZ needed: "+fe_range_gain_cost,"Small");
				GUI.Label( new Rect(386,95,0,0), "BrainZ needed: "+restorehp_brain_cost, "Small");
				GUI.Label( new Rect(386,109,0,0),"GainZ needed: "+restorehp_gain_cost, "Small");
				GUI.Label( new Rect(386,188,0,0),"BrainZ needed: "+restoreammo_brain_cost,"Small");
				GUI.Label( new Rect(386,202,0,0),"GainZ needed: "+restoreammo_gain_cost,"Small");
				GUI.Label( new Rect(386,140,0,0),"BrainZ needed: "+upgrade_speed_brain_cost, "Small");
				GUI.Label( new Rect(386,154,0,0),"GainZ needed: "+upgrade_speed_gain_cost, "Small");

				// Health upgrade pressed
				if(gameState.brainz >= upgrade_health_brain_cost && gameState.gainz >= upgrade_health_gain_cost)
				{
					if(GUI.Button( new Rect(168,95,25,25), new GUIContent("", LevelupB3, "")))
					{
						gameState.SpendBrainzNGainz(upgrade_health_brain_cost, upgrade_health_gain_cost);
						playerState.playerStats.health_level++;
						playerState.UpdateMaxHealth();
						playerState.health +=1;
						upgrade_health_brain_cost += health_cost_increase_rate;
						upgrade_health_gain_cost += health_cost_increase_rate;
					}
				}
				else
				{
					//Show something instead of the button?
					GUI.Button( new Rect(168,95,25,25), new GUIContent("", XTexture, ""));
				}

				//Stamina upgrade pressed
				if(gameState.brainz >= upgrade_stamina_brain_cost && gameState.gainz >= upgrade_stamina_gain_cost)
				{
					if(GUI.Button( new Rect(168,141,25,25), new GUIContent("", LevelupB2, "")))
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
					GUI.Button( new Rect(168,141,25,25), new GUIContent("", XTexture, ""));
				}

				//Melee attack speed upgrade pressed
				if(gameState.brainz >= melee_speed_brain_cost && gameState.gainz >= melee_speed_gain_cost && weaponSystem.meleeAttackSpeed_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(168,190,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(melee_speed_brain_cost, melee_speed_gain_cost);
						melee_speed_brain_cost += melee_speed_increase_rate;
						melee_speed_gain_cost += melee_speed_increase_rate;
						weaponSystem.meleeAttackSpeed_level++;
						weaponSystem.UpgradeMeleeAttackSpeed();
						weaponSystem.meleeAttackSpeed = weaponSystem.meleeAttackSpeed_max;

					}
				}
				else
				{
					GUI.Button( new Rect(168,190,25,25), new GUIContent("", XTexture, ""));
				}

				//Gun attack speed upgrade
				if(gameState.brainz >= gun_speed_brain_cost && gameState.gainz >= gun_speed_gain_cost && weaponSystem.fireRate_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(168,239,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(gun_speed_brain_cost, gun_speed_gain_cost);
						gun_speed_brain_cost += gun_speed_increase_rate;
						gun_speed_gain_cost += gun_speed_increase_rate;
						weaponSystem.fireRate_level++;
						weaponSystem.UpgradeFireRate();
						weaponSystem.fireRate = weaponSystem.fireRate_max;
						
					}
				}
				else
				{
					GUI.Button( new Rect(168,239,25,25), new GUIContent("", XTexture, ""));
				}

				//Gun ammo upgrade
				if(gameState.brainz >= gun_ammo_brain_cost && gameState.gainz >= gun_ammo_gain_cost && weaponSystem.gunAmmo_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(168,288,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(gun_ammo_brain_cost, gun_ammo_gain_cost);
						gun_ammo_brain_cost += gun_ammo_increase_rate;
						gun_ammo_gain_cost += gun_ammo_increase_rate;
						weaponSystem.gunAmmo_level++;
						weaponSystem.UpgradeGunAmmo();
						weaponSystem.gunAmmo = weaponSystem.gunAmmo_max;
						
					}
				}
				else
				{
					GUI.Button( new Rect(168,288,25,25), new GUIContent("", XTexture, ""));
				}

				//Pulse attack radius upgrade
				if(gameState.brainz >= pulse_range_brain_cost && gameState.gainz >= pulse_range_brain_cost && weaponSystem.pulse_radius_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(168,337,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(pulse_range_brain_cost, pulse_range_brain_cost);
						pulse_range_brain_cost += pulse_range_increase_rate;
						pulse_range_gain_cost += pulse_range_increase_rate;
						weaponSystem.pulse_radius_level++;
						weaponSystem.UpgradePulseRadius();
						weaponSystem.pulse_radius = weaponSystem.pulse_radius_max;
						
					}
				}
				else
				{
					GUI.Button( new Rect(168,337,25,25), new GUIContent("", XTexture, ""));
				}

				//Pulse ammo upgrade
				if(gameState.brainz >= pulse_ammo_brain_cost && gameState.gainz >= pulse_ammo_gain_cost && weaponSystem.pulseAmmo_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(168,386,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(pulse_ammo_brain_cost, pulse_ammo_gain_cost);
						pulse_ammo_brain_cost += pulse_ammo_increase_rate;
						pulse_ammo_gain_cost += pulse_ammo_increase_rate;
						weaponSystem.pulseAmmo_level++;
						weaponSystem.UpgradePulseAmmo();
						weaponSystem.pulseAmmo = weaponSystem.pulseAmmo_max;
						
					}
				}
				else
				{
					GUI.Button( new Rect(168,386,25,25), new GUIContent("", XTexture, ""));
				}

				//FE ammo upgrade
				if(gameState.brainz >= fe_ammo_brain_cost && gameState.gainz >= fe_ammo_gain_cost && weaponSystem.feAmmo_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(498,239,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(fe_ammo_brain_cost, fe_ammo_gain_cost);
						fe_ammo_brain_cost += fe_ammo_increase_rate;
						fe_ammo_gain_cost += fe_ammo_increase_rate;
						weaponSystem.feAmmo_level++;
						weaponSystem.UpgradeFEAmmo();
						weaponSystem.feAmmo = weaponSystem.feAmmo_max;	
					}
				}
				else
				{
					GUI.Button( new Rect(498,239,25,25), new GUIContent("", XTexture, ""));
				}


				if(gameState.brainz >= fe_range_brain_cost && gameState.gainz >= fe_range_gain_cost && weaponSystem.ferange_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(498,288,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(fe_range_brain_cost, fe_range_gain_cost);
						fe_range_brain_cost += fe_range_increase_rate;
						fe_range_gain_cost += fe_range_increase_rate;
						weaponSystem.ferange_level++;
						weaponSystem.UpgradeFERange();
						weaponSystem.ferange = weaponSystem.ferange_max;
					}
				}
				else
				{
					GUI.Button( new Rect(498,288,25,25), new GUIContent("", XTexture, ""));
				}


				//FT ammo upgrade
				if(gameState.brainz >= ft_ammo_brain_cost && gameState.gainz >= ft_ammo_gain_cost && weaponSystem.flameAmmo_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(498,337,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(ft_ammo_brain_cost, ft_ammo_gain_cost);
						ft_ammo_brain_cost += ft_ammo_increase_rate;
						ft_ammo_gain_cost += ft_ammo_increase_rate;
						weaponSystem.flameAmmo_level++;
						weaponSystem.UpgradeFlameAmmo();
						weaponSystem.flameAmmo = weaponSystem.flameAmmo_max;
					}
				}
				else
				{
					GUI.Button( new Rect(498,337,25,25), new GUIContent("", XTexture, ""));
				}


				if(gameState.brainz >= ft_range_brain_cost && gameState.gainz >= ft_range_gain_cost && weaponSystem.flamerange_level < weaponSystem.weaponLevelCap)
				{
					if(GUI.Button( new Rect(498,386,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(ft_range_brain_cost, ft_range_gain_cost);
						ft_range_brain_cost += ft_range_increase_rate;
						ft_range_gain_cost += ft_range_increase_rate;
						weaponSystem.flamerange_level++;
						weaponSystem.UpgradeFlameRange();
						weaponSystem.flamerange = weaponSystem.flamerange_max;
					}
				}
				else
				{
					GUI.Button( new Rect(498,386,25,25), new GUIContent("", XTexture, ""));
				}


				// Health +1 upgrade pressed
				if(gameState.brainz >= restorehp_brain_cost && gameState.gainz >= restorehp_gain_cost && playerState.health < playerState.playerStats.max_health)
				{
					if(GUI.Button( new Rect(498,95,25,25), new GUIContent("", LevelupB3, "")))
					{
						gameState.SpendBrainzNGainz(restorehp_brain_cost, restorehp_gain_cost);
						playerState.health += 1;
					}
				}
				else
				{
					//Show something instead of the button?
					GUI.Button( new Rect(498,95,25,25), new GUIContent("", XTexture, ""));
				}

				//Max ammo upgrade
				if(gameState.brainz >= restoreammo_brain_cost && gameState.gainz >= restoreammo_gain_cost && !weaponSystem.isAmmoMaxed())
				{
					if(GUI.Button( new Rect(498,190,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(melee_speed_brain_cost, restoreammo_gain_cost);
						weaponSystem.maxAmmo();
					}
				}
				else
				{
					GUI.Button( new Rect(498,190,25,25), new GUIContent("", XTexture, ""));
				}

				//Stamina upgrade pressed
				if(gameState.brainz >= upgrade_speed_brain_cost && gameState.gainz >= upgrade_speed_gain_cost)
				{
					if(GUI.Button( new Rect(498,141,25,25), new GUIContent("", LevelupB2, "")))
					{
						gameState.SpendBrainzNGainz(upgrade_speed_brain_cost, upgrade_speed_gain_cost);
						upgrade_speed_brain_cost += upgrade_speed_increast_rate;
						upgrade_speed_gain_cost += upgrade_speed_increast_rate;
						thirdPersonController.increaseWalk(0.2f);
						thirdPersonController.increaseSprint(0.3f);
					}
				}
				else
				{
					GUI.Button( new Rect(498,141,25,25), new GUIContent("", XTexture, ""));
				}





				//User is done applying upgrades
				if(GUI.Button( new Rect(589f,440f,45f,32f), new GUIContent("Done", null, "")))
				{
					//Increment game level and set new primary objective
					gameState.level++;
					playerState.power_up = PowerUp.NONE;
					playerState.power_up_time_remaining = 0.0f;
					GameObject.Find("baseMale").GetComponent<SkinnedMeshRenderer>().enabled = true;
					gameState.InitializeLevel();
					weaponSystem.currentWeapon = Weapon.MELEE;
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

	// TODO: Def Move later
	void Despawn(){
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Deadly")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Item")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Powerup")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Fire")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Effect")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Brainz")){
			Destroy(o);
		}
		foreach (GameObject o in GameObject.FindGameObjectsWithTag ("Gainz")){
			Destroy(o);
		}
	}
}