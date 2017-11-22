using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordTest : MonoBehaviour {

    public GameObject cordPart;
    public GameObject player;

    public int cordLength = 2;

    private GameObject[] _cordParts;
    
	private void Start () {
        _cordParts = new GameObject[cordLength];

        for (int i = 0; i < cordLength; i++)
        {
            GameObject newCord = Instantiate(cordPart);
            _cordParts[i] = newCord;
        }
    }
	
	private void Update () {
        if((this.gameObject.transform.position - player.transform.position).magnitude / cordLength > 0.25f){
            player.transform.position = this.gameObject.transform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

		for (int i = 0; i < cordLength; i++)
        {
            _cordParts[i].transform.position = (this.gameObject.transform.position + ((player.transform.position - this.gameObject.transform.position).normalized * (this.gameObject.transform.position - player.transform.position).magnitude/cordLength*i));
        }
	}
}
