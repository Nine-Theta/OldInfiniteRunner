using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour
{
    public GameObject thrownItem;
    public Transform player;
    public Vector2 followOffset = new Vector2(0, 0);

    public float thinkSpeed = 3.0f;
    public float detectionRange = 5.0f;
    public float followSpeedMult = 1.0f;
    public float throwSpeedMult = 1.0f;
    public float throwHeightMult = 1.0f;
    public float gravityScale = 1.0f;

    public bool throwArc = true;

    private Rigidbody2D _body;
    private Rigidbody2D _playerBody;
    private Vector3 _offset;

    private float _thinkTimer = 3.0f;

    private void Start()
    {
        _thinkTimer = Random.Range(0.0f, thinkSpeed);
        _body = this.gameObject.GetComponent<Rigidbody2D>();
        _playerBody = player.gameObject.GetComponent<Rigidbody2D>();
        _offset = new Vector3(followOffset.x, followOffset.y, 0);
    }

    /// <summary> Creates a copy of an Object and flings it at the target. </summary>
    /// <param name="pObject">The original GameObject to be copied. Requires a Rigidbody2D.</param>
    /// <param name="pTarget">The position of the target.</param>
    /// <param name="arc">If the object should be thrown in an arc or not. False by default.</param>
    public void flingItem(GameObject pObject, Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f, bool arc = false, float pGravityScale = 1.0f)
    {
        GameObject projectile = Instantiate(pObject);
        projectile.transform.position = gameObject.transform.position;
        Rigidbody2D projBody = projectile.GetComponent<Rigidbody2D>();

        if (arc)
        {
            projectile.GetComponent<Rigidbody2D>().gravityScale = pGravityScale;

            float x = Mathf.Abs(pTarget.x - gameObject.transform.position.x);
            float y = Mathf.Abs(pTarget.y - gameObject.transform.position.y);

            Vector2 normalized = new Vector2(pTarget.x - gameObject.transform.position.x, y).normalized;

            projBody.AddForce(new Vector2(normalized.x * x * pXVelocity, normalized.y * (y+x) * pYVelocity), ForceMode2D.Impulse);
        }
        else
        {
            projBody.gravityScale = 0.0f;
            projBody.AddForce((pTarget - gameObject.transform.position).normalized * pXVelocity, ForceMode2D.Impulse);
        }
    }

    private void UpdatePosition()
    {
        Vector2 subtracted = new Vector2(player.position.x - gameObject.transform.position.x + _offset.x, player.position.y - gameObject.transform.position.y + _offset.y);

        if (Mathf.Abs(player.position.x - gameObject.transform.position.x) < _offset.x)
            subtracted.x = 0;

        _body.AddForce(new Vector2(subtracted.normalized.x * followSpeedMult, 0), ForceMode2D.Force);
        _body.velocity = new Vector2(_body.velocity.x, subtracted.normalized.y * subtracted.magnitude);
    }

    private void Step()
    {
        if (throwArc)
            flingItem(thrownItem, new Vector2(player.position.x + _playerBody.velocity.x, player.position.y), throwSpeedMult, throwHeightMult, true, gravityScale);
        else
            flingItem(thrownItem, new Vector2(player.position.x + _playerBody.velocity.x, player.position.y), throwSpeedMult);

        _thinkTimer = thinkSpeed;
    }

    private void Update()
    {
        if ((player.position - gameObject.transform.position).magnitude < detectionRange)
        {
            UpdatePosition();

            if (_thinkTimer <= 0) Step();
            else _thinkTimer -= Time.deltaTime;
        }
    }
}
