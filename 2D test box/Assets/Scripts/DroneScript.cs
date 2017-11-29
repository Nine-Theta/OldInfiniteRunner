using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    public GameObject thrownItem;
    public Transform player;

    public float thinkSpeed = 3.0f;
    public float detectionRange = 5.0f;
    public float followSpeedMult = 1.0f;
    public float throwSpeedMult = 1.0f;
    public float radiusOffset = 0.0f;
    public float rotateSpeed = 1.0f;

    private Rigidbody2D _body;
    private Rigidbody2D _playerBody;

    private float _thinkTimer = 3.0f;
    private float _rotateSpeed = 0.0f;
    private float _angle = 0;

    private void Start()
    {
        if (player == null)
            player = MovementScript.GetPlayer().transform;

        _thinkTimer = Random.Range(0.0f, thinkSpeed);
        _body = this.gameObject.GetComponent<Rigidbody2D>();
        _playerBody = player.gameObject.GetComponent<Rigidbody2D>();

        _rotateSpeed = rotateSpeed * (Mathf.PI / 180);
    }

    /// <summary> Creates a copy of an Object and flings it at the target. </summary>
    /// <param name="pObject">The original GameObject to be copied. Requires a Rigidbody2D.</param>
    /// <param name="pTarget">The position of the target.</param>
    public void flingItem(GameObject pObject, Vector3 pTarget, float pVelocity = 1.0f)
    {
        GameObject projectile = Instantiate(pObject);
        projectile.transform.position = gameObject.transform.position;
        Rigidbody2D projBody = projectile.GetComponent<Rigidbody2D>();

        projBody.gravityScale = 0.0f;
        projBody.AddForce((pTarget - gameObject.transform.position).normalized * pVelocity, ForceMode2D.Impulse);
    }

    private void UpdatePosition()
    {
        Vector2 subtracted = new Vector2(player.position.x - gameObject.transform.position.x, player.position.y - gameObject.transform.position.y);
        //_body.AddForce(new Vector2(subtracted.normalized.x * followSpeedMult, 0), ForceMode2D.Force);
        //_body.velocity = new Vector2(_body.velocity.x, subtracted.normalized.y * subtracted.magnitude);

        _angle += _rotateSpeed;

        float s = Mathf.Sin(_angle);
        float sy = Mathf.Cos(_angle);

        //gameObject.transform.position = new Vector2(player.position.x + radiusOffset * s, player.position.y + radiusOffset * sy);

        //Debug.Log("submag: " +subtracted.magnitude );

        //_body.velocity = new Vector2(subtracted.x + radiusOffset * s, subtracted.y + radiusOffset * sy);
        _body.velocity = new Vector2(subtracted.x + _playerBody.velocity.x + radiusOffset * s, subtracted.y + _playerBody.velocity.y + radiusOffset * sy);
        //_body.velocity += new Vector2();


    }

    private void Step()
    {
        //flingItem(thrownItem, new Vector2(player.position.x + _playerBody.velocity.x, player.position.y), throwSpeedMult);

        //UpdatePosition();

        _thinkTimer = thinkSpeed;
    }

    private void Update()
    {
        UpdatePosition();
        /**/
        //if ((player.position - gameObject.transform.position).magnitude < detectionRange)
        {
            //UpdatePosition();

            if (_thinkTimer <= 0) Step();
            else _thinkTimer -= Time.deltaTime;
        }/**/
    }
}
