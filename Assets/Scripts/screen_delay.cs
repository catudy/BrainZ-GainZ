using UnityEngine;
using System.Collections;

public class screen_delay : MonoBehaviour 
{
	//Public variables
	public int sceneTransTime = 1;
	public GUITexture guiTexture;

	IEnumerator Start()
	{
		//Wait sceneTransTime before loading next scene
		yield return new WaitForSeconds(sceneTransTime);

		//Load next scene
		Application.LoadLevel("Sushil_Test");
	}

}
