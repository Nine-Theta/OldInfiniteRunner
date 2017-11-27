using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTest : MonoBehaviour
{
    public Transform _player;

    public float speedMultiplier = 1.0f;

    private Rigidbody2D _body;

    private void Start()
    {
        _body = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 subtracted = (_player.position - gameObject.transform.position);
        _body.velocity = (subtracted.normalized * subtracted.magnitude * speedMultiplier);
    }
}
