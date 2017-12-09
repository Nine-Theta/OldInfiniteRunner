using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    [SerializeField]
    private float jumpHeight = 5.0f;
    [SerializeField]
    private float movementSpeed = 3.0f;
    [SerializeField]
    private float hookVelocity = 3.0f;

    [SerializeField]
    private AudioClip _damageSound;
    [SerializeField]
    private AudioClip _deathSound;
    [SerializeField]
    private AudioClip _jumpSound;
    [SerializeField]
    private AudioClip _pickupSound;

    private static GameObject _singletonInstance;

    private Rigidbody2D _rigidbody;
    private LifelineScript _lifeline;
    private LineRenderer _hookLine; //And sinker
    private GameObject _lastHook;
    private Vector3 _hookPos;
    private HealthBarScript _healthBar;
    private Animator _animator;
    private AudioSource _audio;

    private int _jumps = 2;
    private float _gravityScale = 1.0f;
    private float _redTimer = 1.0f;
    private float _redTimerValue = 1.0f;

    private bool _grounded = false;
    private bool _hooked = false;
    private bool _safe = false;
    private bool _FacingRight = true;
    private bool _gotHit = false;

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
        //_lifeline = gameObject.GetComponentInChildren<LifelineScript>();
        _hookLine = GetComponentInChildren<LineRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_gotHit)
        {
            if (_redTimer <= 0.0f)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                _gotHit = false;
                _redTimer = _redTimerValue;
            }
            else
                _redTimer -= Time.deltaTime;
        }

        if (_healthBar.isAlive)
            MovementUpdate();
        //else if (Input.GetKeyDown(KeyCode.R))
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Flip();
    }


    private void MovementUpdate()
    {
        if (!_hooked)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
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
                _FacingRight = true;
            }
            if (!_grounded && Input.GetKey(KeyCode.A) && _rigidbody.velocity.x > -movementSpeed)
            {
                _rigidbody.AddForce(Vector2.left);
                _animator.SetBool("Running", true);
                _FacingRight = false;
            }

            Vector2 vel = _rigidbody.velocity;
            if (Input.GetKey(KeyCode.A) && _rigidbody.velocity.x > -movementSpeed && _grounded)
            {
                vel.x -= 1;
                _animator.SetBool("Running", true);
                if (vel.x < -movementSpeed)
                    vel.x = -movementSpeed;
                _FacingRight = false;
            }
            if (Input.GetKey(KeyCode.D) && _rigidbody.velocity.x < movementSpeed && _grounded)
            {
                vel.x += 1;
                _animator.SetBool("Running", true);
                if (vel.x > movementSpeed)
                    vel.x = movementSpeed;
                _FacingRight = true;
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
            _hookLine.SetPosition(1, _hookPos - transform.position);
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
        if (_audio != null)
            _audio.PlayOneShot(_jumpSound);
    }

    public void Hook(Vector2 direction, GameObject hook)
    {
        if (!_healthBar.isAlive)
            return;
        _hooked = true;
        _rigidbody.velocity = new Vector2(0.0f, 0.0f);
        _rigidbody.AddForce(direction.normalized * hookVelocity);
        _rigidbody.gravityScale = 0;
        _lastHook = hook;
        _hookPos = hook.transform.position;
        _hookLine.SetPosition(1, hook.transform.position - transform.position);
        if (!_FacingRight)
        {
            Vector3 flippedPos = _hookLine.GetPosition(1);
            flippedPos.x *= -1;
            _hookLine.SetPosition(1, flippedPos);
        }
        if (_lifeline != null)
        {
            _lifeline.isUnhooked = false;
            _lifeline.AddPoint(hook.transform.position);
        }
        _animator.SetBool("Hooking", true);
    }

    private void UnHook(bool resetVelocity = true)
    {
        if (!_healthBar.isAlive)
            return;
        _hooked = false;
        _rigidbody.gravityScale = _gravityScale;
        if (resetVelocity)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0.0f);
        Destroy(_lastHook);
        _hookLine.SetPosition(1, Vector3.zero);
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
            if (!_healthBar.TakeDamage())
                _audio.PlayOneShot(_deathSound);
            else
            {
                _audio.PlayOneShot(_damageSound);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                _gotHit = true;
            }
        }
        if (other.tag == "FogField")
        {
            if (other.GetComponent<FogFieldScript>().EnterFog(_healthBar))
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (other.tag == "BackgroundChanger")
        {
            other.GetComponent<BackgroundChangeScript>().Activate();
            _audio.PlayOneShot(_pickupSound);
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
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
            if (!_healthBar.TakeDamage())
            {
                _audio.PlayOneShot(_deathSound);
            }
            else
            {
                _audio.PlayOneShot(_damageSound);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                _gotHit = true;
            }
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
    private void Flip()
    {
        if (!_FacingRight)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            if (_hooked)
            {
                Vector3 flippedPos = _hookLine.GetPosition(1);
                flippedPos.x *= -1;
                _hookLine.SetPosition(1, flippedPos);
            }
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void PlayDeathSound()
    {
        _audio.PlayOneShot(_deathSound);
    }
}
