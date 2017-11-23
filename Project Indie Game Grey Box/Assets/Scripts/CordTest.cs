using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordTest : MonoBehaviour {

    public GameObject cordPart;
    public GameObject player;

    public int cordLength = 500;
    public float cordFloppiness = 1; //Floppiness™

    private GameObject[] _cordParts;
    private float _sineLengthModifier = 0.002f;
    
	private void Start () {
        _cordParts = new GameObject[cordLength];

        for (int i = 0; i < cordLength; i++)
        {
            GameObject newCord = Instantiate(cordPart);
            _cordParts[i] = newCord;
        }
        _sineLengthModifier = 1.0f/cordLength;
    }
	
	private void Update () {
        if((this.gameObject.transform.position - player.transform.position).magnitude / cordLength > 0.25f){
            player.transform.position = this.gameObject.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        /*
		for (int i = 0; i < cordLength; i++)
        {
            _cordParts[i].transform.position = (this.gameObject.transform.position + ((player.transform.position - this.gameObject.transform.position).normalized * (this.gameObject.transform.position - player.transform.position).magnitude/cordLength*i));
        }
        /**/

        for (int i = 0; i < cordLength; i++)
        {
            //_cordParts[i].transform.position = new Vector2(-(this.gameObject.transform.position.x - player.transform.position.x) / cordLength * i, -Mathf.Log((this.gameObject.transform.position.y - player.transform.position.y)*i, 2.0f));

            _cordParts[i].transform.position = (this.gameObject.transform.position + ((player.transform.position - this.gameObject.transform.position).normalized * (this.gameObject.transform.position - player.transform.position).magnitude / cordLength * i));
            _cordParts[i].transform.position = (new Vector3 (_cordParts[i].transform.position.x, _cordParts[i].transform.position.y + (cordFloppiness * Mathf.Sin((_sineLengthModifier * 3.14f) * i + 3.14f))));
            float x = _cordParts[i].transform.position.x;
            float y = _cordParts[i].transform.position.y;
        }
        /**/
    }
}
