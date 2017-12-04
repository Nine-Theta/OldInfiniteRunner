using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node> //Send nodes
{
    public float costCurrent = 0;                               //keep track of cost up to now
    public float costEstimate = 0;                             //keep track of cost estimate to goal

    private Node _parent = null;
    private List<Node> _connections;

    private Vector2 _position = Vector2.zero;

    private string _label = "";

    public Node()
    {
        _connections = new List<Node>();
    }

    public Vector2 Position
    {
        set
        {
            if (value == null)
                _position = Vector2.zero;
            else
                _position = value;
        }

        get { return _position; }
    }

    public Node Parent
    {
        set { _parent = value; }
        get { return _parent; }
    }

    public void ResetNode()
    {
        _parent = null;
    }

    public void AddConnection(Node pNode)
    {
        _connections.Add(pNode);
    }

    public bool HasConnection(Node pNode)
    {
        return _connections.IndexOf(pNode) > -1;
    }

    public int GetConnectionCount()
    {
        return _connections.Count;
    }

    public Node GetConnectionAt(int pIndex)
    {
        return _connections[pIndex];
    }

    public string Label
    {
        set { _label = value; }
        get { return _label; }
    }

    #region IComparable implementation

    public int CompareTo(Node pOther)
    {
    return (costEstimate + costCurrent).CompareTo(pOther.costCurrent + pOther.costEstimate);
    }

    #endregion
}