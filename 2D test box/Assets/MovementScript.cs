using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    public float jumpHeight = 5.0f;
    public float movementSpeed = 3.0f;
    public float hookVelocity = 3.0f;

    private int _jumps = 2;
    private bool _grounded = false;
    private bool _hooked = false;
    private float _gravityScale = 1.0f;
    private Rigidbody2D _rigidbody;
    private LifelineScript _lifeline;
    //private LineRenderer _hookLine; //And sinker
    private bool _safe = false;
    private GameObject _lastHook;
    private HealthBarScript _healthBar;
    private Animator _animator;
    private static GameObject _singletonInstance;

    public bool isHooked { get { return _hooked; } }

    private void Awake()
    {
        if (_singletonInstance != null && _singletonInstance != this.gameObject)
            Destroy(gameObject);
        else
            _singletonInstance = this.gameObject;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _gravityScale = _rigidbody.gravityScale;
        _healthBar = gameObject.GetComponentInChildren<HealthBarScript>();
        _lifeline = gameObject.GetComponentInChildren<LifelineScript>();
        _animator = gameObject.GetComponent<Animator>();
        //_hookLine = gameObject.GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        //if (_healthBar.isAlive)
        MovementUpdate();
        //else if (Input.GetKeyDown(KeyCode.R))
        //    SceneManager.LoadScene(0);
        //FixHookLine();
        
    }

    //private void FixHookLine()
    //{
    //    //if (_hookLine != null)
    //    //{
    //    //    _hookLine.SetPosition(0, transform.position);
    //    //    if (!_hooked)
    //    //        _hookLine.SetPosition(1, transform.position);
    //    //}
    //}

    private void MovementUpdate()
    {
        if (!_hooked)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    _rigidbody.AddForce(Vector2.down * jumpHeight * 2);
            //}
            //Horizontal
            if (!_grounded && Input.GetKey(KeyCode.D) && _rigidbody.velocity.x < movementSpeed)
            {
                _rigidbody.AddForce(Vector2.right);
                _animator.SetBool("Running", true);
            }
            if (!_grounded && Input.GetKey(KeyCode.A) && _rigidbody.velocity.x > -movementSpeed)
            {
                _rigidbody.AddForce(Vector2.left);
                _animator.SetBool("Running", true);
            }

            Vector2 vel = _rigidbody.velocity;
            if (Input.GetKey(KeyCode.A) && _rigidbody.velocity.x > -movementSpeed && _grounded)
            {
                vel.x -= 1;
                _animator.SetBool("Running", true);
                if (vel.x < -movementSpeed)
                    vel.x = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.D) && _rigidbody.velocity.x < movementSpeed && _grounded)
            {
                vel.x += 1;
                _animator.SetBool("Running", true);
                if (vel.x > movementSpeed)
                    vel.x = movementSpeed;
            }

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                if (_grounded)
                    vel.x = 0.0f;
                else
                    vel.x *= 0.8f;
                _animator.SetBool("Running", false);
            }
            _rigidbody.velocity = vel;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                UnHook(false);
                Jump();
            }
        }
    }

    private void Jump()
    {
        if (_jumps <= 0)
            return;
        //other substraction is in OnCollisionExit
        if (_jumps == 1)
            _jumps--;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _rigidbody.AddForce(Vector2.up * jumpHeight);
        _animator.SetBool("Jumping", true);
    }

    public void Hook(Vector2 direction, GameObject hook)
    {
        _hooked = true;
        _rigidbody.velocity = new Vector2(0.0f, 0.0f);
        _rigidbody.AddForce(direction.normalized * hookVelocity);
        _rigidbody.gravityScale = 0;
        _lastHook = hook;
        if (_lifeline != null)
        {
            _lifeline.isUnhooked = false;
            _lifeline.AddPoint(hook.transform.position);
        }
        _animator.SetBool("Hooking", true);
    }

    private void UnHook(bool resetVelocity = true)
    {
        _hooked = false;
        _rigidbody.gravityScale = _gravityScale;
        if (resetVelocity)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0.0f);
        Destroy(_lastHook);
        if (_lifeline != null)
        {
            _lifeline.RemoveLastPoint();
            _lifeline.isUnhooked = true;
        }
        _animator.SetBool("Hooking", false);
    }

    #region OnTriggers

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hook")
        {
            UnHook(false);
        }
        if (other.tag == "SafeZone")
        {
            _safe = true;
        }
        if (other.tag == "EnemyExplosion")
        {
            _healthBar.TakeDamage();
        }
        if (other.tag == "FogField")
        {
            other.GetComponent<FogFieldScript>().EnterFog(_healthBar);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SafeZone")
        {
            _safe = false;
        }
        if (other.tag == "FogField")
        {
            other.GetComponent<FogFieldScript>().LeaveFog();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "SafeZone" && !_safe)
        {
            _safe = true;
        }
        if (other.tag == "AllSeeingEye" && !_safe)
        {
            if (other.GetComponent<SeeingEyeScript>() != null)
                other.GetComponent<SeeingEyeScript>().DetectPlayer(transform);
            else
                other.GetComponentInParent<SeeingEyeScript>().DetectPlayer(transform);
        }
    }

    #endregion

    #region OnCollisions
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
        {
            if (_hooked)
            {
                UnHook();
            }
        }
        if (coll.collider.tag == "Enemy")
        {
            _healthBar.TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
        {
            _jumps--;
            _grounded = false;
            _animator.SetBool("Falling", true);
        }
    }

    #endregion

    public static GameObject GetPlayer()
    {
        return _singletonInstance;
    }

    public void Ground()
    {
        _jumps = 2;
        _grounded = true;
        _animator.SetBool("Jumping", false);
        _animator.SetBool("Falling", false);
    }

    public void StepOnEnemy()
    {
        _jumps = 1;
        _grounded = true;
        _animator.SetBool("Jumping", false);
        _animator.SetBool("Falling", false);
    }

    public void StopThrowing()
    {
        _animator.SetBool("Throwing", false);
    }
    public void StopJumping()
    {
        _animator.SetBool("Jumping", false);
    }
    public void StopPulling()
    {
        _animator.SetBool("PulledBack", false);
    }
}
