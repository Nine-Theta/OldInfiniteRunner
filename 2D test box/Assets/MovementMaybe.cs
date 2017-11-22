using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMaybe : MonoBehaviour
{
    public float jumpHeight = 5.0f;
    public float movementSpeed = 3.0f;
    public float hookVelocity = 3.0f;

    private int _jumps = 2;
    private bool _hooked = false;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
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
            if (Input.GetKeyDown(KeyCode.A)) _rigidbody.AddForce(Vector2.left * movementSpeed);
            if (Input.GetKeyDown(KeyCode.S))
            {
                _rigidbody.AddForce(Vector2.down * jumpHeight * 2);
            }
            if (Input.GetKeyDown(KeyCode.D)) _rigidbody.AddForce(Vector2.right * movementSpeed);

            Vector2 vel = _rigidbody.velocity;
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) vel.x = 0;
            _rigidbody.velocity = vel;
        }
    }

    public void Hook(Vector2 direction)
    {
        _hooked = true;
        _rigidbody.velocity = new Vector2(0.0f, 0.0f);
        _rigidbody.AddForce(direction.normalized * hookVelocity);
        _rigidbody.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hook")
        {
            _hooked = false;
            _rigidbody.gravityScale = 1.0f;
            _rigidbody.velocity = new Vector2(0.0f, 0.0f);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.collider.tag == "Terrain")
        {
            _jumps = 2;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
        {
            _jumps--;
        }
    }
}
