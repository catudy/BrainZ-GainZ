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
	public bool showUpgradeMenu = true;
	//Private PlayerState playerState;

	//Resolution variables
	private float originalWidth;
	private float originalHeight;
	private Vector3 scale;

	//Upgrade background plane
	public GameObject upgradeBG;

	//Placement test variables
	//public float x = 0;
	//public float y = 0;
	//public float w = 0;
	//public float h = 0;

	void Start()
	{
		//playerState = GameObject.Find("Player").GetComponenet(PlayerState);
	}

	void Update()
	{
		//Get current resonlution
		originalWidth = Screen.resolutions[0].width;
		originalHeight = Screen.resolutions[0].height;
	}
	void OnGUI()
	{
		//Resize GUI based on resolution
		scale.x = Screen.width/originalWidth;
		scale.y = Screen.height/originalHeight;
		scale.z = 1;
		
		Matrix4x4 svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scale);

		if(showUpgradeMenu)
		{
			//Toggle background plane for upgrade menu
			upgradeBG.SetActive(true);

			//Draw upgrade texture logos

			GUI.DrawTexture( new Rect(75f, 254f, 100f, 100f), HeartTexture);
			GUI.DrawTexture( new Rect(250f, 254f, 100f, 100f), StaminaTexture);
			GUI.DrawTexture( new Rect(350f, 185f, 250f, 250f), WeaponLogo);

			//Draw upgrade text logos
			//Edit these to show the actual level values
			GUI.Label( new Rect(92f, 195f, 250f, 250f), new GUIContent("HealthLevel", null, ""));
			GUI.Label( new Rect(262f, 195f, 250f, 250f), new GUIContent("StaminaLevel", null, ""));
			GUI.Label( new Rect(439, 195f, 250f, 250f), new GUIContent("WeaponLevel", null, ""));
			GUI.Label( new Rect(280f, 25f, 77f, 50f), new GUIContent("Upgrades", null, ""));


			//Draw upgrade button logos 

			//Health upgrade pressed
			//Check to see if enough brainZ or gainZ aquired
			if(GUI.Button( new Rect(452,379, 50f, 50f), new GUIContent("", LevelupB1, "")))
			{
				//Call the code that upgrades the players maximum health
			}
			//Stamina upgrade pressed
			//Check to see if enough brainZ or gainZ aquired
			if(GUI.Button( new Rect(272f,379f,50f,50f), new GUIContent("", LevelupB2, "")))
			{
				//Call code that upgrades the players stamina duration or speed modifier
			}
			//Weapon upgrade pressed
			//Check to see if enough brainZ or gainZ aquired
			if(GUI.Button( new Rect(98f,379f,50f,50f), new GUIContent("", LevelupB3, "")))
			{
				//Call code that upgrades the players weapon damage modifier
			}
			//User is done applying upgrades
			if(GUI.Button( new Rect(589f,440f,45f,32f), new GUIContent("Done", null, "")))
			{
				showUpgradeMenu = false;
				//Call code that scene transitions and set apporpriate variables for next level
			}
		}

		else
		{
			//Hide background plane for upgrade menu
			upgradeBG.SetActive(false);
		}

		GUI.matrix = svMat;
	}
}