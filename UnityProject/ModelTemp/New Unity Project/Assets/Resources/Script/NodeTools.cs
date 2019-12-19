using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Script.Components;
using JsonFx;

namespace Script
{
    public class NodeTools : MonoBehaviour
    {
        private static void SetNodeType(GameObject node, string tag)
        {
            var nodeSetting = node.GetComponent<NodeBaseComponent>();
            var nodeRender = node.GetComponent<SpriteRenderer>();
            var nodeText = node.GetComponentInChildren<TextMesh>();
            nodeSetting.NodeType = tag;
            if (tag!="Null")
            {
                node.SetActive(true);
                nodeRender.sprite = Resources.Load<Sprite>("Sprites/Node");
            }
            else
            {
                node.SetActive(false);
                nodeRender.sprite = null;
            }
            switch (tag)
            {
                case "Start":
                    nodeSetting.NodeArea = -1;
                    nodeRender.color = Color.green;
                    nodeText.text = $"Start\n{nodeSetting.StartNodeId}\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    break;

                case "Node":
                    if (nodeSetting.NodeArea == -1)
                    {
                        nodeSetting.NodeArea = -1;
                        nodeRender.color = Color.white;
                        nodeText.text = $"Node\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    }
                    else
                    {
                        var nodeColor = new Color(0f, 0f, nodeSetting.NodeArea  * 0.02f, 1f);
                        nodeRender.color = nodeColor;
                        nodeText.text = $"Area-{nodeSetting.NodeArea}\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    }
                    break;
                case "Boss":
                    nodeSetting.NodeArea = -1;
                    nodeRender.color = Color.red;
                    nodeText.text = $"Boss\n{nodeSetting.BossNodeId}\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    break;
                case "Monster":
                    nodeSetting.NodeArea = -1;
                    nodeRender.color = Color.yellow;
                    nodeText.text = $"Monster\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    break;
                case "Border":
                    nodeSetting.NodeArea = -1;
                    nodeRender.color = Color.gray;
                    nodeText.text = $"Border\n{nodeSetting.BorderNodeId}\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    break;
                case "NoObjectGround":
                    nodeSetting.NodeArea = -1;
                    nodeRender.color = Color.gray;
                    nodeRender.color+=nodeRender.color.linear;
                    nodeText.text = $"NoObjectGround\n{nodeSetting.BorderNodeId}\n{nodeSetting.NodeId}\n({nodeSetting.NodeColIndex},{nodeSetting.NodeRowIndex})";
                    break;
                case "Null":
                    nodeSetting.NodeArea = -1;
                    nodeRender.color = Color.gray;
                    nodeText.text = "";
                    break;
                default:
                    break;
            }
        }
        internal static void SetStart()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node,"Start");
            }
        }

        internal static void SetNode()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node, "Node");
            }
        }

        internal static void SetNoObjectGround()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node, "NoObjectGround");
            }
        }

        internal static void SetBoss()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node, "Boss");
            }
        }

        internal static void SetMonster()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node, "Monster");
            }
        }

        internal static void SetBorder()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node, "Border");
            }
        }

        internal static void SetNull()
        {
            var nodeList = Selection.gameObjects;
            foreach (var node in nodeList)
            {
                SetNodeType(node, "Null");
            }
        }
    }
}

