using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this script does two things:

//		1: it randomizes between different zombie textures on startup by
//			offseting the mesh uv coordinates to a specific or random slot
//			of a 4x4 grid of the main texture

//		2: it simulates skin damage on the zombie by blending the current
//			slot texture with the last slot texture according to the vertex
//			alpha of the mesh, which can be manipulated at runtime by this
//			or an external script (such as "ZombieShooter.cs")

// IMPORTANT:
//		1: this script requires that the mesh renderer uses the
//		"Normal-BumpSpec-Zombie" shader
//		2: if you wish to edit this script, you likely want to remove its
//		custom editor script (present in the "Editor" folder), or any new
//		public variables you add will remain hidden in the editor


public class ZombieTexturer : MonoBehaviour
{

	public int TextureSlot = 0;		// texture slot used for this zombie. range: 0-14. a value of -1 means "pick randomly"
	public int DamageAmount = 0;	// the amount of injuries (bullet wounds) sustained by the zombie on startup
	private float m_GridRes = 4;	// amount of slots in the grid. changing this is not recommended

	// mesh data vars
	private Vector2[] m_UVOffsets = null;
	private Vector2[] m_OriginalUVs;
	private Vector2[] m_UVs;
	private Mesh m_Mesh = null;
	private Vector3[] m_Vertices = null;		


	public void Start()
    {
		
		m_UVOffsets = new Vector2[(int)(m_GridRes * m_GridRes)];

        int v = 0;
        for (float y = 0; y < m_GridRes; y++)
        {
            for (float x = 0; x < m_GridRes; x++)
            {
                float ux = (x / m_GridRes);
                float uy = (y / m_GridRes);

                m_UVOffsets[v] = new Vector2(ux, uy);
                v++;
            }

        }

        SkinnedMeshRenderer skin = GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

        skin.sharedMesh = (Mesh)Instantiate(skin.sharedMesh);
        m_Mesh = skin.sharedMesh;

		m_Vertices = m_Mesh.vertices;
        m_UVs = new Vector2[m_Mesh.uv.Length];
        m_OriginalUVs = new Vector2[m_Mesh.uv.Length];

        for (int vv = 0; vv < m_Mesh.uv.Length; vv++)
        {
            m_OriginalUVs[vv] = m_Mesh.uv[vv];
            m_UVs[vv] = m_Mesh.uv[vv];
        }

     	Regenerate();

		if (DamageAmount > 0)
			RandomDamage();

	}
    
    
	public void Regenerate()
	{

		if (TextureSlot == -1)
			TextureSlot = Random.Range(0, 15);
		int tile = Mathf.Clamp(TextureSlot, 0, 15);
        tile = (tile == 7) ? 15 : tile;

        Color[] cols = new Color[m_Mesh.colors.Length];

        for (int v = 0; v < m_OriginalUVs.Length; v++)
        {
            m_UVs[v] = m_OriginalUVs[v] + m_UVOffsets[tile];
            cols[v] = m_Mesh.colors[v];

            cols[v].r =  1 - ((tile % m_GridRes) / m_GridRes);
            cols[v].g =  1 - (((int)(tile / m_GridRes)) / m_GridRes);
        }
        m_Mesh.colors = cols;
        m_Mesh.uv = m_UVs;
		Refresh();

	}


    public void Refresh()
    {

        Color[] cols = new Color[m_Mesh.colors.Length];

        for (int v = 0; v < m_UVs.Length; v++)
        {
            cols[v] = m_Mesh.colors[v];
            cols[v].a = 0.0f;
        }

        m_Mesh.colors = cols;

    }


	public void RandomDamage(object[] o)
	{
		RandomDamage((float)o[0], (int)o[1]);
	}


	public void RandomDamage(float caliber = 0.1f, int damageAmount = 0)
	{

		if (damageAmount > 0)
			DamageAmount = damageAmount;

		int wounds = Random.Range(1, DamageAmount);
        for (int v = 0; v < wounds; v++ )
        {
            Vector3 pos = Vector3.zero;
            pos.y = Random.Range(0, 2f);
            pos = transform.TransformPoint(pos);
            Vector3 dir = Random.rotation.eulerAngles;
            PositionDamage(dir, pos, caliber);

        }

	}


	public void PositionDamage(object[] dirPos)
	{
		PositionDamage((Vector3)dirPos[0], (Vector3)dirPos[1], (float)dirPos[2]);
	}


	public void PositionDamage(Vector3 dir, Vector3 pos, float caliber = 0.1f)
	{

        Vector3[] verts = GetBakedVerts();

        if(!ApplyDamage(dir, pos, verts, caliber))
            ApplyDamage(dir, FindClosestVertex(pos, verts), verts, caliber);

	}


    Vector3 FindClosestVertex(Vector3 pos, Vector3[] verts)
    {

        Vector3 newpos = pos;
        float shortestDist = 1000000;
        float dist;

        for (int v = 0; v < verts.Length; v++)
        {
            dist = Vector3.Distance(pos, transform.TransformPoint(verts[v]));
            if (dist < shortestDist)
            {
                shortestDist = dist;
                newpos = transform.TransformPoint(verts[v]);
            }
        }

        return newpos;

    }


    bool ApplyDamage(Vector3 dir, Vector3 pos, Vector3[] verts, float caliber)
    {

        bool hit = false;
        Ray ray = new Ray(pos, dir);
        for (int v = 0; v < m_Vertices.Length; v++)
        {

			if (DistanceToLine(ray, transform.TransformPoint(verts[v])) < caliber)
            {

                hit = true;
               
                Color[] colors = new Color[m_Vertices.Length];
                colors = m_Mesh.colors;
                colors[v].a = 1;
                m_Mesh.colors = colors;
                m_Mesh.vertices = m_Vertices;

            }

        }

		return hit;

    }


    Vector3[] GetBakedVerts()
    {
        SkinnedMeshRenderer skin = this.GetComponentInChildren<SkinnedMeshRenderer>();
        Mesh baked = new Mesh();
        skin.BakeMesh(baked);
        return baked.vertices;
    }


    public static float DistanceToLine(Ray ray, Vector3 point)
    {
        return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    }
	

}
