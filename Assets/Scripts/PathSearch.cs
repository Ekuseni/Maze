using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSearch
{
    public static bool FindPath(Vector2Int start, Vector2Int end, out Vector2Int[] path)
    {
        Maze.ClearCells();
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Maze.GetCellAtIndex(start).WasVisited = true;

        stack.Push(start);
            

        while(stack.Count != 0)
        {

            if(stack.Peek() != end)
            {
                Vector2Int directionToBestCell = DirectionToBestCell(stack.Peek(), end);

                if (directionToBestCell != Vector2Int.zero)
                {
                    stack.Push(stack.Peek() + directionToBestCell);
                    //Debug.Log("Going to: " + stack.Peek());
                    Maze.GetCellAtIndex(stack.Peek()).WasVisited = true;
                }
                else
                {
                    stack.Pop();
                    //Debug.Log("Returning to: " + stack.Peek());
                }
            }
            else
            {
                path = stack.ToArray();
                return true;
            }
            
        }

        path = null;
        return false;
    }


    static Vector2Int DirectionToBestCell(Vector2Int current, Vector2Int end)
    {
        int currentBest = int.MaxValue;
        Vector2Int returnVector = Vector2Int.zero;
        Cell cell;

        for(int i = 0; i < Maze.Directions.Length; i++)
        {
            if(Maze.TryGetValidCell(current + Maze.Directions[i], out cell) && !cell.WasVisited && cell.DirectionClear(Maze.Directions[i] * -1) && Distance(current + Maze.Directions[i], end) < currentBest)
            {
                //Debug.Log("Estimating node: " + (current + Maze.Directions[i]));

                returnVector = Maze.Directions[i];
                currentBest = Distance(current + Maze.Directions[i], end);
            }
        }


        return returnVector;
    }

    static int Distance(Vector2Int start, Vector2Int end)
    {
        Vector2Int distance = end - start;

        return Mathf.Abs(distance.x) + Mathf.Abs(distance.y);
    }

}
