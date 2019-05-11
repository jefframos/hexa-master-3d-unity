using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZoneData
{
    // Start is called before the first frame update
    internal List<Tile> tiles = new List<Tile>();
    internal Tile flagTile;
    internal int zoneID;
    internal List<PlayerData> players;
    internal List<PlayerCount> scoreCount = new List<PlayerCount>();
    internal class PlayerCount
    {
        internal bool isWinning = false;
        internal int teamID;
        internal int count = 0;
        internal PlayerData playerData;
    }
    internal void Init()
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlayerCount pcount = new PlayerCount
            {
                teamID = players[i].teamID,
                playerData = players[i]
            };
            scoreCount.Add(pcount);
        }
    }
    internal void FilterTiles(List<List<Tile>> tileList)
    {
        tiles = new List<Tile>();
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j] && tileList[i][j].ZoneID == zoneID)
                {
                    if (tileList[i][j].isFlag)
                    {
                        flagTile = tileList[i][j];
                    }
                    else
                    {
                        tiles.Add(tileList[i][j]);
                    }
                }
            }
        }

        
    }

    internal void UpdateScore()
    {
        string output = "\n";
        for (int i = 0; i < scoreCount.Count; i++)
        {

            output += "ZONE "+zoneID+"\n";

            scoreCount[i].count = 0;
            scoreCount[i].isWinning = false;

            for (int j = 0; j < tiles.Count; j++)
            {
                if (tiles[j].hasCard && scoreCount[i].teamID == tiles[j].TeamID)
                {
                    scoreCount[i].count++;
                    scoreCount[i].playerData.totalOnBoard++;
                }
            }

            output += scoreCount[i].teamID + " - " + scoreCount[i].count + "\n";
        }

        scoreCount = scoreCount.OrderBy(score => score.count).ToList();
        scoreCount.Reverse();
        if (flagTile)
        {
            if(scoreCount[0].count == 0 || scoreCount[0].count == scoreCount[1].count)
            {
                flagTile.tileView.SetFlagColor(Color.white);
            }
            else
            {
                flagTile.tileView.SetFlagColor(scoreCount[0].playerData.teamColor);
                scoreCount[0].isWinning = true;
                scoreCount[0].playerData.zonesWinning ++;
            }
        }

    }
}
