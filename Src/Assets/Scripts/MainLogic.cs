using UnityEngine;
using System.Collections;
using System.IO;

public class MainLogic : MonoBehaviour{

    private float    _time;
    private int      _tileNumRuns;
    public  GUIText  _timerText;
    public  GUIText  _scoreText;
    public  GUIText  _resualText;
    public  GUIText  _powerText;
    public  GUIText  _tipText;
    

    private  int _scorePerTile   = 10;
    private  int _tilesScore      = 0;

    private  bool    _gameOver = false;
    private  TilesManager _tilesManager;
    private  TileGenarator _tileGenarator;
    private  AudioSource  _boostAudio;
    private  int          _topScore;   
  
    private  bool         _newRecord;
    
    enum BoostState
    {
        BoostOff,
        BoostOn
    }

    private   BoostState      _boostState;
    private  float            _power;
    private  float            _boostTime;

    
    
    // Use this for initialization
	void Start () {
        _timerText.font.material.color = Color.black;
        _scoreText.font.material.color = Color.black;
        _resualText.font.material.color = Color.black;
        _resualText.active = false;
        _tileNumRuns = 0;
        _tilesManager = GameObject.Find("TilesManager").GetComponent<TilesManager>();
        _tileGenarator = GameObject.Find("TileGenerator").GetComponent<TileGenarator>();
        _power = 99;
        _boostTime = 0.0f;
        AudioSource _boostAufdio = GameObject.Find("BoostBGM").GetComponent<AudioSource>();
        
        
        // read leader board;
        string leadboardFile = Application.dataPath + "/" + "leadboards.txt";
        
        string[] filecontent = File.ReadAllLines(leadboardFile);
        _topScore = int.Parse(filecontent[0]);
        _newRecord = false;
    }
	
    public void   BoostUp()
    {
        if(_boostState == BoostState.BoostOn)
        {
            return;
        }
        if(_power<100.0f)
        {
            return;
        }

        _boostState = BoostState.BoostOn;
        OnBoostOn();
        _boostTime  = 6.0f;

    }

    private void  OnBoostOn()
    {
        AudioSource click = GameObject.Find("BoostBGM").GetComponent<AudioSource>();
        click.Play();
        rigidbody.useGravity = false;
    }

    private void OnBoostOff()
    {
        AudioSource click = GameObject.Find("BoostBGM").GetComponent<AudioSource>();
        click.Stop();
        rigidbody.useGravity = true;
    }

	// Update is called once per frame
	void Update () 
    {
        //Debug.Log("Velocity: " + GameConfig.Velocity);
        TestGameOver();
        if (_gameOver != true)
        {
            _time += Time.deltaTime;
            _tileNumRuns = _tileGenarator.GetTileGenNum();
            Debug.Log("Tile Num: "+ _tileNumRuns);
            //if (_tileNumRuns % 300 == 299)
            //{
            //    GameConfig.Velocity+=1.0f;
            //}
            ShowTime();
            ShowScore();  
            ShowBoost();
        }

        if(_boostState == BoostState.BoostOff)
        {
            _power += Time.deltaTime*2;
            if(_power >100.0f)
            {
                _power = 100.0f;
            }
        }

        if(_boostState == BoostState.BoostOn)
        {
            AudioSource click = GameObject.Find("BoostBGM").GetComponent<AudioSource>();
            click.volume -= (click.volume/6.0f) * Time.deltaTime;
            _power -= Time.deltaTime*(100.0f/6.0f);
            _boostTime -= Time.deltaTime;
            if(_power<0)
            {
                _power = 0;
            }
            if(_boostTime<=0.0f)
            {
                _boostState = BoostState.BoostOff;
                OnBoostOff();
            }
        }

        


        // to stop falling
        if( transform.position.y < -100)
        {
            rigidbody.useGravity = false;
            rigidbody.Sleep();
        }

        if(transform.position.y < -30)
        {
            AudioSource bgmover = GameObject.Find("BGMOver").GetComponent<AudioSource>();
            if(!bgmover.isPlaying)
            {
                bgmover.Play();                
            }

        }
	}

    void Restart()
    {
        Application.LoadLevel("Stage");
    }

    void OnGUI()
    {
        if(_gameOver)
        {
            Vector2 center = new Vector2(Screen.width/2, Screen.height/2);
            int offsetX = (int)(Screen.width * 0.1);
            int offsetY = (int)(Screen.height * 0.05);
            if(GUI.Button(new Rect(center.x - offsetX,
                                   center.y - offsetY + (int)(Screen.height * 0.2), 
                                   2*offsetX, 
                                   2*offsetY),
                                   "RETRY"))
            {
                Restart();

            }
            if (GUI.Button(new Rect(center.x - offsetX,
                                   center.y - offsetY + (int)(Screen.height * 0.35),
                                   2 * offsetX,
                                   2 * offsetY),
                                   "QUIT"))
            {
                // edior does not work;
                Application.Quit();
            }
        }
    }

    private void TestGameOver()
    {
        if(_boostState == BoostState.BoostOn)
        {
            return;
        }

        if( transform.position.y < 0.6f)
        {
            if(_gameOver == false)
            {
                OnGameOver();
            }
            _gameOver = true;
        }

        if( transform.position.x > 3.0f || transform.position.x < -3.0f)
        {
       
            if(_gameOver == false)
            {
                OnGameOver();
            }
            _gameOver = true;
        }
    }

    private  void OnGameOver()
    {
        HideScore();
        HideTime();
        if (TileScore > _topScore)
        {
            string[] wirteLines = new string[10];
            _newRecord = true;
            string leadboardFile = Application.dataPath + "/" + "leadboards.txt";
            wirteLines[0] = TileScore.ToString();
            File.WriteAllLines(leadboardFile, wirteLines);
            _topScore = TileScore;

        }
        _resualText.active = true;
        _powerText.active = false;

        string resualt = string.Format("GAME OVER\nTOP SCORE:{2}\nSCORE:{0}\n{1}\n", Score.ToString(), GetTimeStr(), _topScore.ToString());
        _resualText.text = resualt;

        collider.enabled = false;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        // change BGM
        AudioSource bgm  = GameObject.Find("BGMInGame").GetComponent<AudioSource>();
        bgm.Stop();



        AudioSource bgmrun = GameObject.Find("runingSound").GetComponent<AudioSource>();
        bgmrun.Stop();


    }

    private void ShowTime()
    {
        _timerText.text = GetTimeStr();
    }

    private string GetTimeStr()
    {
        int minute = (int)_time / 60;
        int second = (int)(_time - minute * 60);
        int millisecond = (int)((_time - second - minute * 60) * 100);

        string minuteStr = minute < 10 ? "0" + minute.ToString() : minute.ToString();
        string secondStr = second < 10 ? "0" + second.ToString() : second.ToString();
        string millisecondStr = millisecond < 10 ? "0" + millisecond.ToString() : millisecond.ToString();
        string timeStr = string.Format("TIME: {0}:{1}:{2}",
                                       minuteStr,
                                       secondStr,
                                       millisecondStr);
        return timeStr;
    }

    private void HideTime()
    {
        _timerText.active = false;
    }

    private void ShowScore()
    {
        _scoreText.text = string.Format("SCORE:{0}\nTOP SCORE:{1}\nSPEED:{2}", Score.ToString(), _topScore.ToString(),GameConfig.Velocity.ToString());
        _tipText.text = string.Format("press LMB to Jump\npress RMB to Float");
    }

    private void ShowBoost()
    {
        _powerText.text = string.Format("FLOAT:{0}", ((int)(_power)).ToString());
    }
    


    private void HideScore()
    {
        _scoreText.active = false;
    }

    public  int Score
    {
        get
        {
            return TileScore;
        }
       
    }
    public int TileScore
    {
        get
        {
            return (int)(_tileNumRuns* _scorePerTile);

        }

    }

}
