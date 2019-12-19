using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Classes
{
    public class MapStart:NodeInfo
    {
        public MapStart(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

    }

}
