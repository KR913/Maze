using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    [RequireComponent(typeof(ColliderChecker))]
    public class FrameMove3D : MonoBehaviour
    {
        [SerializeField] private KeyCode _moveK;
        [SerializeField] private KeyCode _leftK;
        [SerializeField] private KeyCode _rightK;

        private ColliderChecker _body;
        private Vector3 _dest;
        [SerializeField] private float _dest_ang;
        [SerializeField] private float _cur_ang;

        [SerializeField] private float _spd = 0;
        [SerializeField] private float _rotate_spd = 0;

        [SerializeField] GameObject _avatar;
        void Start()
        {
            _dest = transform.position;
            _cur_ang = _dest_ang = transform.rotation.eulerAngles.z;

            _body = GetComponent<ColliderChecker>();
        }

        void Update()
        {
            var dir = (_dest - transform.position).normalized;
            if ((_dest - transform.position).magnitude > _spd * Time.deltaTime)
            {
                transform.position += _spd * Time.deltaTime * dir;
            }
            else
            {
                transform.position = _dest;
            }

            var dir_ang = Mathf.Sign(_dest_ang - _cur_ang);
            if(Mathf.Abs(_dest_ang - _cur_ang) > _rotate_spd * Time.deltaTime)
            {
                _cur_ang += dir_ang * _rotate_spd * Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, 0, _cur_ang);
            }
            else
            {
                _cur_ang = _dest_ang;
                if(_cur_ang >= 360)
                {
                    _cur_ang = _dest_ang = _dest_ang - 360;
                }
                if (_cur_ang <= -360)
                {
                    _cur_ang = _dest_ang = _dest_ang + 360;
                }
                transform.rotation = Quaternion.Euler(0, 0, _cur_ang);
            }
            GetInput();
        }

        void GetInput()
        {
            if (_dest == transform.position && _dest_ang == _cur_ang)
            {
                if (Input.GetKey(_moveK))
                {
                    var temp = transform.position + (Quaternion.Euler(0,0,_dest_ang) * Vector3.right);
                    if (!_body.CheckCollideAt(temp))
                    {
                        _dest = temp;
                    }
                }
                else if (Input.GetKey(_leftK))
                {
                    _dest_ang -= 90;
                }
                else if (Input.GetKey(_rightK))
                {
                    _dest_ang += 90;
                }
            }
        }
    }
}
