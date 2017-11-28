using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public GameObject player;

    public float thinkSpeed = 3.0f;
    public float detectionRange = 5.0f;
    public float useMagnitudeRange = 2.0f;

    public float jumpDistance = 1.0f;
    public float jumpHeight = 1.0f;

    private float _thinkTimer = 3.0f;
    private Animator _animator;
    private bool _woundUp = false;
    private bool _jumping = false;

    private void Start()
    {
        _thinkTimer = thinkSpeed + Random.Range(0.0f, 3.0f);
        _animator = GetComponent<Animator>();
        if (player == null)
        {
            player = MovementScript.GetPlayer();
        }
    }

    private void jumpAttack(Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f)
    {
        Vector3 normalized = (pTarget - gameObject.transform.position).normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(normalized.x * pXVelocity, normalized.y + 1.0f * pYVelocity));
    }

    /// <summary>
    /// Public function for the animator, calling the private function internally
    /// </summary>
    public void JumpAttack()
    {
        float distance = (player.transform.position - gameObject.transform.position).magnitude;
        _animator.SetBool("JumpWindup", false);
        _woundUp = true;
        if (distance < detectionRange)
        {
            if (distance < useMagnitudeRange)
                jumpAttack(player.transform.position, distance, jumpHeight);
            else
                jumpAttack(player.transform.position, jumpDistance, jumpHeight);
        }
    }

    private void Step()
    {
        _animator.SetBool("JumpWindup", true);
        _animator.SetBool("Land", false);
        _jumping = true;
        _thinkTimer = thinkSpeed;
    }

    private void Update()
    {
        float distance = (player.transform.position - gameObject.transform.position).magnitude;
        if (!_jumping && distance < detectionRange)
        {
            if (_thinkTimer <= 0)
                Step();
            else
                _thinkTimer -= Time.deltaTime;
        }
        if(_woundUp && gameObject.GetComponent<Rigidbody2D>().velocity.y < 0.0f)
        {
            _woundUp = false;
            _animator.SetBool("Fall", true);
        }
        else if (_animator.GetBool("Fall") && gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0.0f)
        {
            _animator.SetBool("Fall", false);
            _animator.SetBool("Land", true);
            _jumping = false;
        }
    }
}
