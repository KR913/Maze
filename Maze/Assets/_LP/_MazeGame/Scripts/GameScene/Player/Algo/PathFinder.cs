using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [RequireComponent(typeof(ColliderChecker))]
    public class PathFinder : MonoBehaviour
    {
        Vector3[] _movements = new Vector3[4] { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

        [SerializeField] private Vector3 _startPath;
        [SerializeField] private Vector3 _endPath;
        ColliderChecker _bdy;

        private void Start()
        {
            if(Camera.main.aspect > 1)
            {
                _movements = new Vector3[4] { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
            }
            _bdy = GetComponent<ColliderChecker>();
        }

        public List<Vector3> PathFinding(Vector3 start, Vector3 goal)
        {
            _startPath = start;
            _endPath = goal;
            if (_bdy.CheckCollideAt(goal))
            {
                Debug.Log("No way found");
                return new List<Vector3>();
            }
            List<PathCell> priorQueue = new List<PathCell>();
            List<Vector3> visitedCell = new List<Vector3>();

            visitedCell.Add(_startPath);
            priorQueue.Add(new PathCell(_startPath, _endPath, -1));

            while (priorQueue.Count > 0)
            {
                PathCell current = new PathCell(priorQueue[0]);
                priorQueue.RemoveAt(0);

                //Check possible movement
                for (int i = 0; i<_movements.Length; i++)
                {
                    var nextpos = current.Position + _movements[i];
                    //check if cell visited
                    bool visited = false;
                    for (int j = 0; j < visitedCell.Count; j++)
                    {
                        if (Vector3.Distance(nextpos, visitedCell[j]) < 0.5f)
                        {
                            visited = true;
                            break;
                        }
                    }

                    //calculated if not visited
                    if (!visited)
                    {
                        //No shorter way to current position => current position is visited
                        if (!visitedCell.Contains(current.Position))
                        {
                            visitedCell.Add(current.Position);
                        }
                        //if wall then skip
                        if (_bdy.CheckCollideAt(nextpos))
                        {
                            continue;
                        }
                        //goal visited? yes then end
                        if (visitedCell.Contains(_endPath))
                        {
                            current.Next.Reverse();
                            return current.Next;
                        }

                        //if can move: add new cell
                        PathCell next = new PathCell(nextpos, _endPath, i);
                        next.Next.AddRange(current.Next);

                        //Heuretics
                        next.Turn = current.Turn;
                        if(current.Dir >= 0)
                        {
                            if (current.Dir != next.Dir)
                            {
                                next.Turn += 1;
                            }
                        }
                        next.Value = next.Hueristic();

                        //Add sort
                        priorQueue.Add(next);
                        int pos = priorQueue.Count - 1;
                        while (pos > 0)
                        {
                            if (next.Value > priorQueue[pos - 1].Value)
                            {
                                break;
                            }
                            priorQueue[pos] = priorQueue[pos - 1];
                            pos--;
                        }
                        priorQueue[pos] = next;
                    }
                }
            }
            Debug.Log("No way found");
            return new List<Vector3>();
        }

        class PathCell
        {
            public int Turn = 0;
            public int Dir = 0;
            public Vector3 Position;
            public Vector3 Goal;
            public float Value;
            public List<Vector3> Next;

            public PathCell(Vector3 pos, Vector3 goal, int dir)
            {
                Dir = dir;
                Position = pos;
                Goal = goal;
                Value = Vector3.Distance(pos, goal);
                Next = new List<Vector3>();
                Next.Add(Position);
            }
            public PathCell(PathCell pc)
            {
                Position = pc.Position;
                Goal = pc.Goal;
                Value = pc.Value;
                Next = pc.Next;
                Turn = pc.Turn;
                Dir = pc.Dir;
            }

            public float Hueristic()
            {
                int distanceH = Mathf.Abs((int)(Goal.x - Position.x)) + Mathf.Abs((int)(Goal.y - Position.y));
                int stepH = Next.Count;
                return distanceH + stepH + Turn;
            }
        }
    }
}
