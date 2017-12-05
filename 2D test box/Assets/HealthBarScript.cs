using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 10;
    private float _hp = 10;
    private float maxWidth = 0.4f;
    private bool _isEnemy = false;
    private Color _initialColor;

    public bool isAlive
    {
        get
        {
            if (_hp <= 0)
            {
                GetComponentInParent<Rigidbody2D>().simulated = false;
                if (!_isEnemy)
                {
                    GetComponentInParent<Rigidbody2D>().GetComponent<SpriteRenderer>().color = Color.red;
                    CustomCursorScript.GetCursor().SetActive(false);
                    FadeToBlackScript.GetScript().fade = true;
                }
            }
            return _hp > 0;
        }
    }

    private void Start()
    {
        _hp = maxHP;
        maxWidth = GetComponent<SpriteRenderer>().size.x;
        _isEnemy = !GetComponent<SpriteRenderer>().enabled;
        _initialColor = GetComponentInParent<SpriteRenderer>().color;
    }

    private void Update()
    {

    }

    public bool TakeDamage()
    {
        if (_hp <= 0)
            return false;
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
        if (_isEnemy)
        {
            Color newColor = _initialColor;
            newColor.r = 1 - (_hp / maxHP);
            newColor.g = (_hp / maxHP);
            newColor.b = (_hp / maxHP);
            GetComponentInParent<Rigidbody2D>().GetComponent<SpriteRenderer>().color = newColor;
        }
        else
        {
            Vector2 size = GetComponent<SpriteRenderer>().size;
            size.x = (maxWidth * (_hp / maxHP));
            GetComponent<SpriteRenderer>().size = size;
        }
    }
}
