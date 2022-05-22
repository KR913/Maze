using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public interface IDirectionGetter
    {
        public void UpdateDirection(Vector2 dir);
    }
}
