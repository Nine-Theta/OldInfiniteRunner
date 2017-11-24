using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifelineScript : MonoBehaviour
{
    public Transform player;
    private LineRenderer _lineRenderer;
    private bool doUpdate = true;

    public bool doLineUpdate
    {
        set { doUpdate = value; }
    }


    private void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, player.transform.position);
        player.GetComponent<MovementScript>().SetLifeLine(this);
    }

    private void Update()
    {
        if (doUpdate)
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, player.transform.position);
    }

    public void AddPoint(Vector3 point)
    {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, point);
    }

    ///Not yet done TODO: fix
    private void RemovePoint(int index)
    {
        Debug.Log(_lineRenderer.positionCount);
        if (_lineRenderer.positionCount <= 2)
            return;
        for (int i = index; i < _lineRenderer.positionCount - 2; i++)
        {
            _lineRenderer.SetPosition(i, _lineRenderer.GetPosition(i + 1));
        }
        _lineRenderer.positionCount--;
    }

}
