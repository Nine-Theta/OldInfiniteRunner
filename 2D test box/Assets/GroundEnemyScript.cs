using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyScript : MonoBehaviour
{

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.collider.tag);
        if (coll.collider.tag == "Explosion") Destroy(gameObject);
    }
}
