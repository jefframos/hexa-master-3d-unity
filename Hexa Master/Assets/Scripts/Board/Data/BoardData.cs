using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BoardData
{
    public enum TileMapType {
        ZONE,FLAG,BLOCK,NEUTRAL,NONE
    }
    public BoardLayerData[] layers;
    public int height;
    public int width;
    BoardTilemapConfig tilemapConfig;
    internal void SetConfig(BoardTilemapConfig _tilemapConfig)
    {
        tilemapConfig = _tilemapConfig;
    }
    internal void CreateMock()
    {
        height = 8;
        width = 11;
        layers = new BoardLayerData[1];
        layers[0] = new BoardLayerData
        {
            data = new int[height * width]
        };
        for (int i = 0; i < height * width; i++)
        {
            layers[0].data[i] = 3;
        }
    }
    internal TileMapType GetType(int id)
    {

        for (int i = 0; i < tilemapConfig.Zones.Length; i++)
        {
            if (tilemapConfig.Zones[i] == id)
            {
                return TileMapType.ZONE;
            }
        }

        for (int i = 0; i < tilemapConfig.ZonesFlags.Length; i++)
        {
            if (tilemapConfig.ZonesFlags[i] == id)
            {
                return TileMapType.FLAG;
            }
        }

       
        
        if (tilemapConfig.Blocker == id)
        {
            return TileMapType.BLOCK;
        }
        if (tilemapConfig.Standard == id)
        {
            return TileMapType.NEUTRAL;
        }
        if (tilemapConfig.Empty == id)
        {
            return TileMapType.NONE;
        }
        return TileMapType.NONE;
    }

    internal int GetZone(int dataId)
    {
        for (int i = 0; i < tilemapConfig.Zones.Length; i++)
        {
            if(tilemapConfig.Zones[i] == dataId)
            {
                return i + 1;
            }

            if (tilemapConfig.ZonesFlags[i] == dataId)
            {
                return i + 1;
            }
        }
        return 0;
    }
}