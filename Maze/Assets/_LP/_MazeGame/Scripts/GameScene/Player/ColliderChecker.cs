using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MazeGame
{
    public class ColliderChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitLayer;

        private Collider2D _shape;

        private void Start()
        {
            _shape = GetComponent<Collider2D>();
        }

        public bool CheckCollideAt(Vector3 dest)
        {
            if (_shape != null)
            {
                var castH = Physics2D.BoxCast(dest, _shape.bounds.size * .9f, 0, Vector2.down, 0, _hitLayer);
                return castH.collider != null && castH.collider!=_shape;
            }
            return false;
        }

        public bool CheckCollideHere()
        {
            if (_shape != null)
            {
                var castH = Physics2D.BoxCastAll(transform.position, _shape.bounds.size * .9f, 0, Vector2.down, 0, _hitLayer);
                return castH.Length > 1;
            }
            return false;
        }
    }
}
