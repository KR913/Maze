using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class GoalMark : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            gameObject.SetActive(false);
        }
    }
}
