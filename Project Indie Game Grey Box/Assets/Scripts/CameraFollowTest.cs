using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTest : MonoBehaviour {
    public GameObject _player;
    public float speedMultiplier = 1.0f;
    private Rigidbody2D _body;
    
	private void Start () {
        //_player = this.gameObject.GetComponentInParent<TestMovement>().gameObject;
        _body = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	private void Update () {

        //if (this.transform.position.x > _player.transform.position.x - 0.1f && this.transform.position.x < _player.transform.position.x + 0.1f && this.transform.position.y > _player.transform.position.y - 0.1f && this.transform.position.y < _player.transform.position.y + 0.1f)
        {
            //_body.velocity = Vector2.zero;
            //this.gameObject.transform.position = _player.transform.position;
        }
        _body.velocity = ((_player.transform.position - this.gameObject.transform.position).normalized * (_player.transform.position - this.gameObject.transform.position).magnitude * speedMultiplier);
        //Debug.Log(_player.transform.position);
	}
}
