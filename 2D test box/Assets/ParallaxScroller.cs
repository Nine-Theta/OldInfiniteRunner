using UnityEngine;
using System.Collections;

public class ParallaxScroller : MonoBehaviour
{
    public float scrollSpeed = 0.01f;
    public float tileSizeZ;

    private Vector3 _startPosition;
    private Vector3 _playerStartPosition;
    private Transform _player;

    void Start()
    {
        tileSizeZ = Mathf.Abs(tileSizeZ);
        _startPosition = transform.position;
        _player = MovementScript.GetPlayer().transform;
        _playerStartPosition = _player.position;
    }

    void Update()
    {
        float newPosition = (_playerStartPosition.x - _player.position.x) * scrollSpeed; //Mathf.Repeat( Mathf.Abs(_playerStartPosition.x - _player.position.x) * scrollSpeed, tileSizeZ);
        //if (_player.position.x > _playerStartPosition.x)
        //    newPosition *= -1;
        Vector3 addPos = new Vector3(newPosition, 0.0f);
        transform.position = _startPosition + addPos;
    }
}
