using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    [SerializeField]
    private float delayInSeconds = 1.0f;
    [SerializeField]
    private bool randomDelay = false;

    private Animator _animator;

    void Start()
    {
        if (randomDelay)
            delayInSeconds = Random.Range(0.0f, delayInSeconds);
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    void Update()
    {
        delayInSeconds -= Time.deltaTime;
        if(delayInSeconds <= 0.0f)
        {
            _animator.enabled = true;
            Destroy(this);
        }
    }
}
