using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour
{
    public float projectileSpeed = 5.0f;
    public GameObject explosion;
    private Rigidbody2D _rigidbody;
    private GameObject _player;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, 3);
        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        switch (coll.collider.tag)
        {
            case "Terrain":
                GameObject go = Instantiate(explosion, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                Physics2D.IgnoreCollision(go.GetComponent<Collider2D>(), _player.GetComponent<Collider2D>());
                Destroy(gameObject);
                break;
            case "Explosion":
                Physics2D.IgnoreCollision(coll.collider, gameObject.GetComponent<Collider2D>());
                break;
            case "Potion":
                Physics2D.IgnoreCollision(coll.collider, gameObject.GetComponent<Collider2D>());
                break;
        }
    }

    public void SetForce(Vector2 pDirection)
    {
        _rigidbody.AddForce(pDirection * projectileSpeed, ForceMode2D.Impulse);
    }

    public void SetPlayer(GameObject pPlayer)
    {
        _player = pPlayer;
    }

}
