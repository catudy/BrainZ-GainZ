using UnityEngine;
using System.Collections;

public class GameGui : MonoBehaviour 
{
	public int gamePauseW = 50;
	public int gmaePauseH = 50;
	private int placeGamePauseW;
	private int placeGamePauseH;

	// Use this for initialization
	void Start () 
	{
		placeGamePauseW = Screen.width/2;
		placeGamePauseH = Screen.height/2;
	}
	
	// Update is called once per frame
	void OnGui () 
	{
		if(GUI.Button(new Rect(gamePauseW,gmaePauseH,placeGamePauseW,placeGamePauseH), "PAUSE"))
		{
			Application.LoadLevel("Main_Menu");
		}
	}
}
