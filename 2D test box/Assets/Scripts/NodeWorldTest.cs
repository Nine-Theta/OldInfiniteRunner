using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeWorldTest : MonoBehaviour {
    /*
    private List<Node> _nodes = null;

    private GameObject _linkLayer;  //layer to add all the visible connections to
    private GameObject _nodeLayer;  //layer to add all the visible nodes to

    public NodeWorldTest()
    {
        _nodes = new List<Node>();

        //two separate layers for visual debugging, so that all lines can be behind all nodes
        _linkLayer = new Pivot();
        AddChild(_linkLayer);
        _nodeLayer = new Pivot();
        AddChild(_nodeLayer);
    }

    ///////////////////////////////////// NODE ADDITIONS ////////////////////////////////////////////

    public void AddNode(Node node)
    {
        _nodeLayer.AddChild(node);
        _nodes.Add(node);
    }

    public int GetNodeCount()
    {
        return _nodes.Count;
    }

    public Node GetNodeAt(int index)
    {
        return _nodes[index];
    }

    public void AddConnection(Node nodeA, Node nodeB)
    {
        if (nodeA.HasConnection(nodeB)) return;

        //for visual debugging
        LineSegment visualLink = new LineSegment(nodeA.position, nodeB.position, (uint)Color.LightGray.ToArgb(), 10);
        _linkLayer.AddChild(visualLink);

        //pass in the visual link (change from previous implementation) so that each node knows which linesegment corresponds with which connection
        nodeA.AddConnection(nodeB, visualLink);
        nodeB.AddConnection(nodeA, visualLink);
    }

    ~NodeWorldTest()
    {
        _nodes.Clear();
        _nodes = null;
    }*/
}