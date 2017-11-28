using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogFieldScript : MonoBehaviour
{
    [Tooltip("The time in between dealing damage to the player")]
    public float damageCooldown = 1.0f;

    private float _damageTimer = 0.0f;
    private HealthBarScript _healthRef;

    private void Start()
    {

    }

    private void Update()
    {
        if (_healthRef != null)
            Act();
    }

    private void Act()
    {
        _damageTimer -= Time.deltaTime;
        if (_damageTimer <= 0.0f)
        {
            _healthRef.TakeDamage();
            _damageTimer = damageCooldown;
        }
    }

    public void EnterFog(HealthBarScript script)
    {
        _healthRef = script;
    }

    public void LeaveFog()
    {
        _healthRef = null;
    }
}
