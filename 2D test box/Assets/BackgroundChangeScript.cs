using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChangeScript : MonoBehaviour
{
    [SerializeField]
    private bool _fadeToBlack = false;
    [SerializeField]
    private bool _destroyGameObjectAfterwards = false;
    [SerializeField]
    private bool _stayAtEnable = true;
    [SerializeField]
    [Tooltip("The maximum time in the random float that returns the glitchy effect")]
    private float _glitchTimeMax = 1.0f;
    [SerializeField]
    private float _blinkTime = 5.0f;
    [SerializeField]
    private List<GameObject> _disableList;
    [SerializeField]
    private List<GameObject> _enableList;

    private bool _activated = false;
    private float _glitchTimer;
    private bool _flipped = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (_activated)
        {
            _blinkTime -= Time.deltaTime;
            _glitchTimer -= Time.deltaTime;
            if (_glitchTimer <= 0.0f)
            {
                Activate();
            }
        }
    }

    public void Activate()
    {
        if (_flipped)
        {
            foreach (GameObject disable in _disableList)
                disable.SetActive(true);
            foreach (GameObject enable in _enableList)
                enable.SetActive(false);
        }
        else
        {
            foreach (GameObject disable in _disableList)
                disable.SetActive(false);
            foreach (GameObject enable in _enableList)
                enable.SetActive(true);
        }
        _activated = true;
        _glitchTimer = Random.Range(0.0f, _glitchTimeMax);
        _flipped = !_flipped;
        if (_fadeToBlack && _blinkTime <= 0)
        {
            FadeToBlackScript.GetScript().fade = true;
            _activated = false;
        }
        else if (_blinkTime <= 0)
        {
            _activated = false;
            if (_stayAtEnable && !_enableList[0].activeSelf)
                Activate();
        }
        if (_destroyGameObjectAfterwards)
            Destroy(this.gameObject);
    }
}
