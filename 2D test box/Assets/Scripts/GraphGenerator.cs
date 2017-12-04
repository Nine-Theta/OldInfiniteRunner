using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public Vector2 mapOffset;
    public int mapWidth = 0;
    public int mapHeight = 0;
    public float tileSize = 0;

    public GameObject debugnode;
    
    private NodeGraph _graph;
    private SearchGraph _search;

    private static GameObject _singletonInstance;

    private void Awake()
    {
        if (_singletonInstance != null && _singletonInstance != this.gameObject)
            Destroy(gameObject);
        else
            _singletonInstance = this.gameObject;

        int[,] map = new int[mapHeight, mapWidth];

        for (int row = 0; row < mapHeight; row++)
        {
            for (int col = 0; col < mapWidth; col++)
            {
                Vector2 point = new Vector2((row + mapOffset.x) * tileSize, (col + mapOffset.y) * tileSize);
                Collider2D hitCollider = Physics2D.OverlapPoint(point);
                if (hitCollider != null && hitCollider.CompareTag("Terrain")) map[row, col] = 1;
                else map[row, col] = 0;

                GameObject newNode = Instantiate(debugnode, new Vector3((row + mapOffset.x) * tileSize, (col + mapOffset.y) * tileSize, -10), Quaternion.Euler(0,0,0));
                if (map[row, col] == 1) newNode.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        _graph = new NodeGraph(map, tileSize);
        Debug.Log("map length: " + map.Length);
        _search = new SearchGraph(_graph);
    }

    private void Start()
    {
        
    }

    public NodeGraph GetNodeGraph()
    {
        return _graph;
    }

    public SearchGraph GetSearchGraph()
    {
        return _search;
    }

    public static GameObject GeneratorObject()
    {
        return _singletonInstance;
    }
}
