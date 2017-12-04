using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
    private NodeGraph _graph;
    private SearchGraph _search;

    private List<Node> _path = new List<Node>();

    private Vector2 _mapOffset;

    private int _tileSize = 1;
    private int _colums = 0;

    private void Start()
    {
        _graph = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().GetNodeGraph();

        _search = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().GetSearchGraph();

        _mapOffset = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().mapOffset;

        _tileSize = _graph.GetTileSize();
        _colums = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().mapWidth;
    }

    public void FindPath(Vector2 pPosition, Vector2 pTarget)
    {
        int start = (int)(_colums * ((pPosition.x / _tileSize) - _mapOffset.x) + (pPosition.y / _tileSize) - _mapOffset.y);
        int end = (int)(_colums * ((pTarget.x / _tileSize) - _mapOffset.x) + (pTarget.y / _tileSize) - _mapOffset.y);

        _search.Start(_graph.nodes[start], _graph.nodes[end]);

        while (!_search.IsDone())
        {
            _search.Step();
        }
        if (_search.IsDone())
        {
            _path = _search.GetLastFoundPath();
        }

        Debug.Log("Search done. Path lenth: " + _search.GetLastFoundPath().Count + " iterations: ");
    }

    public List<Node> GetPath()
    {
        return _path;
    }
}
