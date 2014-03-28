using UnityEngine;
using System.Collections;

public class Scene_Transition : MonoBehaviour 
{
	public GUITexture myTexture;
	public bool fadeOn = true;
	private float fadeInRate = 0.08f;
	private float fadeOutRate = 0.7f;
	public float alpha = 0.0f;
	private bool nextScene = false;
	private float maxFade = 0.32f;
	private float minFade = 0.005f;

	void Update()
	{
		if(fadeOn)
		{
			FadeIn();
		}
		else if(!fadeOn)
		{
			FadeOut();
		}

		if(nextScene)
		{
			Application.LoadLevel("Main_Menu");
		}


	}

	void FadeIn()
	{
		alpha = Mathf.Lerp (alpha, 1, fadeInRate * Time.deltaTime);
		myTexture.color = new Color(1,1,1,alpha);

		if(alpha >= maxFade)
		{
			fadeOn = false;
		}
	}

	void FadeOut()
	{
		alpha = Mathf.Lerp (alpha, 0, fadeOutRate * Time.deltaTime);
		myTexture.color = new Color(1,1,1,alpha);

		if(alpha <= minFade)
		{
			nextScene = true;
		}
	}
}


