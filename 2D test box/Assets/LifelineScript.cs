using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifelineScript : MonoBehaviour
{
    private Transform _player;
    private LineRenderer _lineRenderer;
    private bool _unhooked = true;

    public bool isUnhooked
    {
        set { _unhooked = value; }
    }


    private void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _player = MovementScript.GetPlayer().transform;
        _lineRenderer.SetPosition(0, _player.transform.position);
    }

    private void Update()
    {
        if (_unhooked)
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _player.transform.position);
        else
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 2, _player.transform.position);
    }

    public void AddPoint(Vector3 point)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, point);
    }

    public void RemoveLastPoint()
    {
        if (_lineRenderer.positionCount > 2)
            _lineRenderer.positionCount--;
    }
}
