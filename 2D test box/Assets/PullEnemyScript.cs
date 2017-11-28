using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullEnemyScript : MonoBehaviour
{
    public float pullRange = 10.0f;
    public float detectionRange = 15.0f;
    public float pullModifier = 1.0f;
    public Transform player;

    private void Start()
    {
        player = MovementScript.GetPlayer().transform;
    }

    private void Update()
    {
        float distance = (transform.position - player.position).magnitude;
        if (distance < detectionRange)
        {
            Pull(distance);
        }
    }

    private void Pull(float distance)
    {
        GetComponent<SpriteRenderer>().color = Color.cyan;
        if(!player.GetComponent<MovementScript>().isHooked && distance < pullRange)
        {
            player.GetComponent<Rigidbody2D>().AddForce((transform.position - player.position).normalized * pullModifier);
        }
    }
}
