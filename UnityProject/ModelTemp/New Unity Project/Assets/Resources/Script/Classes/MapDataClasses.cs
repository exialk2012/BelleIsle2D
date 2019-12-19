using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Classes
{
    public class AdventureMapModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<AdventureBackgroundTile> BackgroundFill { get; set; }
        public List<AdventureForegroundRangeTile> Fill { get; set; }
        public int View { get; set; }
        public string GateId { get; set; }
        public List<AdventureForegroundRangeTile> StartFill { get; set; }
        public List<AdventureForegroundRangeTile> BossFill { get; set; }
        public List<AdventureForegroundTile> SpecialFill { get; set; }
        public List<AdventureForegroundGroupTile> GroupFill { get; set; }
        public List<AdventureForegroundRangeFillTile> RandomFill { get; set; }
        public string Background { get; set; }
        public string PowerOverBuffId { get; set; }
        public string PlaneResListStr { get; set; }
        public int KeyCount { get; set; }
    }
    /// <summary>
    /// 冒险地图底格类：底格填充用
    /// </summary>
    public class AdventureBackgroundTile
    {
        public string BackgroundId { get; set; }
        public int FillCount { get; set; }

        public AdventureGridPosition Begin { get; set; }
        public AdventureGridPosition End { get; set; }

    }

    /// <summary>
    /// 冒险地图地格范围类：初始点、Boss点，最后填充地格用
    /// </summary>
    public class AdventureForegroundRangeTile
    {
        public string ForegroundId { get; set; }
        public AdventureGridPosition Begin { get; set; }
        public AdventureGridPosition End { get; set; }
    }

    /// <summary>
    /// 固定格子类:Special地格用
    /// </summary>
    public class AdventureForegroundTile
    {
        public string ForegroundId { get; set; }
        public AdventureGridPosition Position { get; set; }
    }
    /// <summary>
    /// 地格组合类
    /// </summary>
    public class AdventureForegroundGroupTile
    {
        public string ForegroundGroupId { get; set; }
        public int  Percentage { get; set; }
        public AdventureGridPosition Begin { get; set; }
        public AdventureGridPosition End { get; set; }
    }
    /// <summary>
    /// 地格范围填充类：用于随机地块填充
    /// </summary>
    public class AdventureForegroundRangeFillTile
    {
        public string ForegroundId { get; set; }
        public int FillCount { get; set; }
        public AdventureGridPosition Begin { get; set; }
        public AdventureGridPosition End { get; set; }
    }

    /// <summary>
    /// 冒险地图格子位置类
    /// </summary>
    public class AdventureGridPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

