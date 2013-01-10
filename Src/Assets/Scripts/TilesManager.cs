using UnityEngine;
using System.Collections;

public class TilesManager : MonoBehaviour{

    private float _velocity      = GameConfig.Velocity;     
    private float _boxSide       = GameConfig.Tile_Size;  
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Tile"))
        {
            obj.transform.Translate(0, 0, -Velocity * Time.fixedDeltaTime);
        }
    }


    public void Boost()
    {
        _velocity = GameConfig.Max_Velocity;
    }

    public void Unboost()
    {
        _velocity = GameConfig.Velocity;
    }


    public float Velocity
    {
        get
        {
            return _velocity * _boxSide;
        }
    }
}
