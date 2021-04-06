using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Up,
    Right,
    Down,
    Left
}



public class Cell : MonoBehaviour
{

    [SerializeField]
    GameObject upWall;
    [SerializeField]
    GameObject rightWall;
    [SerializeField]
    GameObject downWall;
    [SerializeField]
    GameObject leftWall;


    Dictionary<Direction, GameObject> directionToGameObject;
    static Dictionary<Vector2Int, Direction> vectorToDirection = new Dictionary<Vector2Int, Direction>
    {
        {Vector2Int.up, Direction.Up},
        {Vector2Int.right, Direction.Right},
        {Vector2Int.down, Direction.Down},
        {Vector2Int.left, Direction.Left},
    };
    private void Awake()
    {
        directionToGameObject = new Dictionary<Direction, GameObject>();

        directionToGameObject.Add(Direction.Up, upWall);
        directionToGameObject.Add(Direction.Right, rightWall);
        directionToGameObject.Add(Direction.Down, downWall);
        directionToGameObject.Add(Direction.Left, leftWall);
    }

    public void RemoveWall(Direction direction)
    {
        directionToGameObject[direction].gameObject.SetActive(false);
    }

    public void RemoveWall(Vector2Int direction)
    {
        RemoveWall(vectorToDirection[direction]);
    }

    public bool DirectionClear(Vector2Int direction)
    {
        return !directionToGameObject[vectorToDirection[direction]].gameObject.activeInHierarchy;
    }

    public bool WasVisited = false;

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        Walker.Target = transform.position;
    }
}
