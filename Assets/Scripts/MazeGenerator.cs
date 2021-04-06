using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    

    public static void GenerateMaze(Vector2Int start)
    {
        Maze.ClearCells();

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(start);
        Cell cell;

        Maze.TryGetValidCell(start, out cell);
        cell.WasVisited = true;

        while(stack.Count != 0)
        {
            if(HasOpenNeighbours(stack.Peek()))
            {
                int randomDirection = Maze.Random.Next(0, Maze.Directions.Length - 1);

                for(int i = 0; i < Maze.Directions.Length; i++)
                {
                    if(Maze.TryGetValidCell(stack.Peek() + Maze.Directions[(i + randomDirection) % Maze.Directions.Length], out cell) && !cell.WasVisited)
                    {
                        Maze.GetCellAtIndex(stack.Peek()).RemoveWall(Maze.Directions[(i + randomDirection) % Maze.Directions.Length]);

                        stack.Push(stack.Peek() + Maze.Directions[(i + randomDirection) % Maze.Directions.Length]);
                        
                        Maze.GetCellAtIndex(stack.Peek()).RemoveWall(Maze.Directions[(i + randomDirection) % Maze.Directions.Length] * -1);
                        Maze.GetCellAtIndex(stack.Peek()).WasVisited = true;

                        break;
                    }
                }
            }
            else
            {
                stack.Pop();
            }
        }

    }


    static bool HasOpenNeighbours(Vector2Int position)
    {
        Cell cell;

        for(int i = 0; i < Maze.Directions.Length; i++)
        {
            if(Maze.TryGetValidCell(position + Maze.Directions[i],out cell) && !cell.WasVisited)
            {
                return true;
            }
        }

        return false;
    }
}
