using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph
{
    public Node[] nodes;

    private int _rows = 0;
    private int _colums = 0;
    private float _tileSize = 1;

    public NodeGraph(int[,] pGrid, float pTileSize)
    {
        _rows = pGrid.GetLength(0);
        _colums = pGrid.GetLength(1);
        _tileSize = pTileSize;

        nodes = new Node[pGrid.Length];

        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = new Node();
            node.Label = (i).ToString();
            nodes[i] = node;
        }

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _colums; col++)
            {
                int count = _colums * row + col;

                Node node = nodes[count];
                Debug.Log("nodes count: " + nodes.Length);
                node.Position = new Vector2(row * _tileSize, col * _tileSize);

                if (pGrid[row, col] == 1)
                {
                    node = null;
                    nodes[count] = null;
                    continue;
                }

                //up
                if (row > 0 && pGrid[row-1, col] == 0)
                {
                    node.AddConnection(nodes[_colums * (row - 1) + col]);
                }

                //right
                if (col < _colums - 1 && pGrid[row, col+1] == 0)
                {
                    node.AddConnection(nodes[count + 1]);
                }

                //down
                if (row < _rows - 1 && pGrid[row + 1, col] == 0)
                {
                    node.AddConnection(nodes[_colums * (row + 1) + col]);
                }

                //left
                if (col > 0 && pGrid[row, col-1] == 0)
                {
                    node.AddConnection(nodes[count - 1]);
                }

                //up-right
                if (row > 0 && col < _colums - 1 && pGrid[row - 1, col+1] == 0)
                {
                    node.AddConnection(nodes[_colums * (row - 1) + col+1]);
                }

                //down-right
                if (row < _rows - 1 && col < _colums - 1 && pGrid[row+1, col + 1] == 0)
                {
                    node.AddConnection(nodes[_colums * (row + 1) + col+1]);
                }

                //down-left
                if (col > 0 && row < _rows - 1 && pGrid[row + 1, col-1] == 0)
                {
                    node.AddConnection(nodes[_colums * (row + 1) + col-1]);
                }

                //up-left
                if (row > 0 && col > 0 && pGrid[row-1, col - 1] == 0)
                {
                    node.AddConnection(nodes[_colums * (row - 1) + col-1]);
                }
            }
        }
    }

    public float GetTileSize()
    {
       return _tileSize;
    }
}
