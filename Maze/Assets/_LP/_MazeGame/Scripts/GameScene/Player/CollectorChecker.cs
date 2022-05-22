using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public class CollectorChecker : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Coin"))
            {
                Collectible coll = collision.GetComponent<Collectible>();
                if (coll != null)
                {
                    coll.Collected();
                }
            }
        }
    }
}
