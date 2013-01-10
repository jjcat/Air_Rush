using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour{

    public Texture2D  _titleTex;
    private bool      _startLevel;
    private float     _startLevelTimer = 3.0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.anyKeyDown && _startLevel != true)
        {
            AudioSource click = GameObject.Find("StartClick").GetComponent<AudioSource>();
            click.Play();
            AudioSource bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
            bgm.Stop();
        }
        if(Input.anyKeyDown)
        {
            _startLevel = true;
        }
        if (_startLevel)
        {
            _startLevelTimer -= Time.deltaTime;
        }
        if(_startLevelTimer <= 0.0f)
        {
            StartGame();
        }
        
	}

    public void StartGame()
    {
        Application.LoadLevel("Stage");
    }

    void OnGUI()
    {
        if (!_startLevel)
        {
            GUI.Button(new Rect(0, 0, Screen.width, Screen.height), _titleTex);            
        }
    }


}
