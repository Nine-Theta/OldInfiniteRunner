using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTest : MonoBehaviour {

    public GameObject bullet;
    
	private void Start () {
		
	}
	
	private void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.position = this.gameObject.transform.position + new Vector3(-0.5f,0);
            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.15f, 0));
        }
	}
}
