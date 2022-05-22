using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class MazeUtil
    {
        #region Movement
        public delegate void Move(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable);
        public static Move[] Movement = { MoveUp, MoveDown, MoveRight, MoveLeft };
        public static void MoveUp(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable)
        {
            if (pos.y >= map.GetLength(1) - 2)
            {
                //Add unmoveable
                return;
            }
            Vector2Int nextStep = pos + new Vector2Int(0, 2);
            if (visitedCells.Contains(nextStep))
            {
                //Add unmoveable
                return;
            }
            visitedCells.Add(nextStep);
            map[pos.x, pos.y + 1] = false;
            map[pos.x, pos.y + 2] = false;
            pos = nextStep;
        }
        public static void MoveDown(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable)
        {
            if (pos.y <= 1)
            {
                //Add unmoveable
                return;
            }
            Vector2Int nextStep = pos + new Vector2Int(0, -2);
            if (visitedCells.Contains(nextStep))
            {
                //Add unmoveable
                return;
            }
            visitedCells.Add(nextStep);
            map[pos.x, pos.y - 1] = false;
            map[pos.x, pos.y - 2] = false;
            pos = nextStep;
        }
        public static void MoveRight(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable)
        {
            if (pos.x >= map.GetLength(0) - 2)
            {
                //Add unmoveable
                return;
            }
            Vector2Int nextStep = pos + new Vector2Int(2, 0);
            if (visitedCells.Contains(nextStep))
            {
                //Add unmoveable
                return;
            }
            visitedCells.Add(nextStep);
            map[pos.x + 1, pos.y] = false;
            map[pos.x + 2, pos.y] = false;
            pos = nextStep;
        }
        public static void MoveLeft(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable)
        {
            if (pos.x <= 1)
            {
                //Add unmoveable
                return;
            }
            Vector2Int nextStep = pos + new Vector2Int(-2, 0);
            if (visitedCells.Contains(nextStep))
            {
                //Add unmoveable
                return;
            }
            visitedCells.Add(nextStep);
            map[pos.x - 1, pos.y] = false;
            map[pos.x - 2, pos.y] = false;
            pos = nextStep;
        }
        #endregion

        public static void CheckPos(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable)
        {
            moveable.Clear();
            Vector2Int[] nextPos = {
            pos + new Vector2Int(0, 2),
            pos + new Vector2Int(0, -2),
            pos + new Vector2Int(2, 0),
            pos + new Vector2Int(-2, 0),
        };
            for (int i = 0; i < nextPos.Length; i++)
            {
                var cur = nextPos[i];
                if ((!visitedCells.Contains(cur)) && cur.x < map.GetLength(0) && cur.y < map.GetLength(1)
                    && cur.x > 0 && cur.y > 0)
                {
                    moveable.Add(i);
                }
            }
        }

        public static void Retract(ref bool[,] map, ref Vector2Int pos, ref List<Vector2Int> visitedCells, ref List<int> moveable)
        {
            var tmp = visitedCells[visitedCells.Count - 1];
            visitedCells.Insert(0, tmp);
            visitedCells.RemoveAt(visitedCells.Count - 1);
            pos = visitedCells[visitedCells.Count - 1];
        }
    }
}
