using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursorScript : MonoBehaviour
{
    public MovementMaybe player;
    public GameObject hookPrefab;
    public GameObject potionPrefab;
    //Distance between mouse and player
    private Vector2 _mouseDistance;
    private GameObject _prevHook;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        TrackMouse();
        if (Input.GetMouseButtonDown(0))
        {
            ThrowPotion();
            //HookTarget();
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
        GameObject potion = Instantiate(potionPrefab, player.transform.position, transform.rotation);
        potion.GetComponent<PotionScript>().SetForce(_mouseDistance.normalized);
        Physics2D.IgnoreCollision(potion.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

    private void HookTarget()
    {
        if (_prevHook != null)
            Destroy(_prevHook);
        RaycastHit2D info = Physics2D.Raycast(player.transform.position, _mouseDistance.normalized, _mouseDistance.magnitude);
        if (info.point != Vector2.zero)
            _prevHook = Instantiate(hookPrefab, info.point, transform.rotation);
        else
            _prevHook = Instantiate(hookPrefab, transform.position, transform.rotation);
        player.Hook(_mouseDistance);
    }
}
