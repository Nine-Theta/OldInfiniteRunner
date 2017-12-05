using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
    private NodeGraph _graph;
    private SearchGraph _search;

    private List<Node> _path = new List<Node>();

    private Vector2 _mapOffset;

    private float _tileSize = 1;
    private int _colums = 0;

    private void Start()
    {
        _graph = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().GetNodeGraph();

        _search = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().GetSearchGraph();

        _mapOffset = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().mapOffset;

        _tileSize = _graph.GetTileSize();
        _colums = GraphGenerator.GeneratorObject().GetComponent<GraphGenerator>().mapWidth;
    }

    public bool CanFindPath(Vector3 pPosition, Vector3 pTarget, int pMaxIterations)
    {
        return CanFindPath (new Vector2(pPosition.x, pPosition.y), new Vector2(pTarget.x, pTarget.y), pMaxIterations);
    }

    public bool CanFindPath(Vector2 pPosition, Vector2 pTarget, int pMaxIterations)
    {
        _path = null;
        int start = (int)(_colums * ((pPosition.x / _tileSize) - _mapOffset.x) + (pPosition.y / _tileSize) - _mapOffset.y);
        //Debug.Log("pTarget: "+ pTarget);
        //Debug.Log("start: " + start + " startpos: " + _graph.nodes[start].Position);
        //Debug.Log((int)pTarget.y/_tileSize);
        int end = (int)(_colums * (((int)pTarget.x / _tileSize) - _mapOffset.x) + ((int)pTarget.y / _tileSize) - _mapOffset.y);
        //Debug.Log("end: " + end + " endpos: " + _graph.nodes[end].Position);

        //Debug.Log("length: " + _graph.nodes.Length);
        _search.Start(_graph.nodes[start], _graph.nodes[end]);

        while(!_search.IsDone() || _search.GetIteration() <= pMaxIterations)
        {
            _search.Step();
        }

        if (_search.IsDone())
        {
            _path = _search.GetLastFoundPath();
            _search.ResetPathFinder();
            return true;
        }
        else
        {
            _search.ResetPathFinder();
            return false;
        }

        Debug.Log("Search done. Path lenth: " + _search.GetLastFoundPath().Count);
    }

    public List<Node> GetPath()
    {
        return _path;
    }

    public Vector2 GetMapOffset()
    {
        return _mapOffset;
    }
}
