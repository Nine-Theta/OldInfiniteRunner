using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTest
{
    private Vector2 _position;
    private List<NodeTest> _connections;
    private int _size;

    //additions specific for path finding
    private NodeTest _nodeParent;                                //keep track of where we came from
    public float costCurrent = 0;                               //keep track of cost up to now
    public float costEstimate = 0;                             //keep track of cost estimate to goal

    public NodeTest(int pSize, Color pColor, Vector2 pPosition)
    {
        _size = pSize;
        position = pPosition;
        _connections = new List<NodeTest>();
    }

    public Vector2 position
    {
        set
        {
            if (value == null)
                _position = Vector2.zero;
            else
                _position = value;

            //x = _position.x;
            //y = _position.y;
        }
        get { return _position; }
    }

    public NodeTest parentNode
    {
        get { return _nodeParent; }
        set { _nodeParent = value; }
    }

    public void AddConnection(NodeTest node)
    {
        _connections.Add(node);

        //for debugging, map node connection to a corresponding line segment
        //_nodeToLineSegment[node] = visualLink;
    }

    public bool HasConnection(NodeTest node)
    {
        return _connections.IndexOf(node) > -1;
    }

    public int GetConnectionCount()
    {
        return _connections.Count;
    }

    public NodeTest GetConnectionAt(int index)
    {
        return _connections[index];
    }

    #region IComparable implementation

    //public int CompareTo(NodeTest pOther)
    //{
        //return (costEstimate + costCurrent).CompareTo(pOther.costCurrent + pOther.costEstimate);
    //}

    #endregion

}


