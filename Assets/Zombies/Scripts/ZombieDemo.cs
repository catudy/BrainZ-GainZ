using UnityEngine;
using System.Collections;

// a special demo script hard-coded for the zombie package demo

public class ZombieDemo : MonoBehaviour
{


	GameObject[] m_Zombies;
	int m_CurrentZombie = 0;
	Viewer m_Viewer = null;
	ZombieShooter m_Shooter = null;


	void Start()
	{

		m_Zombies = GameObject.FindGameObjectsWithTag("Zombie");

		m_Shooter = (ZombieShooter)GameObject.FindObjectOfType(typeof(ZombieShooter));

		RefreshZombie();

	}


	void PrevModel()
	{

		m_CurrentZombie--;
		if (m_CurrentZombie < 0)
			m_CurrentZombie = m_Zombies.Length - 1;

		RefreshZombie();

	}


	void NextModel()
	{

		m_CurrentZombie++;
		if (m_CurrentZombie > m_Zombies.Length - 1)
			m_CurrentZombie = 0;

		RefreshZombie();

	}


	void PrevTexture()
	{

		ZombieTexturer z = m_Zombies[m_CurrentZombie].GetComponent<ZombieTexturer>();
		z.TextureSlot--;

		if (z.TextureSlot < 0)
			z.TextureSlot = 14;
		z.Regenerate();

		RefreshZombie();
	
	}


	void NextTexture()
	{

		ZombieTexturer z = m_Zombies[m_CurrentZombie].GetComponent<ZombieTexturer>();
		z.TextureSlot++;

		if (z.TextureSlot > 14)
			z.TextureSlot = 0;
		z.Regenerate();
		RefreshZombie();

	}


	void RefreshZombie()
	{

		m_Zombies[m_CurrentZombie].SetActive(true);
		m_Viewer = (Viewer)GameObject.FindObjectOfType(typeof(Viewer));
		m_Viewer.m_Target = m_Zombies[m_CurrentZombie];

	}


	void OnGUI()
	{

		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		GUILayout.BeginVertical();
		GUILayout.Space(20);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<"))
		{
			PrevModel();
		}
		GUILayout.Label("Zombie");
		if (GUILayout.Button(">"))
		{
			NextModel();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<"))
			PrevTexture();
		GUILayout.Label("Texture");
		if (GUILayout.Button(">"))
			NextTexture();
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Add random injuries"))
			m_Zombies[m_CurrentZombie].SendMessage("RandomDamage", new object[2] {m_Shooter.Caliber, 10});
		if (GUILayout.Button("Clean up"))
			m_Zombies[m_CurrentZombie].SendMessage("Refresh");
		GUILayout.EndVertical();
		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		GUILayout.BeginVertical();
		GUILayout.Space(20);
		GUILayout.Label("Left Mouse Button = Rotate");
		GUILayout.Label("Right Mouse Button = Shoot");
		GUILayout.Label("Middle Mouse Button = Pan up / down");
		GUILayout.Label("Mouse Wheel = Zoom in / out");
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		
	}

}
