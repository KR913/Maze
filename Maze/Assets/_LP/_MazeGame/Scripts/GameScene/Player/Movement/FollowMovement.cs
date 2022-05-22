using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [RequireComponent(typeof(ColliderChecker))]
    public class FollowMovement : MonoBehaviour, IPathGetter, IDirectionGetter
    {
        private ColliderChecker _body;
        private Vector3 _dest;
        private Vector3 _preDest;
        [SerializeField] private float _spd = 0;
        public List<Vector3> _path = new List<Vector3>();

        [SerializeField] private GameObject _avatar;

        void Start()
        {
            _preDest = _dest = transform.position;

            _body = GetComponent<ColliderChecker>();
        }

        void Update()
        {
            if (_body.CheckCollideHere())
            {
                _dest = _preDest;
                _path.Clear();
            }
            var dir = (_dest - transform.position).normalized;
            if ((_dest - transform.position).magnitude > _spd * Time.deltaTime)
            {
                transform.position += _spd * Time.deltaTime * dir;
            }
            else
            {
                _preDest = transform.position = _dest;
            }
            GetInput();
            DrawPath();
        }

        void GetInput()
        {
            if (_path.Count > 0)
            {
                if (_dest == transform.position)
                {
                    var temp = _path[0];
                    _avatar.transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), temp - _dest);
                    if (!_body.CheckCollideAt(temp))
                    {
                        _dest = temp;
                        _path.RemoveAt(0);
                    }
                    else
                    {
                        Debug.Log(temp);
                        _path.Clear();
                    }
                }
            }
        }

        public void UpdatePath(Vector3 pos)
        {
            var curgoal = _dest;
            if (_path.Count > 0)
            {
                curgoal = _path[_path.Count - 1];
            }

            if(pos == curgoal)
            {
                return;
            }
            if (_body.CheckCollideAt(pos))
            {
                return;
            }
            //Logic update
            if (Mathf.Abs(pos.x - curgoal.x)<=1 && Mathf.Abs(pos.y - curgoal.y) <= 1)
            {
                if ((pos.x - curgoal.x) * (pos.y - curgoal.y) != 0)
                {
                    if(!_body.CheckCollideAt(curgoal + new Vector3(pos.x - curgoal.x, 0, 0)))
                    {
                        _path.Add(curgoal + new Vector3(pos.x - curgoal.x, 0, 0));
                        _path.Add(pos);
                    }
                    else if (!_body.CheckCollideAt(curgoal + new Vector3(0, pos.y - curgoal.y, 0)))
                    {
                        _path.Add(curgoal + new Vector3(0, pos.y - curgoal.y, 0));
                        _path.Add(pos);
                    }
                    return;
                }
                _path.Add(pos);
                return;
            }

            //optimize update
            if((pos.x - curgoal.x) * (pos.y - curgoal.y) != 0)
            {
                return;
            }
            var midgoal = curgoal;
            while (pos != midgoal)
            {
                midgoal += (pos - curgoal).normalized;
                if (_body.CheckCollideAt(midgoal))
                {
                    return;
                }
                _path.Add(midgoal);
                curgoal = _path[_path.Count - 1];
            }
            return;
        }


        public void UpdateDirection(Vector2 dir)
        {
            if (_path.Count > 0)
            {
                return;
            }
            if(_dest != transform.position)
            {
                return;
            }
            if(dir.magnitude == 0)
            {
                return;
            }
            Vector3 pos = _dest + new Vector3(dir.x, dir.y, 0);
            if (_body.CheckCollideAt(pos))
            {
                return;
            }
            _path.Add(pos);
        }


        private void DrawPath()
        {
            var lastPos = _dest;
            for (int i = 0; i < _path.Count; i++)
            {
                Debug.DrawLine(lastPos, _path[i], Color.green);

                GameObject go = new GameObject();
                go.AddComponent<LineRenderer>();
                LineRenderer lr = go.GetComponent<LineRenderer>();
                lr.startColor = Color.green;
                lr.endColor = Color.green;
                lr.startWidth = .1f;
                lr.endWidth = .1f;
                lr.SetPosition(0, lastPos);
                lr.SetPosition(1, _path[i]);
                GameObject.Destroy(go, Time.deltaTime * 2);

                lastPos = _path[i];
            }
        }
    }
}