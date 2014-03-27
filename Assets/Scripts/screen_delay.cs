using UnityEngine;
using System.Collections;

public class screen_delay : MonoBehaviour 
{
	public GUITexture myTexture;
	private bool fadeOn = true;
	private float fadeInRate = 0.05f;
	private float fadeOutRate = 1f;
	private float alpha;
	private float maxFade = 0.2f;

	void Update()
	{
		if(fadeOn)
		{
			FadeIn();
		}
		else
		{
			FadeOut();
		}
	}

	void FadeIn()
	{/*
		while(alpha <= maxFade)
		{
			alpha = Mathf.Lerp (0,1,Time.deltaTime*fadeInRate);
			myTexture.color = new Color(1,1,1,alpha);
		}

		fadeOn = false;
		*/

	}

	void FadeOut()
	{
		myTexture.color = Color.Lerp(myTexture.color,Color.black, Time.deltaTime*fadeOutRate);
	}
}


