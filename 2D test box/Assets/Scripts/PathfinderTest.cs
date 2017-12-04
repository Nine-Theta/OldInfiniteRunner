using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderTest : MonoBehaviour
{
    private NodeTest _startNode;
    private NodeTest _endNode;

    private List<NodeTest> _todoList;
    private List<NodeTest> _doneList;
    private NodeTest _currentNode;

    private bool _done = false;
    private List<NodeTest> _lastPathFound = null;

    public void Start()
    {
    }

    public void SetStartNode(NodeTest pStartNode)
    {
        if (_startNode != null) resetNode(_startNode);
        _startNode = pStartNode;
        resetPathFinder();
    }

    public void SetEndNode(NodeTest pEndNode)
    {
        if (_endNode != null) resetNode(_endNode);
        _endNode = pEndNode;
        resetPathFinder();
    }

    public bool Step()
    {
        //return false;

        //are we able to find a path??
        if (_done || _startNode == null || _endNode == null || _todoList.Count == 0)
        {
            _done = true;
            return true;
        }

        //we are not done, start and end are set and there is at least 1 item on the open list...

        //check if we were already processing nodes, if so color the last processed node as black because it is on the closed list

        //get a node from the open list
        _currentNode = _todoList[0];
        _todoList.RemoveAt(0);

        //and move that node to the closed list (one way or another, we are done with it...)
        _doneList.Add(_currentNode);
        //_currentNode.info = "";

        //is this our node? yay done...
        if (_currentNode == _endNode)
        {
            generatePath();
            _done = true;
        }
        else
        {

            //get all children and process them
            for (int i = 0; i < _currentNode.GetConnectionCount(); i++)
            {
                NodeTest connectedNode = _currentNode.GetConnectionAt(i);
                /**/ //A* Fixed

                //float newCostCurrent = _currentNode.costCurrent + _currentNode.position.DistanceTo(connectedNode.position);
                //float newCostEstimate = connectedNode.position.DistanceTo(_endNode.position);

                float newCostCurrent = _currentNode.costCurrent + (_currentNode.position - connectedNode.position).magnitude;
                float newCostEstimate = (connectedNode.position - _endNode.position).magnitude;

                /**/

                if ((_doneList.IndexOf(connectedNode) == -1 && _todoList.IndexOf(connectedNode) == -1)/**/ || connectedNode.costCurrent > newCostCurrent/**/)
                {
                    connectedNode.parentNode = _currentNode;

                    //strategy:A* Fixed
                    /**/
                    connectedNode.costCurrent = newCostCurrent;
                    connectedNode.costEstimate = newCostEstimate;
                    _todoList.Add(connectedNode);
                    /**/

                }
            }

            _todoList.Sort();//Used o.a. for Dijkstra, but is slow. needs optimization.
        }

        return _done;
    }

    public bool IsDone()
    {
        return _done;
    }

    public List<NodeTest> GetLastFoundPath()
    {
        return _lastPathFound;
    }

    private void generatePath()
    {
        _lastPathFound = new List<NodeTest>();

        NodeTest node = _endNode;

        while (node != null)
        {
            _lastPathFound.Add(node);

            node = node.parentNode;
        }

        _lastPathFound.Reverse();
    }

    private void resetPathFinder()
    {
        if (_todoList != null) _todoList.ForEach(resetNode);
        if (_doneList != null) _doneList.ForEach(resetNode);

        _todoList = new List<NodeTest>();
        _doneList = new List<NodeTest>();
        _done = false;
        _lastPathFound = null;
        _currentNode = null;

        //setup for next path
        if (_startNode != null)
        {
            _todoList.Add(_startNode);
            _startNode.costCurrent = _startNode.costEstimate = 0;
        }
    }

    private void resetNode(NodeTest pNode)
    {
        if (pNode.parentNode != null)
        {
            pNode.parentNode = null;
        }
    }
}


