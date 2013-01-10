using UnityEngine;
using System.Collections;

public class CloudsScript : MonoBehaviour {
	
	public static int CloudPlaneNum = 10;
    public  GameObject _cloudsPlane;
    public float PlaneSize = 10.0f;
    public GameObject[]  m_cloudPlanes;
    public bool   _static;

    public float _velocity = GameConfig.Velocity;

	
    // Use this for initialization
	void Start () 
    {
        m_cloudPlanes = new GameObject[CloudPlaneNum];
        for (int i = 0; i < CloudPlaneNum; i++)
		{
            Vector3 randomPos = new Vector3(Random.Range(-PlaneSize, PlaneSize), Random.Range(-PlaneSize, PlaneSize), Random.Range(-PlaneSize, PlaneSize));
			Vector3 position = transform.position + randomPos;
			m_cloudPlanes[i]=(GameObject)Instantiate(_cloudsPlane, position, Quaternion.identity);
		    m_cloudPlanes[i].GetComponent<CloudsPlaneScript>().SetRandomPos(randomPos);
		}
	}

    public void RandomCloud()
    {
        foreach(GameObject cloudPlane in m_cloudPlanes)
        {
            Vector3 randomPos = new Vector3(Random.Range(-PlaneSize, PlaneSize), Random.Range(-PlaneSize, PlaneSize), Random.Range(-PlaneSize, PlaneSize));
            cloudPlane.GetComponent<CloudsPlaneScript>().SetRandomPos(randomPos);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {

        foreach( GameObject cloudPlane in m_cloudPlanes)
        {
            CloudsPlaneScript cloudsPlanScript = cloudPlane.GetComponent<CloudsPlaneScript>();
            Vector3 pos = cloudsPlanScript.GetPosition();
            Vector3 position = transform.position + pos;
            cloudPlane.GetComponent<Transform>().position = position;
        }


	}

    void FixedUpdate()
    {
        if(_static)
        {
            return;
        }
        transform.Translate(0, 0, -_velocity * Time.fixedDeltaTime);

        if(transform.position.z <= -2.0 )
        {
            transform.position = new Vector3(Random.Range(-10,10), Random.Range(-10, 10), Random.Range(100, 200));
            RandomCloud();
        }
    }

    public void Destory()
    {
        foreach(GameObject obj in m_cloudPlanes)
        {
            GameObject.Destroy(obj);
        }
    }


}