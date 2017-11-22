using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleTest : MonoBehaviour {

    public GameObject player;
    private Rigidbody2D _body;
    
	private void Start () {
        _body = this.gameObject.GetComponent<Rigidbody2D>();
        _body.centerOfMass = new Vector2(100, 10.25f);
    }
	
	private void Update () {
        this.gameObject.transform.localPosition = new Vector3(0, -0.5f, -1);
        float playerVelX = player.GetComponent<Rigidbody2D>().velocity.x;
        if (playerVelX >= -15 && playerVelX <= 25) _body.rotation = -playerVelX * 4;
        //_body.rotation++;
    }
}
