using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazeGame
{
    public class MazeManager : MonoBehaviour
    {
        [SerializeField] private DynamicScrollView _scoreText;
        private int _score = 0;

        private static MazeManager _instance;

        public static MazeManager Instance
        {
            get
            {
                return _instance;
            }
        }


        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this.gameObject.GetComponent<MazeManager>();
            }
        }


        public void Score(Sprite ic)
        {
            _score += 1;
            _scoreText?.AddScore(ic);
        }
    }
}
