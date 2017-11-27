using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public Transform player;

    public float thinkSpeed = 3.0f;
    public float detectionRange = 5.0f;
    public float useMagnitudeRange = 2.0f;

    public float jumpDistance = 1.0f;
    public float jumpHeight = 1.0f;

    private float _thinkTimer = 3.0f;

    private void jumpAttack(Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f)
    {
        Vector3 normalized = (pTarget - gameObject.transform.position).normalized;
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(normalized.x * pXVelocity, normalized.y + 1.0f * pYVelocity));
    }

    private void Step()
    {
        float distance = (player.position - gameObject.transform.position).magnitude;

        if (distance < detectionRange)
        {
            if (distance < useMagnitudeRange)
                jumpAttack(player.position, distance, jumpHeight);
            else
                jumpAttack(player.position, jumpDistance, jumpHeight);
        }

        Debug.Log("called step");
        _thinkTimer = thinkSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) jumpAttack(player.position, jumpDistance, jumpHeight);

        if (_thinkTimer <= 0) Step();
        else _thinkTimer -= Time.deltaTime;
    }
}