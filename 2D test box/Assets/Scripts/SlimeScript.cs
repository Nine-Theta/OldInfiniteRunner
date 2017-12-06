using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private float thinkSpeed = 3.0f;
    [SerializeField]
    private float detectionRange = 5.0f;
    [SerializeField]
    private float useMagnitudeRange = 2.0f;

    [SerializeField]
    private float jumpDistance = 1.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float _idleDelay = 5.0f;
    [SerializeField]
    private AudioClip _idleSound;
    [SerializeField]
    private AudioClip _attackSound;
    [SerializeField]
    private AudioClip _damageSound;
    [SerializeField]
    private AudioClip _deathSound;
    [SerializeField]
    private AudioClip _alertSound;

    private float _thinkTimer = 3.0f;
    private float _idleTimer = 5.0f;
    private Animator _animator;
    private HealthBarScript _healthBar;
    private bool _woundUp = false;
    private bool _jumping = false;
    private Color _initialColor;
    private AudioSource _audio;

    private void Start()
    {
        _thinkTimer = Random.Range(0.0f, thinkSpeed);
        _animator = GetComponent<Animator>();
        player = MovementScript.GetPlayer().transform;
        _healthBar = GetComponentInChildren<HealthBarScript>();
        _initialColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = Color.black;
        _audio = GetComponent<AudioSource>();
        _idleTimer = Random.Range(0.0f, _idleDelay);
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
        float distance = (player.position - gameObject.transform.position).magnitude;
        _animator.SetBool("JumpWindup", false);
        _woundUp = true;
        //if (distance < detectionRange)
        //{
        if (distance < useMagnitudeRange)
            jumpAttack(player.position, distance, jumpHeight);
        else
            jumpAttack(player.position, jumpDistance, jumpHeight);
        //}
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetAnimFalse()
    {
        _animator.SetBool("Dead", false);
    }

    private void Step()
    {
        _animator.SetBool("JumpWindup", true);
        _animator.SetBool("Land", false);
        _jumping = true;
        _thinkTimer = thinkSpeed;
        _audio.PlayOneShot(_attackSound);
    }

    private void Update()
    {
        if (_healthBar.isAlive)
        {
            
            float distance = (player.position - gameObject.transform.position).magnitude;
            if (!_jumping && distance < detectionRange)
            {
                _idleTimer -= Time.deltaTime;
                if (_idleTimer <= 0.0f)
                {
                    _audio.PlayOneShot(_idleSound);
                    _idleTimer = _idleDelay;
                }
                if (GetComponent<SpriteRenderer>().color == Color.black)
                {
                    _audio.PlayOneShot(_alertSound);
                    GetComponent<SpriteRenderer>().color = _initialColor;
                }
                if (_thinkTimer <= 0)
                    Step();
                else
                    _thinkTimer -= Time.deltaTime;
            }
            if (_woundUp && gameObject.GetComponent<Rigidbody2D>().velocity.y < 0.0f)
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

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Explosion")
        {
            _audio.PlayOneShot(_damageSound);
            if (_healthBar.isAlive & !_healthBar.TakeDamage())
            {
                _animator.SetBool("Fall", false);
                _animator.SetBool("Land", false);
                _animator.SetBool("JumpWindup", false);
                _animator.SetBool("Dead", true);
                _audio.PlayOneShot(_deathSound);
            }
        }
    }
}
