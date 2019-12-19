using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Classes
{
    public class MapBoss:NodeInfo
    {
        public MapBoss(int bossForeId, int bossNodeId)
        {
            BossForeId = bossForeId;
            BossNodeId = bossNodeId;
        }

        public int BossForeId { get; set; }
        public int BossNodeId { get; set; }
    }

}
