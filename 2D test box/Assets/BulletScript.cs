using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public float projectileSpeed = 5.0f;
    private Vector3 _velocity;
    private Rigidbody2D _rigidbody;

	private void Awake () {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

    private void Update () {
	}

    public void SetVelocity(Vector3 pVel)
    {
        _velocity = pVel;
        _rigidbody.velocity = _velocity * projectileSpeed;
        transform.rotation.SetLookRotation(_velocity);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.gameObject.tag);
        Destroy(gameObject);
    }
}
