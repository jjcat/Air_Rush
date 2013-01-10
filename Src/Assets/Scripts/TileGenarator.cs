using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TileGenarator : MonoBehaviour{

    public  GameObject  _perfabTile;
    public  string      _mapFile;
    
    private static int  _tilesArrayLength = (int)GameConfig.Start_Distance + 10;
    private static int  _lineWidth      = GameConfig.Roadway_Width;
    private GameObject[][] _tileArray;
    private GameObject     _newestTile;
    private int            _lastIndex;
    
    
    
    private  string[]      _map;
    private  int           _mapLength;
    private  int           _mapIndex;





    private void ReadMapFile()
    {        
        Road road = GameObject.Find("Road").GetComponent<Road>();
        string filename = Application.dataPath + "/" + _mapFile;
        _map = road.CreateRoad();
        _mapLength = road._roadLength;
        _mapIndex = 0;
    }

    // Use this for initialization
	void Start () 
    {
        ReadMapFile();
        _newestTile = new GameObject();

        _tileArray = new GameObject[_tilesArrayLength][];
        for( int i = 0; i < _tilesArrayLength; i++)
        {
            _tileArray[i] = new GameObject[_lineWidth];
        }


        for (int i = 0; i < _tilesArrayLength; i++)
        {

            for (int j = 0; j < _lineWidth; j++)
            {
                _tileArray[i][j] = (GameObject)Instantiate(_perfabTile, new Vector3(-j + 2.5f, 0, i), Quaternion.identity);    
                _tileArray[i][j].name = i.ToString() + "_" + char.ConvertFromUtf32(65 + j); ;
                //DisableTile(i, j);
            }
        }
        _newestTile = _tileArray[_tilesArrayLength-1][0];
        _lastIndex = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Debug.Break();
        if (_newestTile.transform.position.z <= GameConfig.Start_Distance)
        {

            CreateNewLine(new Vector3(0, 0, _newestTile.transform.position.z + GameConfig.Tile_Size), _map[_mapIndex]);
            _mapIndex = ++_mapIndex % _mapLength;
        }

        	    
	}
    void OnPostRender()
    {
        //Debug.Break();
    }

    public int GetTileGenNum()
    {
        return _lastIndex;
    }

    public void CreateNewLine(Vector3 position, string slot)
    {
        int currentIndex = _lastIndex % _tilesArrayLength;
        ++_lastIndex;

        
        float offset  = GameConfig.Tile_Size;
        float offsetX = 2.5f * offset + position.x;
        for (int i = 0; i < _lineWidth; ++i)
        {
            _tileArray[currentIndex][i].transform.position = new Vector3(offsetX, position.y, position.z);
            offsetX -= offset;
            if( slot[i] == 'O')
            {
                DisableTile(currentIndex, i);
            }
            else if( slot[i] == '@')
            {
                EnableTile(currentIndex, i);
            }
            
        }
        _newestTile = _tileArray[currentIndex][0];
    }

    

    private  void  DisableTile(int i, int j)
    {
        _tileArray[i][j].renderer.enabled = false;
        _tileArray[i][j].collider.enabled = false;
    }

    private  void  EnableTile( int i, int j)
    {
        _tileArray[i][j].renderer.enabled = true;
        _tileArray[i][j].collider.enabled = true;
    }


    private void DestoryTile()
    {
        
    }





}
