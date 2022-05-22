using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class MazeGenerator : MonoBehaviour
    {
        private bool[,] _Map;
        [SerializeField] GameObject _wall;
        [SerializeField] GameObject _maze;
        [SerializeField] int _x_size;
        [SerializeField] int _y_size;

        private bool _complete = false;
        private bool _complex = false;

        //Set up
        [SerializeField] Vector2Int curpos = new Vector2Int(1, 1);
        List<Vector2Int> visitedCells = new List<Vector2Int>();
        List<int> moveable = new List<int>();

        private void Start()
        {
            InitMap(_x_size, _y_size);
        }

        void MazeGenerate(int x_size, int y_size)
        {
            Vector2Int goal = new Vector2Int(x_size - 2, y_size - 2);

            //Find a way
            if (curpos != goal)
            {
                MazeUtil.CheckPos(ref _Map, ref curpos, ref visitedCells, ref moveable);
                if (moveable.Count <= 0)
                {
                    _complex = true;
                    MazeUtil.Retract(ref _Map, ref curpos, ref visitedCells, ref moveable);
                }
                else
                {
                    int move = Random.Range(0, moveable.Count);
                    MazeUtil.Movement[moveable[move]](ref _Map, ref curpos, ref visitedCells, ref moveable);
                    //moveable.RemoveAt(move);

                    //more complex
                    if (curpos == goal && !_complex)
                    {
                        visitedCells.RemoveAt(visitedCells.Count - 1);
                        curpos = visitedCells[visitedCells.Count - 1];
                        int randomRep = Random.Range(1, visitedCells.Count);
                        for (int i = 0; i < randomRep; i++)
                        {
                            MazeUtil.Retract(ref _Map, ref curpos, ref visitedCells, ref moveable);
                        }
                        _complex = true;
                    }
                }
            }
            else
            {
                _complete = true;
            }
        }

        void BuildMaze()
        {
            if(_maze.transform.childCount == 0)
            {
                for (int i = 0; i < _Map.Length; i++)
                {
                    int x = i % _Map.GetLength(0);
                    int y = i / _Map.GetLength(0);
                    if (_Map[x, y])
                    {
                        Instantiate(_wall, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0), _maze.transform);
                    }
                }
            }
            foreach (Transform child in _maze.transform)
            {
                if (!_Map[(int)child.position.x, (int)child.position.y])
                {
                    Destroy(child.gameObject);
                }
            }

        }

        void InitMap(int x_size, int y_size)
        {
            //Init map of desired size
            if (x_size % 2 == 0)
            {
                x_size++;
            }
            if (y_size % 2 == 0)
            {
                y_size++;
            }
            _Map = new bool[x_size, y_size];
            for (int i = 0; i < _Map.Length; i++)
            {
                int x = i % x_size;
                int y = i / x_size;
                //_Map[x, y] = (x % 2 == 0 || y % 2 == 0);
                //full wall
                _Map[x, y] = true;
                _Map[curpos.x, curpos.y] = false;
            }
            visitedCells.Add(curpos);
        }

        private void Update()
        {
            if (!_complete)
            {
                MazeGenerate(_x_size, _y_size);
                BuildMaze();
            }
        }
    }
}
