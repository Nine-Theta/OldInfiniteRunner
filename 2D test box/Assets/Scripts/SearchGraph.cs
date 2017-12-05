using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchGraph
{
    private NodeGraph _graph;
    private List<Node> _todoList;
    private List<Node> _doneList;
    private List<Node> _path;
    private List<Node> _lastPathFound = null;

    private Node _startNode;
    private Node _endNode;
    private Node _currentNode;

    private int iterations = 0;
    private bool _done = false;

    public SearchGraph(NodeGraph pGraph)
    {
        _graph = pGraph;
    }

    public void Start(Node pStartNode, Node pEndNode)
    {
        _todoList = new List<Node>();
        _todoList.Add(pStartNode);

        _startNode = pStartNode;
        _endNode = pEndNode;

        _doneList = new List<Node>();
        _path = new List<Node>();

        for (int i = 0; i < _graph.nodes.Length; i++)
        {
            if (_graph.nodes[i] == null) continue;
            _graph.nodes[i].ResetNode();
        }
    }
    /* public void Step()
    {
        if (path.Count > 0) return;

        if (reachable.Count == 0)
        {
            finished = true;
            return;
        }

        iterations++;

        Node node = ChoseNode();
        if(node == endNode)
        {
            while (node != null)
            {
                path.Insert(0, node);
                node = node.Parent;
            }
            finished = true;
            return;
        }
        
        explored.Add(node);
        reachable.Remove(node);

        for (int i = 0; i < node.GetConnectionCount(); i++)
        {
            AddConnection(node, node.GetConnectionAt(i));
        }
    }
    */

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

        //Debug.Log("conncount: " + _todoList[0].GetConnectionCount());
        //Debug.Log("conneclabel: " + _todoList[0].GetConnectionAt(0).Label);

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
                Node connectedNode = _currentNode.GetConnectionAt(i);
                /**/ //A* Fixed

                float newCostCurrent = _currentNode.costCurrent + (_currentNode.Position - connectedNode.Position).magnitude;
                float newCostEstimate = (connectedNode.Position - _endNode.Position).magnitude;

                /**/

                if ((_doneList.IndexOf(connectedNode) == -1 && _todoList.IndexOf(connectedNode) == -1)/**/ || connectedNode.costCurrent > newCostCurrent/**/)
                {
                    connectedNode.Parent = _currentNode;

                    //strategy:A* Fixed
                    /**/
                    connectedNode.costCurrent = newCostCurrent;
                    connectedNode.costEstimate = newCostEstimate;
                    _todoList.Add(connectedNode);
                    /**/
                }
            }

            _todoList.Sort();
            iterations++;
        }

        return _done;
    }

    public bool IsDone()
    {
        return _done;
    }

    public int GetIteration()
    {
        return iterations;
    }

    public List<Node> GetLastFoundPath()
    {
        return _lastPathFound;
    }

    public void GeneratePartialPath()
    {
        _lastPathFound = new List<Node>();
        Node node = _currentNode;

        while (node != null)
        {
            _lastPathFound.Add(node);
            node = node.Parent;
        }
        _done = true;
    }

    private void generatePath()
    {
        _lastPathFound = new List<Node>();
        Node node = _endNode;

        while (node != null)
        {
            _lastPathFound.Add(node);
            node = node.Parent;
        }

        //_lastPathFound.Reverse();
    }

    public void ResetPathFinder()
    {
        if (_todoList != null) _todoList.ForEach(resetNode);
        if (_doneList != null) _doneList.ForEach(resetNode);

        _todoList = new List<Node>();
        _doneList = new List<Node>();
        _done = false;
        _lastPathFound = null;
        _currentNode = null;
        iterations = 0;

        //setup for next path
        if (_startNode != null)
        {
            _todoList.Add(_startNode);
            _startNode.costCurrent = _startNode.costEstimate = 0;
        }
    }

    private void resetNode(Node pNode)
    {
        if (pNode.Parent != null)
            pNode.Parent = null;
    }

    /*public void AddConnection(Node pNode, Node pConnection)
    {
        if (FindNode(pConnection, _doneList) || FindNode(pConnection, _todoList))
            return;

        pConnection.Parent = pNode;
        _todoList.Add(pConnection);
    }*/

    /*public bool FindNode(Node pNode, List<Node> pList)
    {
        return GetNodeIndex(pNode, pList) >= 0;
    }*/

    /*public int GetNodeIndex(Node pNode, List<Node> pList)
    {
        for (int i = 0; i < pList.Count; i++)
        {
            if (pNode == pList[i])
                return i;
        }
        return -1;
    }*/
}
