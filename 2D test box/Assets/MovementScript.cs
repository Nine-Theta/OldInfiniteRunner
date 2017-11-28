using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool _safe = false;
    private HealthBarScript healthBar;
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
        healthBar = gameObject.GetComponentInChildren<HealthBarScript>();
    }

    private void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        if (!_hooked)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && _jumps > 0)
            {
                //other substraction is in OnCollisionExit
                if (_jumps == 1)
                    _jumps--;
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, 0);
                _rigidbody.AddForce(Vector2.up * jumpHeight);
            }
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    _rigidbody.AddForce(Vector2.down * jumpHeight * 2);
            //}
            //Horizontal
            if (!_grounded && Input.GetKey(KeyCode.D) && _rigidbody.velocity.x < movementSpeed)
            {
                _rigidbody.AddForce(Vector2.right);
            }
            if (!_grounded && Input.GetKey(KeyCode.A) && _rigidbody.velocity.x > -movementSpeed)
            {
                _rigidbody.AddForce(Vector2.left);
            }

            Vector2 vel = _rigidbody.velocity;
            if (Input.GetKey(KeyCode.A) && _rigidbody.velocity.x > -movementSpeed && _grounded)
            {
                vel.x -= 1;
                if (vel.x < -movementSpeed)
                    vel.x = -movementSpeed;
            }
            if (Input.GetKey(KeyCode.D) && _rigidbody.velocity.x < movementSpeed && _grounded)
            {
                vel.x += 1;
                if (vel.x > movementSpeed)
                    vel.x = movementSpeed;
            }

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                if (_grounded)
                    vel.x = 0.0f;
                else
                    vel.x *= 0.8f;
            }
            _rigidbody.velocity = vel;
        }
    }

    public void Hook(Vector2 direction, GameObject hook)
    {
        _hooked = true;
        _rigidbody.velocity = new Vector2(0.0f, 0.0f);
        _rigidbody.AddForce(direction.normalized * hookVelocity);
        _rigidbody.gravityScale = 0;
        if (_lifeline != null)
        {
            _lifeline.doLineUpdate = false;
            _lifeline.AddPoint(hook.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hook")
        {
            _hooked = false;
            _rigidbody.gravityScale = _gravityScale;
            _rigidbody.velocity = new Vector2(0.0f, 0.0f);
            if (_lifeline != null)
            {
                _lifeline.AddPoint(transform.position);
                _lifeline.doLineUpdate = true;
            }
            Destroy(other.gameObject);
        }
        if(other.tag == "SafeZone")
        {
            _safe = true;
        }
        if(other.tag == "FogField")
        {
            other.GetComponent<FogFieldScript>().EnterFog(healthBar);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "SafeZone")
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
        if (other.tag == "AllSeeingEye" && !_safe)
        {
            if (other.GetComponent<SeeingEyeScript>() != null)
                other.GetComponent<SeeingEyeScript>().DetectPlayer(transform);
            else
                other.GetComponentInParent<SeeingEyeScript>().DetectPlayer(transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
        {
            _jumps = 2;
            _grounded = true;
        }

        if (coll.collider.tag == "Enemy")
        {
            healthBar.TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
        {
            _jumps--;
            _grounded = false;
        }
    }

    public void SetLifeLine(LifelineScript life)
    {
        _lifeline = life;
    }

    public static GameObject GetPlayer()
    {
        return _singletonInstance;
    }
}
