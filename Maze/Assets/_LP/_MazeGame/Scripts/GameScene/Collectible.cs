using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] public Sprite Icon;
        [SerializeField] public string Desc;

        public void Collected()
        {
            MazeManager.Instance?.Score(Icon);
            Destroy(gameObject);
        }
    }
}