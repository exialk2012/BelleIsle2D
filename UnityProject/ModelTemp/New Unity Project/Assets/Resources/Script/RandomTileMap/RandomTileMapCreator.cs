using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTileMapCreator : MonoBehaviour
{
    public Tilemap tilemap;
    private Dictionary<string, Tile> arrTiles;
    private List<string> tilesName;
    string[] tileType;
    public int levelW = 10;
    public int levelH = 10;
    // Start is called before the first frame update
    void Start()
    {
        arrTiles = new Dictionary<string, Tile>();
        tilesName = new List<string>();
        InitTile();
        InitMapTilesInfo();
        InitData();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitData()
    {
        for (int i = 0; i < levelH; i++)
        {
            for (int j = 0; j < levelW; j++)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), arrTiles[tileType[i * levelW + j]]);
            }
        }
    }

    private void InitMapTilesInfo()
    {
        tileType = new string[levelW * levelH];
        for (int i = 0; i < levelH; i++)
        {
            for (int j = 0; j < levelW; j++)
            {
                tileType[i * levelW + j] = tilesName[UnityEngine.Random.Range(0, tilesName.Count)];
            }
        }
    }

    private void InitTile()
    {
        AddTile("Ground", "Sprites/Ground");
    }

    private void AddTile(string labelName, string spritePath)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        var tems = Resources.LoadAll<Sprite>(spritePath);
        var seed = UnityEngine.Random.Range(0, tems.Length);
        Sprite tmp = tems[seed];
        tile.sprite = tmp;
        arrTiles.Add(labelName, tile);
        tilesName.Add(labelName);
        Debug.Log(arrTiles.Keys.Count);
        Debug.Log(tilesName.Count);
    }
}
