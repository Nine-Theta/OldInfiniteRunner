using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    public float projectileSpeed = 5.0f;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, 3);
        if(transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
        {
            Destroy(gameObject);
        }
    }

    public void SetForce(Vector2 direction)
    {
        _rigidbody.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
    }
}
