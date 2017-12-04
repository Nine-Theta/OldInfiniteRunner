using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
    public int mapWidth = 0;
    public int mapHeight = 0;
    public int tileSize = 0;

    public GameObject debugnode;

    private void Start()
    {
        int[,] map = new int[mapWidth, mapHeight];

        for (int row = 0; row < mapHeight; row++)
        {
            for (int col = 0; col < mapWidth; col++)
            {
                Vector2 point = new Vector2(row * tileSize, col * tileSize);
                Collider2D hitCollider = Physics2D.OverlapPoint(point);
                if (hitCollider != null && hitCollider.CompareTag("Terrain")) map[row, col] = 1;
                else map[row, col] = 0;

                GameObject newNode = Instantiate(debugnode, new Vector3(row * tileSize, col * tileSize, -10), Quaternion.Euler(0,0,0));
                if (map[row, col] == 1) newNode.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        NodeGraph graph = new NodeGraph(map, tileSize);
        SearchGraph search = new SearchGraph(graph);
        search.Start(graph.nodes[0], graph.nodes[7]);

        while (!search.IsDone())
        {
            search.Step();
        }
        if (search.IsDone())
        {
            string t = ", ";
            for(int i = 0; i < search.GetLastFoundPath().Count; i++)
            {
                t += search.GetLastFoundPath()[i].Label + ", ";
            }

            Debug.Log(t);
        }

        Debug.Log("Search done. Path lenth: " + search.GetLastFoundPath().Count + " iterations: " + search.iterations);
    }
}
