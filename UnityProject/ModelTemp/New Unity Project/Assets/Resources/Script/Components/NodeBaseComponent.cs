using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Script.Attribute;

namespace Script.Components
{
    [System.Serializable]
    struct NodePosition
    {
        public int X;
        public int Y;
    }
    
    public class NodeBaseComponent : MonoBehaviour
    {
        [Header("地图节点信息")]
        [SerializeField,ReadOnly]
        private int _nodeIndex;
        [SerializeField, ReadOnly]
        private NodePosition _nodePosition;
        [SerializeField, ReadOnly]
        private int _nodeColIndex;
        [SerializeField, ReadOnly]
        private int _nodeRowIndex;
        [SerializeField, ReadOnly]
        private string _nodeType = "Init";
        [Header("普通节点信息")]
        [SerializeField]
        private int _nodeArea = -1;
        [Header("初始节点信息")]
        [SerializeField]
        private int _startNodeId = 0;
        [Header("BOSS节点信息")]
        [SerializeField]
        private int _bossNodeId = 0;
        [Header("地图边界信息")]
        [SerializeField]
        private int borderNodeId = 0;

        public int NodeId
        {
            get
            {
                return _nodeIndex;
            }

            set
            {
                _nodeIndex = value;
            }
        }

        public int NodeColIndex
        {
            get
            {
                return _nodeColIndex;
            }

            set
            {
                _nodeColIndex = value;
            }
        }

        public int NodeRowIndex
        {
            get
            {
                return _nodeRowIndex;
            }

            set
            {
                _nodeRowIndex = value;
            }
        }

        public int NodeArea
        {
            get
            {
                return _nodeArea;
            }

            set
            {
                _nodeArea = value;
            }
        }

        public int StartNodeId
        {
            get
            {
                return _startNodeId;
            }

            set
            {
                _startNodeId = value;
            }
        }

        public int BossNodeId
        {
            get
            {
                return _bossNodeId;
            }

            set
            {
                _bossNodeId = value;
            }
        }

        public int BorderNodeId
        {
            get
            {
                return borderNodeId;
            }

            set
            {
                borderNodeId = value;
            }
        }

        public string NodeType
        {
            get
            {
                return _nodeType;
            }

            set
            {
                _nodeType = value;
            }
        }

        internal NodePosition NodePosition { get => _nodePosition; set => _nodePosition = value; }
    }

    [CustomEditor(typeof(NodeBaseComponent)),CanEditMultipleObjects]
    public class NodeBaseComponentButton:Editor
    {
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            if (GUILayout.Button("设置普通节点"))
            {
                Script.NodeTools.SetNode();
            }
            if (GUILayout.Button("设置无物件地格"))
            {
                Script.NodeTools.SetNoObjectGround();
            }
            if (GUILayout.Button("设置初始点"))
            {
                Script.NodeTools.SetStart();
            }
            if (GUILayout.Button("设置BOSS点"))
            {
                Script.NodeTools.SetBoss();
            }
            if (GUILayout.Button("设置怪物点"))
            {
                Script.NodeTools.SetMonster();
            }
            if (GUILayout.Button("设置边界点"))
            {
                Script.NodeTools.SetBorder();
            }
            if (GUILayout.Button("设置空底格"))
            {
                Script.NodeTools.SetNull();
            }
        }
    }
}

