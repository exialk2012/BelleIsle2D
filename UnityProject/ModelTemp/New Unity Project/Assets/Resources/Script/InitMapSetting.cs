using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Script.Components;
using System;
using System.IO;
using Newtonsoft.Json;
using Script.Classes;
using OfficeOpenXml;

namespace Script
{
    public class InitMapSetting : MonoBehaviour
    {

        [MenuItem("GameObject/Map/InitMapSetting",priority =-100)]
        public static void InitMap()
        {
            if (GameObject.Find("MapSetting"))
            {
                return;
            }
            var _ = new GameObject("MapSetting", typeof(MapSettingComponent));
        }

        internal static void CreatMap(MapSettingComponent mapSetting)
        {
            if (GameObject.Find($"Map-{mapSetting.MapId}")!=null)
            {
                return;
            }
            var map = new GameObject($"Map-{mapSetting.MapId}");
            string filepath = "Sprites/Node";
            for (int i = 0; i < mapSetting.MapWidth; i++)
            {
                for (int j = 0; j < mapSetting.MapHeight; j++)
                {
                    var node = new GameObject($"Node-{i * mapSetting.MapWidth + j}({i},{j})", typeof(NodeBaseComponent), typeof(SpriteRenderer));
                    node.transform.SetParent(map.transform);
                    var nodeComp = node.GetComponent<NodeBaseComponent>();
                    nodeComp.NodeId = i * mapSetting.MapWidth + j;
                    nodeComp.NodeColIndex = i;
                    nodeComp.NodeRowIndex = j;
                    nodeComp.NodeType = "Node";
                    node.transform.Translate(new Vector3(i * 0.9f, j * -0.9f, 0));
                    node.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(filepath);
                    var text = new GameObject("Text", typeof(TextMesh));
                    text.layer = 8;
                    text.transform.SetParent(node.transform);
                    text.transform.Translate(node.transform.position);
                    var textMesh = text.GetComponent<TextMesh>();
                    textMesh.text = $"Node\n{i * mapSetting.MapWidth + j}\n({i},{j})";
                    textMesh.characterSize = 0.1f;
                    textMesh.offsetZ = -1;
                    textMesh.anchor = TextAnchor.MiddleCenter;
                    textMesh.alignment = TextAlignment.Center;
                    textMesh.color = Color.black;
                }
            }
        }

        internal static void TestFunc(MapSettingComponent mapSetting)
        {
            MySqlAccess mySqlAccess = new MySqlAccess("192.168.1.48", "5506","ckz", "123456", "game_twin_cross");
            //mySqlAccess.CreateTable("game_test", new string[] { "Id", "Width", "Height" }, new string[] { "int", "int", "int" });
            var data = mySqlAccess.Select("adventure_map", new string[] { "*" }, new string[] { ""} , new string[] { ""} , new string[] { ""}) .Tables[0];
            string field = "";
            foreach (var item in data.Columns)
            {
                field += $"{item}|";
            }
            Debug.Log(field);
            mySqlAccess.Close();

            #region 废弃的代码
            //var bgSet = mySqlAccess.Select("background", new string[] { "*" }, new string[] { "Id" }, new string[] { ">" }, new string[] { "0" });
            //mySqlAccess.Close();
            //for (int i = 0; i < bgSet.Tables[0].Rows.Count; i++)
            //{
            //    for (int j = 0; j < bgSet.Tables[0].Columns.Count; j++)
            //    {
            //        Debug.Log(bgSet.Tables[0].Rows[i][j]);
            //    }

            //}
            //if (!GameObject.Find($"Map-{mapSetting.MapId}"))
            //{
            //    Debug.Log("111");
            //    return;
            //}
            //var nodeComps = GameObject.Find($"Map-{mapSetting.MapId}").GetComponentsInChildren<NodeBaseComponent>();
            //foreach (var nodeComp in nodeComps)
            //{
            //    string outputJson = JsonConvert.SerializeObject(nodeComp.GetComponent<NodeBaseComponent>());
            //    File.AppendAllText("output.json", outputJson);
            //}
            #endregion
        }

        internal static void GenerateMapDate(MapSettingComponent mapSetting)
        {
            if (GameObject.Find($"Map-{mapSetting.MapId}")==null)
            {
                return;
            }
            var map = GameObject.Find($"Map-{mapSetting.MapId}");
            AdventureMapModel adventureMapModel = new AdventureMapModel
            {
                Id = mapSetting.MapId,
                Name = mapSetting.MapName,
                Description = mapSetting.MapDescription,
                Type = mapSetting.MapType,
                Width = mapSetting.MapWidth,
                Height = mapSetting.MapHeight,
                BackgroundFill = new List<AdventureBackgroundTile>(),
                Fill = new List<AdventureForegroundRangeTile>(),
                View = mapSetting.View,
                GateId = mapSetting.GateId,
                StartFill = new List<AdventureForegroundRangeTile>(),
                BossFill = new List<AdventureForegroundRangeTile>(),
                SpecialFill = new List<AdventureForegroundTile>(),
                GroupFill = new List<AdventureForegroundGroupTile>(),
                RandomFill = new List<AdventureForegroundRangeFillTile>(),
                Background = mapSetting.BackGround,
                PowerOverBuffId = mapSetting.PowerOverBuffId,
                PlaneResListStr = mapSetting.PlaneResListStr,
                KeyCount = mapSetting.KeyCount
            };
            Dictionary<int, AdventureForegroundRangeFillTile> randomNodeDict = new Dictionary<int, AdventureForegroundRangeFillTile>();
            Dictionary<int, AdventureBackgroundTile> randomGroundDict = new Dictionary<int, AdventureBackgroundTile>();
            var nodeCompList = map.GetComponentsInChildren<NodeBaseComponent>();
            string dataFilePath = $"Assets/MapDataJson/Map-{mapSetting.MapId}/";
            var mapDataDict = new Dictionary<string, List<NodeInfo>>();
            List<int> areaCodeList = new List<int>();
            Dictionary<int, List<int>> areaNodeDict = new Dictionary<int, List<int>>();
            string[] mySqlConnectingInfo = new string[]
            {
                "192.168.1.48",
                "5506",
                "ckz",
                "123456",
                "game_twin_cross",
            };
            
            
            foreach (var node in nodeCompList)
            {
                if (node.NodeType == "Null")
                {
                    continue;
                }
                if (node.NodeType == "Start")
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureForegroundRangeTile adventureForeground = new AdventureForegroundRangeTile
                    {
                        ForegroundId = "StartNode",
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.StartFill.Add(adventureForeground);
                    AdventureBackgroundTile adventureBackgroundTile = new AdventureBackgroundTile
                    {
                        BackgroundId = "StartGround",
                        FillCount = 1,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.BackgroundFill.Add(adventureBackgroundTile);
                    #region 废弃代码
                    //var startGround = new MapBackGround(node.StartNodeId, 1, node.NodeColIndex, node.NodeColIndex, node.NodeRowIndex, node.NodeRowIndex);
                    //mapBackGrounds.Add(startGround);
                    //var startNode = new MapStart(node.NodeColIndex, node.NodeColIndex, node.NodeRowIndex, node.NodeRowIndex);
                    //mapStarts.Add(startNode);
                    #endregion
                    continue;
                }
                if (node.NodeType == "Boss")
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureForegroundRangeTile adventureForeground = new AdventureForegroundRangeTile
                    {
                        ForegroundId = "BossNode",
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.BossFill.Add(adventureForeground);
                    AdventureBackgroundTile adventureBackgroundTile = new AdventureBackgroundTile
                    {
                        BackgroundId = "BossGround",
                        FillCount = 1,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.BackgroundFill.Add(adventureBackgroundTile);
                    #region 废弃代码
                    //var BossGround = new MapBackGround(6, 1, node.NodeColIndex, node.NodeColIndex, node.NodeRowIndex, node.NodeRowIndex);
                    //mapBackGrounds.Add(BossGround);
                    //var bossNode = new MapBoss(node.BossNodeId, node.NodeId);
                    //mapBosses.Add(bossNode);
                    #endregion
                    continue;
                }
                if (node.NodeType == "Border")
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureBackgroundTile adventureBackground = new AdventureBackgroundTile {
                        BackgroundId = "BorderGround",
                        FillCount = 1,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.BackgroundFill.Add(adventureBackground);
                    #region 废弃代码
                    //var borderNode = new MapBackGround(node.BorderNodeId, 1, node.NodeColIndex, node.NodeColIndex, node.NodeRowIndex, node.NodeRowIndex);
                    //mapBackGrounds.Add(borderNode);
                    #endregion
                    continue;
                }
                if (node.NodeType == "NoObjectGround")
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureBackgroundTile adventureBackground = new AdventureBackgroundTile
                    {
                        BackgroundId = "NoObjectGround",
                        FillCount = 1,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.BackgroundFill.Add(adventureBackground);
                    #region 废弃代码
                    //var borderNode = new MapBackGround(node.BorderNodeId, 1, node.NodeColIndex, node.NodeColIndex, node.NodeRowIndex, node.NodeRowIndex);
                    //mapBackGrounds.Add(borderNode);
                    #endregion
                    continue;
                }
                if (node.NodeType == "Node"&&node.NodeArea==-1)
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureBackgroundTile adventureBackground = new AdventureBackgroundTile
                    {
                        BackgroundId = "SpecialGround",
                        FillCount = 1,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.BackgroundFill.Add(adventureBackground);
                    AdventureGridPosition adventureGridPosition = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureForegroundTile adventureForegroundTile = new AdventureForegroundTile
                    {
                        ForegroundId = "SpecialNode",
                        Position = adventureGridPosition
                    };
                    adventureMapModel.SpecialFill.Add(adventureForegroundTile);
                    #region 废弃代码
                    //var specialNode = new MapSpecialNode(1001, node.NodeColIndex, node.NodeRowIndex);
                    //mapSpecialNodes.Add(specialNode);
                    //var specialGround = new MapBackGround(3, 1, node.NodeColIndex, node.NodeColIndex, node.NodeRowIndex, node.NodeRowIndex);
                    //mapBackGrounds.Add(specialGround);
                    #endregion
                    continue;
                }
                if (node.NodeType == "Node" && node.NodeArea != -1)
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = node.NodeColIndex,
                        Y = node.NodeRowIndex
                    };
                    if (!areaCodeList.Contains(node.NodeArea))
                    {
                        areaCodeList.Add(node.NodeArea);
                        AdventureForegroundRangeFillTile adventureForegroundRangeFillTile = new AdventureForegroundRangeFillTile
                        {
                            ForegroundId = $"AreaNode-{node.NodeArea}",
                            FillCount = 1,
                            Begin = begin,
                            End = end
                        };
                        randomNodeDict[node.NodeArea] = adventureForegroundRangeFillTile;
                        AdventureBackgroundTile adventureBackground = new AdventureBackgroundTile
                        {
                            BackgroundId = $"AreaGround-{node.NodeArea}",
                            FillCount = 1,
                            Begin = begin,
                            End = end
                        };
                        randomNodeDict[node.NodeArea] = adventureForegroundRangeFillTile;
                        randomGroundDict[node.NodeArea] = adventureBackground;
                        #region 废弃
                        var tempList = new List<int> { 1 ,node.NodeColIndex,node.NodeColIndex,node.NodeRowIndex, node.NodeRowIndex };
                        areaNodeDict[node.NodeArea] = tempList;
                        #endregion
                        continue;
                    }
                    else
                    {
                        randomNodeDict[node.NodeArea].FillCount += 1;
                        randomGroundDict[node.NodeArea].FillCount += 1;
                        if (node.NodeColIndex>randomNodeDict[node.NodeArea].End.X)
                        {
                            randomNodeDict[node.NodeArea].End.X =  node.NodeColIndex;
                        }
                        if (node.NodeRowIndex > randomNodeDict[node.NodeArea].End.Y)
                        {
                            randomNodeDict[node.NodeArea].End.Y = node.NodeRowIndex;
                        }
                        if (node.NodeColIndex > randomGroundDict[node.NodeArea].End.X)
                        {
                            randomGroundDict[node.NodeArea].End.X = node.NodeColIndex;
                        }
                        if (node.NodeRowIndex > randomGroundDict[node.NodeArea].End.Y)
                        {
                            randomGroundDict[node.NodeArea].End.Y = node.NodeRowIndex;
                        }
                        continue;
                    }
                }
            }
            foreach (var key in randomNodeDict.Keys)
            {
                adventureMapModel.RandomFill.Add(randomNodeDict[key]);
            }
            foreach (var key in randomGroundDict.Keys)
            {
                adventureMapModel.BackgroundFill.Add(randomGroundDict[key]);
            }
            if (mapSetting.fill!=null)
            {
                List<AdventureForegroundRangeTile> fillNodeList = new List<AdventureForegroundRangeTile>();
                adventureMapModel.Fill = new List<AdventureForegroundRangeTile>();
                foreach (var fillNode in mapSetting.fill)
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = fillNode.beginX,
                        Y = fillNode.beginY
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = fillNode.endX,
                        Y = fillNode.endY
                    };
                    AdventureForegroundRangeTile fillNodeData = new AdventureForegroundRangeTile
                    {
                        ForegroundId = fillNode.ForegroundId,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.Fill.Add(fillNodeData);
                }
            }

            if (mapSetting.nodeGroups!=null)
            {
                List<AdventureForegroundGroupTile> nodeGroups = new List<AdventureForegroundGroupTile>();
                adventureMapModel.GroupFill = new List<AdventureForegroundGroupTile>();
                foreach (var nodeGroup in mapSetting.nodeGroups)
                {
                    AdventureGridPosition begin = new AdventureGridPosition
                    {
                        X = nodeGroup.beginX,
                        Y = nodeGroup.beginY,
                    };
                    AdventureGridPosition end = new AdventureGridPosition
                    {
                        X = nodeGroup.endX,
                        Y = nodeGroup.endY,
                    };
                    AdventureForegroundGroupTile nodeGroupInfo = new AdventureForegroundGroupTile
                    {
                        ForegroundGroupId = nodeGroup.ForegroundGroupId,
                        Percentage = nodeGroup.Percentage,
                        Begin = begin,
                        End = end
                    };
                    adventureMapModel.GroupFill.Add(nodeGroupInfo);
                }
            }

            #region 序列化
            string mapInfo = JsonConvert.SerializeObject(adventureMapModel);
            string mapGroundInfo = JsonConvert.SerializeObject(adventureMapModel.BackgroundFill);
            string mapStartInfo = JsonConvert.SerializeObject(adventureMapModel.StartFill);
            string mapBossInfo = JsonConvert.SerializeObject(adventureMapModel.BossFill);
            string mapSpecialNodeInfo = JsonConvert.SerializeObject(adventureMapModel.SpecialFill);
            string mapGroupNodeInfo = JsonConvert.SerializeObject(adventureMapModel.GroupFill);
            string mapFillNodeInfo = JsonConvert.SerializeObject(adventureMapModel.Fill);
            string mapRandomNodeInfo = JsonConvert.SerializeObject(adventureMapModel.RandomFill);
            #endregion

            #region 生成Json数据
            if (!Directory.Exists(dataFilePath))
            {
                Directory.CreateDirectory(dataFilePath);
            }
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}.json", mapInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-Ground.json", mapGroundInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-Start.json", mapStartInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-Boss.json", mapBossInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-SpecialNode.json", mapSpecialNodeInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-GroupNode.json", mapGroupNodeInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-FillNode.json", mapFillNodeInfo);
            File.WriteAllText(dataFilePath + $"Map-{mapSetting.MapId}-RandomNode.json", mapRandomNodeInfo);
            #endregion

            #region 生成表格数据
            FileInfo fileInfo = new FileInfo(dataFilePath + "adventure_map.xlsx");
            MySqlAccess mySqlAccess = new MySqlAccess(mySqlConnectingInfo[0], mySqlConnectingInfo[1], mySqlConnectingInfo[2], mySqlConnectingInfo[3], mySqlConnectingInfo[4]);
            var dataSet = mySqlAccess.Select("adventure_map", new string[] { "*" }, new string[] {"" }, new string[] {"" }, new string[] {"" });
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                if (excelPackage.Workbook.Worksheets["adventure_map"]==null)
                {
                    excelPackage.Workbook.Worksheets.Add("adventure_map");
                }
                var sheet = excelPackage.Workbook.Worksheets["adventure_map"];
                for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    sheet.Cells[1, i + 1].Value = dataSet.Tables[0].Columns[i].ToString();
                    if (adventureMapModel.GetType().GetProperties()[i].GetValue(adventureMapModel).GetType()==typeof(int)|| adventureMapModel.GetType().GetProperties()[i].GetValue(adventureMapModel).GetType() == typeof(string))
                    {
                        sheet.Cells[2, i + 1].Value = adventureMapModel.GetType().GetProperties()[i].GetValue(adventureMapModel);
                        continue;
                    }
                    sheet.Cells[2, i + 1].Value = JsonConvert.SerializeObject( adventureMapModel.GetType().GetProperties()[i].GetValue(adventureMapModel));
                }
                excelPackage.Save();
            }
            #endregion

            #region 插入数据库
            string[] insertData = new string[]
            {
                adventureMapModel.Id.ToString(),
                adventureMapModel.Name,
                adventureMapModel.Description,
                adventureMapModel.Type.ToString(),
                adventureMapModel.Width.ToString(),
                adventureMapModel.Height.ToString(),
                JsonConvert.SerializeObject(adventureMapModel.BackgroundFill),
                JsonConvert.SerializeObject(adventureMapModel.Fill),
                adventureMapModel.View.ToString(),
                adventureMapModel.GateId,
                JsonConvert.SerializeObject(adventureMapModel.StartFill),
                JsonConvert.SerializeObject(adventureMapModel.BossFill),
                JsonConvert.SerializeObject(adventureMapModel.SpecialFill),
                JsonConvert.SerializeObject(adventureMapModel.GroupFill),
                JsonConvert.SerializeObject(adventureMapModel.RandomFill),
                adventureMapModel.Background,
                adventureMapModel.PowerOverBuffId,
                adventureMapModel.PlaneResListStr,
                adventureMapModel.KeyCount.ToString(),
            };
            mySqlAccess.InsertInto("adventure_map", insertData);
            #endregion
            Debug.Log($"生成Map-{mapSetting.MapId}数据成功");
        }

        internal static void TestFunc2(MapSettingComponent mapSetting)
        {

        }
    }

    public class MyWindow : EditorWindow
    {
        private void OnDestroy()
        {
            Debug.Log("guanle");
        }
    }
}

