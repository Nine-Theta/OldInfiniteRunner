using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifelineScript : MonoBehaviour
{
    private Transform _player;
    private LineRenderer _lineRenderer;
    private bool doUpdate = true;

    public bool doLineUpdate
    {
        set { doUpdate = value; }
    }


    private void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _player = MovementScript.GetPlayer().transform;
        _lineRenderer.SetPosition(0, _player.transform.position);
    }

    private void Update()
    {
        if (doUpdate)
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _player.transform.position);
    }

    public void AddPoint(Vector3 point)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, point);
    }

    public void RemoveLastPoint()
    {
        _lineRenderer.positionCount--;
    }
}
