using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Classes
{
    public class MapSpecialNode : NodeInfo
    {
        public MapSpecialNode(int foreGroundId, int positionX, int positionY)
        {
            ForeGroundId = foreGroundId;
            PositionX = positionX;
            PositionY = positionY;
        }

        public int ForeGroundId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}

