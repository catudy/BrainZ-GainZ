using UnityEngine;
using System.Collections;

public class Scene_Transition : MonoBehaviour 
{
	//Texture variables
	public GUITexture myTexture;

	//Fade rate variables
	public float fadeInRate = 0.08f;
	public float fadeOutRate = 0.7f;
	public float maxFade = 0.4f;
	private float alpha = 0.0f;
	private float minFade = 0.04f;

	//Fade toggle variables
	private bool nextScene = false;
	private bool fadeOn = true;
	public bool fadeInOn = true;
	public bool fadeOutOn = true;
	public bool autoNextScene = true;

	void Update()
	{
		//If its time to fade in and you want to
		if(fadeOn && fadeInOn)
		{
			FadeIn();
		}
		//If its time to fade out and you want to
		else if(!fadeOn && fadeOutOn)
		{
			FadeOut();
		}

		//If you are done with fades and you want to transition
		//scenes automatically 
		//Must edit parameters in the future if we want to auto to a different screen
		if(nextScene && autoNextScene)
		{
			Application.LoadLevel("_MainMenu");
		}


	}

	void FadeIn()
	{
		//While we havent reached max fade in value keep fading in
		if(alpha < maxFade)
		{
			alpha = Mathf.Lerp (alpha, 1, fadeInRate * Time.deltaTime);
			myTexture.color = new Color(myTexture.color.r,myTexture.color.g,myTexture.color.b,alpha);
		}
		//Stop fading in
		else if(alpha >= maxFade)
		{
			fadeOn = false;
		}
	}

	void FadeOut()
	{
		//While we havent reached min fade out value keep fading out
		if(alpha > minFade)
		{
			alpha = Mathf.Lerp (alpha, 0, fadeOutRate * Time.deltaTime);
			myTexture.color = new Color(myTexture.color.r,myTexture.color.g,myTexture.color.b,alpha);
		}

		else if(alpha <= minFade)
		{
			nextScene = true;
		}
	}	


}


