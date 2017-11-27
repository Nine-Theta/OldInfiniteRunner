using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour
{
    public GameObject item;
    public Transform player;
    public Vector2 followOffset = new Vector2(0, 0);

    public float thinkSpeed = 3.0f;
    public float detectionRange = 5.0f;
    public float useMagnitudeRange = 2.5f;
    public float speedMultiplier = 1.0f;
    [Tooltip("Causes the player's Direction to influence the enemy's x offset")]
    public bool usePlayerDirection = true;

    private Rigidbody2D _body;
    private Rigidbody2D _playerBody;
    private Vector3 _offset;

    private float _thinkTimer = 3.0f;

    private void Start()
    {
        _body = this.gameObject.GetComponent<Rigidbody2D>();
        _playerBody = player.gameObject.GetComponent<Rigidbody2D>();
        _offset = new Vector3(followOffset.x, followOffset.y, 0);
    }

    /// <summary> Creates a copy of an Object and flings it at the target. </summary>
    /// <param name="pObject">The original GameObject to be copied. Requires a Rigidbody2D.</param>
    /// <param name="pTarget">The position of the target.</param>
    /// <param name="pVelocity">The velocity given to the Object.</param>
    /// <param name="arch">If the object should be thrown in an arch or not False by default.</param>
    public void flingItem(GameObject pObject, Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f, bool arch = false, float pGravityScale = 1.0f)
    {
        GameObject projectile = Instantiate(pObject);
        projectile.transform.position = gameObject.transform.position;
        Rigidbody2D projBody = projectile.GetComponent<Rigidbody2D>();

        if (arch)
        {
            projectile.GetComponent<Rigidbody2D>().gravityScale = pGravityScale;
            Vector3 normalized = new Vector3(new Vector3(pTarget.x - gameObject.transform.position.x, 0, 0).normalized.x, 1).normalized;
            Debug.Log(normalized);
            projBody.AddForce(new Vector3(normalized.x * (pXVelocity), normalized.y * (pYVelocity)), ForceMode2D.Impulse);
        }
        else
        {
            projBody.gravityScale = 0.0f;
            projBody.AddForce((pTarget - gameObject.transform.position).normalized * pXVelocity, ForceMode2D.Impulse);
        }
    }

    private void UpdatePosition()
    {
        float distance = (player.position - gameObject.transform.position).magnitude;

        if (distance < detectionRange && _playerBody.velocity.magnitude > 0.1f)
        {
            if (usePlayerDirection)
            {
                if (_playerBody.velocity.x < -2)
                    _offset.x = -followOffset.x;
                else if(_playerBody.velocity.x > 2)
                    _offset.x = followOffset.x;
            }

            Vector3 subtracted = (player.position - gameObject.transform.position + _offset);
            _body.velocity = (subtracted.normalized * speedMultiplier);
        }
    }

    private void Step()
    {
        float distance = (player.position - gameObject.transform.position).magnitude;
        Debug.Log(distance);

        if (distance < detectionRange)
        {
            if (distance < useMagnitudeRange) { }

            else { }

        }

        Debug.Log("called step");
        _thinkTimer = thinkSpeed;
    }

    private void Update()
    {
        UpdatePosition();

        if (_thinkTimer <= 0) Step();
        else _thinkTimer -= Time.deltaTime;
    }
}
