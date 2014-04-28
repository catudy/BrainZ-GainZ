using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour 
{
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	public GUISkin instruction_skin;

	public Texture2D left_arrow;
	public Texture2D right_arrow;

	public int a,b,c,d = 0;

	private string current_screen = "How_to_play";

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		originalWidth = Screen.resolutions[0].width;
		originalHeight = Screen.resolutions[0].height;
	}

	void OnGUI()
	{
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z = 1;
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);
		GUI.skin = instruction_skin;

		if(current_screen == "How_to_play")
		{
			int offset = 0;

			GUI.Label(new Rect(325,60+offset,0,0),"HOW TO PLAY","Big");
			offset+=40;
			if(GUI.Button(new Rect(595,220,35,35), new GUIContent("", right_arrow, "")))
			{
				{
					current_screen = "Enemies";
				}
			}

			GUI.Label(new Rect(325,60+offset,0,0),"You have awakened to a zombie apocalypse and you must","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"use your brains and gains to survive as long as possible.","Normal");
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

		}
		if(current_screen == "Enemies")
		{
			int offset = 0;

			GUI.Label(new Rect(325,60+offset,0,0),"ENEMY TYPES","Big");
			offset+=40;
			if(GUI.Button(new Rect(10,220,35,35), new GUIContent("", left_arrow, "")))
			{
				{
					current_screen = "How_to_play";
				}
			}

			if(GUI.Button(new Rect(595,220,35,35), new GUIContent("", right_arrow, "")))
			{
				{
					current_screen = "Objectives";
				}
			}

			GUI.Label(new Rect(325,60+offset,0,0),"Wanderer","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"He walks around aimlessly. He may not be aware of his","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"surroundings but get close enough to him and you","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"may become his dinner.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Chaser","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"She is more vicious. She can smell tasty brains and","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"gains a mile away. Beware, once she's got her eye on","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"you she will chase you relentlessly.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Shooter","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"He shoots rockets that explode with enough force to","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"destroy buildings. Not only does it have a deadly","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"explosion radius but also leaves a lingering fire","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"that will leave you with third degree burns.","Normal");

		}

		if(current_screen == "Objectives")
		{
			int offset = 0;
			
			GUI.Label(new Rect(325,60+offset,0,0),"OBJECTIVES","Big");
			offset+=40;
			if(GUI.Button(new Rect(10,220,35,35), new GUIContent("", left_arrow, "")))
			{
				{
					current_screen = "Enemies";
				}
			}
			
			if(GUI.Button(new Rect(595,220,35,35), new GUIContent("", right_arrow, "")))
			{
				{
					current_screen = "Weapons";
				}
			}
			
			GUI.Label(new Rect(325,60+offset,0,0),"Primary objectives","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"Survive the required amount of time to","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"move on to the next level.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Secondary objectives","Big");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Secondary objectives dont need to be completed","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"but will significantly aid your chances of survival.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Kill - Kill a certain amount of zombies.","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"Scavange - Pick up a certain amount of items.","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"Fire - Put out a certain amount of fires.","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"Damage - Take a certain amount of damage","Normal");

			
		}

		if(current_screen == "Weapons")
		{
			int offset = 0;
			
			GUI.Label(new Rect(325,60+offset,0,0),"WEAPONS","Big");
			offset+=40;
			if(GUI.Button(new Rect(10,220,35,35), new GUIContent("", left_arrow, "")))
			{
				{
					current_screen = "Objectives";
				}
			}
			
			if(GUI.Button(new Rect(595,220,35,35), new GUIContent("", right_arrow, "")))
			{
				{
					current_screen = "Upgrades";
				}
			}
			
			GUI.Label(new Rect(325,60+offset,0,0),"Luckily you have brouhgt your trusty sword from home.","Normal");
			offset += 20;
			GUI.Label(new Rect(325,60+offset,0,0),"Other types of weapons can be scavanged in each level.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Melee - Powerful, indestructable, and hard to master.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Gun - Used to kill zombies with accuracy from a distance.","Normal");
			offset += 40;
			GUI.Label(new Rect(325,60+offset,0,0),"Pulse - Mysterious weapon that causes a deadly pulse.","Normal");
			offset+=40;
			GUI.Label(new Rect(325,60+offset,0,0),"Flamethrower - Fire speaks for itself.","Normal");
			offset+=40;
			GUI.Label(new Rect(325,60+offset,0,0),"Fire Extinguisher - Used to put out fires but nothing else.","Normal");
		}

		if(current_screen == "Upgrades")
		{
			int offset = 0;
			
			GUI.Label(new Rect(325,60+offset,0,0),"Upgrades","Big");
			offset+=40;
			if(GUI.Button(new Rect(10,220,35,35), new GUIContent("", left_arrow, "")))
			{
				{
					current_screen = "Weapons";
				}
			}
				
			GUI.Label(new Rect(325,60+offset,0,0),"Upgrades can be purchased from the aquired brains and gains","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"from completing objectives and other means.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Your weapons and personal stats can be upgraded.","Normal");
			offset+=40;
			GUI.Label(new Rect(325,60+offset,0,0),"Pickups","Big");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Various pickups can be found throughout the level","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"to aid in your survival.","Normal");
			offset+=40;
			GUI.Label(new Rect(325,60+offset,0,0),"Weapons - Each weapon scavanged comes with ammo.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Ammo pack - Restores all your weapon's ammo.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Med kit - Restores one health.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"BrainZ - Worth 10 brainZ.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"GainZ - Worth 10 gainZ.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Invulnerability - Unable to take damage for ten seconds.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Invisibility - Unable to attract zombies for ten seconds.","Normal");
			offset+=20;
			GUI.Label(new Rect(325,60+offset,0,0),"Second wind - Large boost in stamina regeneration","Normal");
		}
		if(GUI.Button(new Rect(4,453,55,22),"BACK"))
		{
			Application.LoadLevel("_MainMenu");
		}






























		GUI.matrix = svMat;
	}
}
