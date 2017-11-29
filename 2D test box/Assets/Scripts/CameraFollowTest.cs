using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTest : MonoBehaviour
{
    public Transform _player;
    public Vector2 followOffset = new Vector2(0, 0);

    public float speedMultiplier = 1.0f;

    private Rigidbody2D _body;

    private void Start()
    {
        _body = this.gameObject.GetComponent<Rigidbody2D>();
    }

    //https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html
    private void LateUpdate()
    {
        Vector2 subtracted = (_player.position - gameObject.transform.position);
        subtracted += followOffset;
        _body.velocity = (subtracted.normalized * subtracted.magnitude * speedMultiplier);
    }
}
