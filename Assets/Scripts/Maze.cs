using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;



public class Maze : MonoBehaviour
{


    [SerializeField]
    GameObject cellPrefab;
    [SerializeField]
    GameObject walkerPrefab;
    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int seed;


    static Maze _instance;
    public static Random Random;
    Cell[,] cells;

    public static bool TryGetValidCell(Vector2Int index, out Cell cell)
    {
        if (index.x >= 0 && index.x < _instance.size.x && index.y >= 0 && index.y < _instance.size.y)
        {
            cell = _instance.cells[index.x, index.y];
            return true;
        }

        cell = null;
        return false;
    }

    public static Cell GetCellAtIndex(Vector2Int index)
    {
        return _instance.cells[index.x, index.y];
    }

    public static Vector2Int[] Directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

    public static void ClearCells()
    {
        foreach (Cell cell in _instance.cells)
        {
            cell.WasVisited = false;
        }
    }

    void Start()
    {
        Random = new Random(seed);

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }

        cells = new Cell[size.x, size.y];


        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                cells[x, y] = Instantiate(cellPrefab).GetComponent<Cell>();
                cells[x, y].transform.SetParent(transform);
                cells[x, y].transform.position = new Vector3(x, 0f, y);
                cells[x, y].gameObject.name = "Cell: " + x + " " + y;
            }
        }

        cells[0, 0].RemoveWall(Direction.Left);
        cells[size.x - 1, size.y - 1].RemoveWall(Direction.Right);


        MazeGenerator.GenerateMaze(new Vector2Int(0, 0));

        Transform walker = Instantiate(walkerPrefab).transform;

        walker.position = new Vector3(0f, walker.position.y, 0f);
    }
}
