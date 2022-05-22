using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [RequireComponent(typeof(IDirectionGetter))]
    public class DirectionInput : MonoBehaviour
    {
        [SerializeField] private float _deltaTouch;
        [SerializeField] private float _deltaTime;

        private IDirectionGetter _movement;

        private Vector2 _lastPos;
        private Vector2 _direction = new Vector2(0,0);

        private float _idleTime = 0;
        // Start is called before the first frame update
        void Start()
        {
            _movement = GetComponent<IDirectionGetter>();
        }

        void Update()
        {
#if UNITY_EDITOR
            //phase 0
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _lastPos = Input.mousePosition;
            }

            //phase 1
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector2 temp = Input.mousePosition;
                Vector2 dis = temp - _lastPos;
                Vector2 dir = new Vector2(0, 0);

                //handle new direction
                if (dis.magnitude > 0)
                {
                    if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
                    {
                        dir = new Vector2(Mathf.Sign(dis.x), 0);
                    }
                    else
                    {
                        dir = new Vector2(0, Mathf.Sign(dis.y));
                    }
                }

                //handle movement
                if (dis.magnitude >= _deltaTouch)
                {
                    _lastPos = temp;
                    _direction = dir;
                }
                else if (dir == _direction)
                {
                    _lastPos = temp;
                }
                else
                {
                    _idleTime += Time.deltaTime;
                    if (_idleTime >= _deltaTime)
                    {
                        _idleTime = 0;
                        _lastPos = temp;
                    }
                }
                _movement.UpdateDirection(_direction);
            }

            //phase 2
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _direction = new Vector2(0, 0);
            }

#else
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _lastPos = touch.position;
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        Vector2 temp = touch.position;
                        Vector2 dis = temp - _lastPos;
                        Vector2 dir = new Vector2(0, 0);

                        //handle new direction
                        if (dis.magnitude > 0)
                        {
                            if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
                            {
                                dir = new Vector2(Mathf.Sign(dis.x), 0);
                            }
                            else
                            {
                                dir = new Vector2(0, Mathf.Sign(dis.y));
                            }
                        }

                        //handle movement
                        if (dis.magnitude >= _deltaTouch)
                        {
                            _lastPos = temp;
                            _direction = dir;
                        }
                        else if (dir == _direction)
                        {
                            _lastPos = temp;
                        }
                        else
                        {
                            _idleTime += Time.deltaTime;
                            if (_idleTime >= _deltaTime)
                            {
                                _idleTime = 0;
                                _lastPos = temp;
                            }
                        }
                        _movement.UpdateDirection(_direction);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                    default:
                        _direction = new Vector2(0, 0);
                        break;
                }
            }
#endif
        }
    }
}
