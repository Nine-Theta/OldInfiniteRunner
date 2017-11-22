using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float decayTime = 3.0f;


    private void Start()
    {

    }

    private void Update()
    {
        decayTime -= Time.deltaTime;
        if(decayTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
