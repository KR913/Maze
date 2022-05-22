using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [RequireComponent(typeof(ColliderChecker))]
    [RequireComponent(typeof(PathFinder))]
    public class FrameMovement : MonoBehaviour, IPathGetter
    {
        private ColliderChecker _body;
        private PathFinder _mind;
        private Vector3 _dest;
        private Vector3 _preDest;
        [SerializeField] private float _spd = 0;
        public List<Vector3> _path = new List<Vector3>();

        [SerializeField] private GameObject _avatar;
        void Start()
        {
            _preDest = _dest = transform.position;

            _body = GetComponent<ColliderChecker>();
            _mind = GetComponent<PathFinder>();
        }

        void Update()
        {
            if (_body.CheckCollideHere())
            {
                _dest = _preDest;
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
                        _path.Clear();
                    }
                }
            }
        }

        public void UpdatePath(Vector3 pos)
        {
            var temp = _mind.PathFinding(_dest, pos);
            if (temp.Count > 0)
            {
                _path = temp;
                _path.RemoveAt(0);
            }
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
