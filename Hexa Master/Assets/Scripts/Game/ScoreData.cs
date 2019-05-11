using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreData
{    
    internal List<ZoneData> zonesList = new List<ZoneData>();
    List<PlayerData> inGamePlayers;

    internal void CalcScore()
    {
        for (int i = 0; i < inGamePlayers.Count; i++)
        {
            inGamePlayers[i].totalOnBoard = 0;
            inGamePlayers[i].zonesWinning = 0;
        }
        for (int i = 0; i < zonesList.Count; i++)
        {
            zonesList[i].UpdateScore();
        }

    }

    internal int[] GetResult()
    {
        int[] result = new int[4];

       
        return result;
    }

    internal void BuildData(List<PlayerData> _inGamePlayers, List<List<Tile>> tileList)
    {
        inGamePlayers = _inGamePlayers;
        List<int> zones = new List<int>();
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if(tileList[i][j] && !zones.Contains(tileList[i][j].ZoneID))
                {
                    zones.Add(tileList[i][j].ZoneID);
                }
            }
        }

        for (int i = 0; i < zones.Count; i++)
        {
            ZoneData zone = new ZoneData
            {
                zoneID = zones[i],
                players = inGamePlayers
            };
            zone.Init();
            zone.FilterTiles(tileList);
            zonesList.Add(zone);

        }
        
    }
}