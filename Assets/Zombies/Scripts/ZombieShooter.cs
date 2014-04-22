using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this script gives a gameobject the ability to injure a zombie.
// it is typically dragged onto the main camera of the scene, but it
// can also be used by a weapon gameobject.

// by default, this script waits for a left mouse click and fires a
// ray that will injure the first zombie in its path. if the component
// is in "Forward" mode, it will raycast in the parent gameobject's
// forward direction. if the script is in "MousePos" mode, it will fire the
// ray from the middle of the screen to the mouse cursor

// IMPORTANT: in order for a zombie to be damaged, it must have the following:
//   1. it must be tagged "Zombie"
//   2. it must have the "Normal-BumpSpec-Zombie" shader
//   3. it must have a "ZombieTexturer" component

public class ZombieShooter : MonoBehaviour
{

	public Mode m_Mode = Mode.Forward;
	public float Caliber = 0.1f;

	public enum Mode
	{
		Forward,
		MousePos
	}
	

	void Update()
	{

		if (Input.GetMouseButtonUp(1))
		{

			Ray ray;
			if (m_Mode == Mode.Forward)
				ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			else
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1000))
			{
				if (hit.collider != null)
				{
					if (hit.collider.gameObject.tag == "Zombie")
						hit.transform.gameObject.SendMessage("PositionDamage", new object[3] { Camera.main.transform.forward, hit.point, Caliber }, SendMessageOptions.DontRequireReceiver);
				}
			}

		}


	}

}
