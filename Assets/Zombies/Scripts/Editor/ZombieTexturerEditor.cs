
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZombieTexturer))]

public class ZombieTexturerEditor : Editor
{

	public ZombieTexturer m_Component = null;

	public virtual void OnEnable()
	{
		m_Component = (ZombieTexturer)target;
	}

	public override void OnInspectorGUI()
	{

		EditorGUILayout.HelpBox("Pick a texture slot between 0 and 14 to be enabled at runtime (-1 means a random slot).", MessageType.Info);

		int change = m_Component.TextureSlot;
		m_Component.TextureSlot = EditorGUILayout.IntSlider("Texture", m_Component.TextureSlot, (Application.isPlaying ? 0 : -1), 14);
		if (Application.isPlaying && (change != m_Component.TextureSlot) && change != -1)
			m_Component.Regenerate();

		EditorGUILayout.HelpBox("Choose how many previous injuries this zombie has suffered. Warning: values above 10 may slow down loading!", MessageType.Info);
		m_Component.DamageAmount = EditorGUILayout.IntSlider("Injuries", m_Component.DamageAmount, 0, 200);
		if (Application.isPlaying)
		{
			if (GUILayout.Button("Regenerate Injuries"))
			{
				m_Component.Regenerate();
				m_Component.RandomDamage();
			}
		}



	}


}

