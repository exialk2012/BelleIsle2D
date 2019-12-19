using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Script;

namespace Script.Components
{
    [System.Serializable]
    public struct FillNode
    {
        public string ForegroundId;
        public int beginX;
        public int beginY;
        public int endX;
        public int endY;
    }

    [System.Serializable]
    public struct NodeGroup
    {
        public string ForegroundGroupId;
        public int Percentage;
        public int beginX;
        public int beginY;
        public int endX;
        public int endY;
    }
    public class MapSettingComponent:MonoBehaviour
    {
        [SerializeField]
        private int _mapId;
        [SerializeField]
        private string _mapName;
        [SerializeField, Multiline]
        private string _mapDescription;
        [SerializeField]
        private int _mapType;
        [SerializeField]
        private int _mapWidth;
        [SerializeField]
        private int _mapHeight;
        [Header("最后填充的地块数据")]
        public List<FillNode> fill = new List<FillNode>();
        [SerializeField]
        private int _view;
        [SerializeField]
        private string _gateId;
        [SerializeField]
        private string _backGround;
        
        [Header("组合地块的填充数据")]
        public List<NodeGroup> nodeGroups = new List<NodeGroup>();
        [SerializeField]
        private string _powerOverBuffId;
        [SerializeField]
        private string _planeResListStr;
        [SerializeField]
        private int _keyCount;

        

        public int MapId
        {
            get
            {
                return _mapId;
            }

            set
            {
                _mapId = value;
            }
        }

        public int MapWidth
        {
            get
            {
                return _mapWidth;
            }

            set
            {
                _mapWidth = value;
            }
        }

        public int MapHeight
        {
            get
            {
                return _mapHeight;
            }

            set
            {
                _mapHeight = value;
            }
        }

        public string MapName
        {
            get
            {
                return _mapName;
            }

            set
            {
                _mapName = value;
            }
        }

        public string MapDescription
        {
            get
            {
                return _mapDescription;
            }

            set
            {
                _mapDescription = value;
            }
        }

        public int View
        {
            get
            {
                return _view;
            }

            set
            {
                _view = value;
            }
        }

        public string GateId
        {
            get
            {
                return _gateId;
            }

            set
            {
                _gateId = value;
            }
        }

        public string BackGround
        {
            get
            {
                return _backGround;
            }

            set
            {
                _backGround = value;
            }
        }

        public string PowerOverBuffId
        {
            get
            {
                return _powerOverBuffId;
            }

            set
            {
                _powerOverBuffId = value;
            }
        }

        public string PlaneResListStr
        {
            get
            {
                return _planeResListStr;
            }

            set
            {
                _planeResListStr = value;
            }
        }

        public int KeyCount
        {
            get
            {
                return _keyCount;
            }

            set
            {
                _keyCount = value;
            }
        }

        public int MapType
        {
            get
            {
                return _mapType;
            }

            set
            {
                _mapType = value;
            }
        }

        
    }

    [CustomEditor(typeof(MapSettingComponent))]
    public class MapSettingButton:Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var mapSetting = GameObject.Find("MapSetting").GetComponent<MapSettingComponent>();
            if (GUILayout.Button("生成地图"))
            {   
                InitMapSetting.CreatMap(mapSetting);
            }
            if (GUILayout.Button("重置地图"))
            {
                if (GameObject.Find($"Map-{mapSetting.MapId}")==null)
                {
                    return;
                }
                GameObject.DestroyImmediate(GameObject.Find($"Map-{mapSetting.MapId}"));
                InitMapSetting.CreatMap(mapSetting);
            }

            if (GUILayout.Button("生成数据"))
            {
                InitMapSetting.GenerateMapDate(mapSetting);
            }

            if (GUILayout.Button("测试用"))
            {
                InitMapSetting.TestFunc(mapSetting);
            }
            if (GUILayout.Button("测试用2"))
            {
                InitMapSetting.TestFunc2(mapSetting);
            }
        }
    }
}

