using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    public int maxHP = 10;
    private float _hp = 10;
    private float maxWidth = 0.4f;

    public bool isAlive
    {
        get
        {
            if (_hp < 0)
                GetComponentInParent<Rigidbody2D>().simulated = false;
            return _hp > 0;
        }
    }

    private void Start()
    {
        _hp = maxHP;
        maxWidth = GetComponent<SpriteRenderer>().size.x;
    }

    private void Update()
    {

    }

    public bool TakeDamage()
    {
        _hp--;
        if (_hp < 0)
            _hp = 0;
        AdjustBar();
        return _hp > 0;
    }

    public bool TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp < 0)
            _hp = 0;
        AdjustBar();
        return _hp > 0;
    }

    public void Heal()
    {
        _hp++;
        if (_hp > maxHP)
            _hp = maxHP;
        AdjustBar();
    }

    public void Heal(int healing)
    {
        _hp += healing;
        if (_hp > maxHP)
            _hp = maxHP;
        AdjustBar();
    }

    private void AdjustBar()
    {
        Vector2 size = GetComponent<SpriteRenderer>().size;
        size.x = (maxWidth * (_hp / maxHP));
        GetComponent<SpriteRenderer>().size = size;
    }
}
