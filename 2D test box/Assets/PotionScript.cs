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
                Physics2D.IgnoreCollision(go.GetComponent<Collider2D>(), MovementScript.GetPlayer().GetComponent<Collider2D>());
                Destroy(gameObject);
                break;
            case "Enemy":
                if (gameObject.tag == "Potion")
                {
                    GameObject go1 = Instantiate(explosion, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    Physics2D.IgnoreCollision(go1.GetComponent<Collider2D>(), MovementScript.GetPlayer().GetComponent<Collider2D>());
                    Destroy(gameObject);
                }
                break;
            case "Explosion":
                Physics2D.IgnoreCollision(coll.collider, gameObject.GetComponent<Collider2D>());
                break;
            case "Potion":
                Physics2D.IgnoreCollision(coll.collider, gameObject.GetComponent<Collider2D>());
                break;
            case "Player":
                if(gameObject.tag == "EnemyPotion")
                {
                    GameObject go2 = Instantiate(explosion, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    Physics2D.IgnoreCollision(go2.GetComponent<Collider2D>(), MovementScript.GetPlayer().GetComponent<Collider2D>());
                    Destroy(gameObject);
                }
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
