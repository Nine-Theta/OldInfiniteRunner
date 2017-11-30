using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour {

    public Transform cameraWisp;

    public Transform background;
    public Transform foregroundOne;
    public Transform foregroundTwo;
    public Transform foregroundThree;

    public float backgroundSpeed = 0.5f;
    public float foregroundOneSpeed = 1.5f;
    public float foregroundTwoSpeed = 2.0f;
    public float foregroundThreeSpeed = 2.5f;

    void Start () {
		
	}
	
	private void Update () {

        background.position = new Vector3(cameraWisp.position.x * backgroundSpeed, cameraWisp.position.y, background.position.z);
        foregroundOne.position = new Vector3(cameraWisp.position.x * foregroundOneSpeed, cameraWisp.position.y, foregroundOne.position.z);
        foregroundTwo.position = new Vector3(cameraWisp.position.x * foregroundTwoSpeed, cameraWisp.position.y, foregroundTwo.position.z);
        foregroundThree.position = new Vector3(cameraWisp.position.x * foregroundThreeSpeed, cameraWisp.position.y, foregroundThree.position.z);

    }
}
