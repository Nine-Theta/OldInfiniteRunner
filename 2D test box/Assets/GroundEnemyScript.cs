using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyScript : MonoBehaviour
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float patrolTime = 60.0f;
    public float attackCooldown = 3.0f;
    public GameObject straightProjectile;

    private float _attackCooldown;
    private Vector2 _startPosition;
    private bool _startToEnd = true;
    private Transform _player;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        Patrol();
        if (_player == null)
            DetectPlayer();
        else
        {
            _attackCooldown -= Time.deltaTime;
            if (_attackCooldown <= 0.0f)
            {
                Attack();
                _attackCooldown = attackCooldown;
            }
        }
    }

    private void Patrol()
    {
        if (_startToEnd)
        {
            transform.position += new Vector3(endPoint.x * (1.0f / patrolTime), 0.0f);
            Vector3 compare = _startPosition + endPoint;
            if (transform.position.x >= compare.x)
            {
                _startToEnd = false;
            }
        }
        else
        {
            transform.position -= new Vector3(endPoint.x * (1.0f / patrolTime), 0.0f);
            if (transform.position.x <= _startPosition.x)
            {
                _startToEnd = true;
            }
        }
    }

    private void DetectPlayer()
    {
        RaycastHit2D infoR = Physics2D.Raycast(transform.position, Vector2.right, 20.0f, 1);
        if (infoR.collider != null && infoR.collider.tag == "Player")
            _player = infoR.transform;
        RaycastHit2D infoL = Physics2D.Raycast(transform.position, Vector2.left, 20.0f, 1);
        if (infoL.collider != null && infoL.collider.tag == "Player")
            _player = infoR.transform;
    }

    private void Attack()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(straightProjectile, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        bullet.GetComponent<BulletScript>().SetVelocity(direction);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Explosion")
            Destroy(gameObject);
    }
}
