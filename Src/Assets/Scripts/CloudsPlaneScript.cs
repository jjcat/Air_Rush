using UnityEngine;
using System.Collections;

public class CloudsPlaneScript : MonoBehaviour {

	public Camera   m_cameraToLookAt;
	private Vector3 m_randomPos;
	// Use this for initialization
	void Start () {
	    m_cameraToLookAt=(Camera)GameObject.Find("Main Camera").camera;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(m_cameraToLookAt.transform.position); 
		transform.Rotate(new Vector3(90,180,0));
	}
	
	public void SetRandomPos(Vector3 p)
	{
		m_randomPos = p;
	}
	
	public Vector3 GetPosition()
	{
		return m_randomPos;
	}


    
}
