using UnityEngine;
using System.Collections;

// this is a generic object viewing component. add it to any gameobject
// in a scene and make sure to drag a target object into its "Target" slot
// (or set it from an external script)

public class Viewer : MonoBehaviour
{

	public GameObject m_Target = null;

	Vector3 m_TargetPos = Vector3.zero;
    float m_Yaw, m_Pitch;
    float m_Pan = 0.0f;
    float m_Zoom = -2.0f;


	void Start()
    {

        UpdateCameraAngle();
        UpdateCamera();

	}
    
    
	void Update()
	{

        if (Input.GetMouseButton(0))
        {
            UpdateCameraAngle();
			UpdateCamera();
        }

        if (!Input.GetMouseButton(2) && Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            m_Zoom += Input.GetAxis("Mouse ScrollWheel");
        }

        if (Input.GetMouseButton(2))
        {
            m_Pan -= Input.GetAxis("Mouse Y") * 0.05f;
			UpdateCamera();
        }

		UpdateCameraSmooth();

    }


    void UpdateCameraAngle()
    {

        m_Yaw += Input.GetAxis("Mouse X") * 3.0f;
        m_Pitch -= Input.GetAxis("Mouse Y") * 3.0f;
        Quaternion xQuaternion = Quaternion.AngleAxis(m_Yaw, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis((-m_Pitch), Vector3.left);
        Camera.main.transform.localRotation = (xQuaternion * yQuaternion);

    }


	public void UpdateCamera()
	{
		if (m_Target == null)
			return;
		Camera.main.transform.position = m_Target.transform.root.GetComponentInChildren<Collider>().bounds.center + (Vector3.up * m_Pan);
		Camera.main.transform.position += Camera.main.transform.forward * m_Zoom;
		m_TargetPos = Camera.main.transform.position;

	}


    void UpdateCameraSmooth()
    {
		if (m_Target == null)
			return;

		m_TargetPos = m_Target.transform.root.GetComponentInChildren<Collider>().bounds.center + (Vector3.up * m_Pan);
        m_TargetPos += Camera.main.transform.forward * m_Zoom;

		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_TargetPos, Time.smoothDeltaTime * 5.0f);

    }
        

}
