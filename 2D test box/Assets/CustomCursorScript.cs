using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursorScript : MonoBehaviour
{
    public MovementScript player;
    public GameObject hookPrefab;
    public GameObject potionPrefab;
    public float hookRange = 7.0f;
    public float hookCooldown = 1.0f;
    public float potionCooldown = 1.0f;
    //Distance between mouse and player
    private Vector2 _mouseDistance;
    private GameObject _prevHook;
    private float _hookTimer = 0.0f;
    private float _potionTimer = 0.0f;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        _potionTimer -= Time.deltaTime;
        _hookTimer -= Time.deltaTime;
        TrackMouse();
        if (Input.GetMouseButtonDown(0))
        {
            if (_potionTimer <= 0.0f)
                ThrowPotion();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (_hookTimer <= 0.0f)
                HookTarget();
        }
    }

    private void TrackMouse()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v.z = 0;
        transform.position = v;

        _mouseDistance = new Vector2(v.x, v.y);
        Vector2 playerPos = player.transform.position;
        _mouseDistance -= playerPos;
    }

    private void ThrowPotion()
    {
        _potionTimer = potionCooldown;
        GameObject potion = Instantiate(potionPrefab, player.transform.position, transform.rotation);
        potion.GetComponent<PotionScript>().SetForce(_mouseDistance.normalized);
        potion.GetComponent<PotionScript>().SetPlayer(player.gameObject);
        Physics2D.IgnoreCollision(potion.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

    private void HookTarget()
    {
        if (_prevHook != null)
            Destroy(_prevHook);
        RaycastHit2D info = Physics2D.Raycast(player.transform.position, _mouseDistance.normalized, hookRange, 1);
        if (info.point != Vector2.zero)
        {
            _prevHook = Instantiate(hookPrefab, info.point, transform.rotation);
            player.Hook(_mouseDistance, _prevHook);
            SetCursorColor(1.0f, 1.0f, 1.0f);
            _hookTimer = hookCooldown;
        }
        else
        {
            SetCursorColor(1.0f, 0.0f, 0.0f);
        }
    }

    private void SetCursorColor(float r, float g, float b, float a = 1.0f)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.r = r;
        color.g = g;
        color.b = b;
        color.a = a;
        GetComponent<SpriteRenderer>().color = color;
    }
}
