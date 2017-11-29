using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootScript : MonoBehaviour
{
    private MovementScript _movementScript;

    private void Start()
    {
        _movementScript = GetComponentInParent<MovementScript>();
    }

    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.tag == "Terrain")
            _movementScript.Ground();
    }
}
