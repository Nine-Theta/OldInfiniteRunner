using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {

    public float sideVelocity = 0.4f;
    public float jumpVelocity = 5.0f;
    public float maxXVelocity = 10.0f;

    public bool extraJumpEnabled = true;
    public int jumpsAmount = 2;
    public int jumpWait = 30;

    public bool jetEnabled = true;
    public float jetWait = 20;
    public float jetVelocity = 0.14f;

    private Rigidbody2D _body;
    private float _jetCounter = 0;
    private float _jumpCounter = 0;
    private int _jumpsLeft = 0;

    private void Start(){
        _body = this.GetComponent<Rigidbody2D>();
        _jetCounter = jetWait;
    }

    private void Update(){
        if (Input.GetKey(KeyCode.A) && _body.velocity.x > -maxXVelocity) _body.AddForce(new Vector2(-sideVelocity, 0.0f));
        if (Input.GetKey(KeyCode.D) && _body.velocity.x < maxXVelocity) _body.AddForce(new Vector2(sideVelocity, 0.0f));

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))){
            if ((_body.velocity.y <= 0.01f && _body.velocity.y >= -0.01f) || (extraJumpEnabled && _jumpsLeft > 0 && _jumpCounter <= 0))
            {
                _body.AddForce(new Vector2(0.0f, jumpVelocity));
                _jumpsLeft--;
            }
            else if (extraJumpEnabled && _jumpCounter >= 0 && _jumpsLeft > 0) _jumpCounter--;
        }

        if (_body.velocity.y <= 0.0001f && _body.velocity.y >= -0.0001f) _jumpsLeft = jumpsAmount;

        if (jetEnabled && Input.GetKey(KeyCode.Space)){
            if ( _jetCounter <= 0){
                _body.AddForce(new Vector2(0.0f, jetVelocity));
            }
            else{
                _jetCounter--;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space)) _jetCounter = jetWait;

        if (Input.GetKey(KeyCode.S)) _body.AddForce(new Vector2(0.0f, -0.5f));
    }
}
