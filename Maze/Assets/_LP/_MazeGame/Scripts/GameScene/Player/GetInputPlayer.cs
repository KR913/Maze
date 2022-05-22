using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame {
    [RequireComponent(typeof(IPathGetter))]
    public class GetInputPlayer : MonoBehaviour
    {
        private IPathGetter _mover;
        [SerializeField] private Vector3 _targetTouch;
        [SerializeField] private GameObject _goal;
        private void Start()
        {
            _mover = GetComponent<IPathGetter>();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.Mouse0))
            {
                var tar = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _targetTouch = new Vector3(Mathf.RoundToInt(tar.x), Mathf.RoundToInt(tar.y), 0);
                _goal.transform.position = _targetTouch + new Vector3(0, 0, 1);
                _goal.SetActive(true);
                _mover.UpdatePath(_targetTouch);
            }
#else
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                var tar = Camera.main.ScreenToWorldPoint(touch.position);
                _targetTouch = new Vector3(Mathf.RoundToInt(tar.x), Mathf.RoundToInt(tar.y), 0);
                _goal.transform.position = _targetTouch + new Vector3(0, 0, 1);
                _goal.SetActive(true);
                _mover.UpdatePath(_targetTouch);
            }
#endif
        }
    }
}
