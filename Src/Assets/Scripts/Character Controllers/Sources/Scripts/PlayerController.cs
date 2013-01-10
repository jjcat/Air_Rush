using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour{

    public  MouseLook  _mouseLook;
    public  float      _force;

    private KeyCode    _jumpKey = KeyCode.Mouse0;
    private KeyCode    _boostKey   = KeyCode.Mouse1;
    private State      _state;
    public  float      _coldTimeJump;
    private float      _jumpColdTimer;
    private int        _jumpTime = 0;

    private MainLogic _main;


    enum State
    {
        Run,
        Jump_Start,
        Jump_Up,
        Jump_Down
    }

    
    // Use this for initialization
	void Start () {
	    _state = State.Run;
        _jumpColdTimer = 0.0f;
        _jumpTime  = 0;
        _main = GameObject.Find("Player").GetComponent<MainLogic>();
	}

    private void ChangeState(State newState)
    {
        _state = newState;
    }

    void OnCollisionEnter(Collision other)
    {
        if( _state != State.Run)
        {
            ChangeState(State.Run);
        }
    }

    void  StartBoost()
    {
        
    }

	
	// Update is called once per frame
	void Update () {
	    float rotationX = _mouseLook.GetRotationX();
        float translationX = Mathf.Sin(Mathf.Deg2Rad * rotationX) * GameConfig.Velocity;
        transform.Translate(new Vector3(translationX * Time.fixedDeltaTime, 0.0f, 0.0f));


        // update Jump cold timer
        if(_jumpColdTimer > 0)
        {
            _jumpColdTimer -= Time.deltaTime;
        }

        // start boost
        if (Input.GetKeyDown(_boostKey))
        {
            _main.BoostUp();
//      
        }


        if(Input.GetKeyUp(_jumpKey))
        {
            switch(_state)
            {
                case State.Run:
                    break;
                case State.Jump_Start:
                    ChangeState(State.Run);                    
                    break;
                case State.Jump_Up:
                    ChangeState(State.Jump_Down);                                        
                    break;
                case State.Jump_Down:
                    ChangeState(State.Jump_Down);                                        
                    break;
            }
        }

        if(Input.GetKeyDown(_jumpKey))
        {
            switch(_state)
            {
                case State.Run:
                    {
                        AudioSource jumpsound = GameObject.Find("jumpingSound").GetComponent<AudioSource>();
                        jumpsound.Play();
                        ChangeState(State.Jump_Start);                                        
                    }
                    break;
                case State.Jump_Start:
                    break;
                case State.Jump_Up:
                    break;
                case State.Jump_Down:
                    break;
            }
        }

        if(Input.GetKey(_jumpKey))
        {
            switch (_state)
            {
                case State.Run:
                    {
                        AudioSource jumpsound = GameObject.Find("jumpingSound").GetComponent<AudioSource>();
                        jumpsound.Play();
                        ChangeState(State.Jump_Start);
                    }
                    break;
                case State.Jump_Start:
                    {
                        rigidbody.AddForce(new Vector3(0.0f, _force * 5.0f, 0.0f), ForceMode.Impulse);
                        ChangeState(State.Jump_Up);                        
                    }
                    break;
                case State.Jump_Up:
                    {
                        if (_jumpTime > 60)
                        {
                            ChangeState(State.Jump_Down);                        
                            _jumpTime = 0;
                        }
                        else
                        {
                            rigidbody.AddForce(new Vector3(0.0f, _force*0.2f, 0.0f), ForceMode.Impulse);
                            _jumpTime ++;
                        }
                    }
                    break;
                case State.Jump_Down:
                    break;
            }
        }      

    
    }



}
