using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DebugDeawLine : MonoBehaviour{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    Debug.DrawLine(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 300.0f), Color.red);
	}
}
