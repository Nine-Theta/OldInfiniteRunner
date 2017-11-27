using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float projectileSpeed = 5.0f;
    public bool destroyOnContact = true;
    private Vector3 _velocity;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    }

    public void SetVelocity(Vector3 pVel)
    {
        _velocity = pVel;
        _rigidbody.velocity = _velocity * projectileSpeed;
        transform.rotation.SetLookRotation(_velocity);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Player")
            coll.collider.GetComponentInChildren<HealthBarScript>().TakeDamage();
        if (destroyOnContact)
            Destroy(gameObject);
    }
}
