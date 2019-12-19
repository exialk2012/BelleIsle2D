using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Classes
{
    public class MapBackGround:NodeInfo
    {
        public MapBackGround(int id, int count, int minX, int maxX, int minY, int maxY)
        {
            Id = id;
            Count = count;
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public int Id { get; set; }
        public int Count { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

    }
}

