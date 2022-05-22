using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
    public interface IPathGetter
    {
        public void UpdatePath(Vector3 pos);
    }
}
