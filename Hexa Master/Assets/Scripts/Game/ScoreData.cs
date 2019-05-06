using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData
{
    //public int player1 = 0;
    //public int player2 = 0;
    //public int player3 = 0;

    internal List<List<int>> allZones = new List<List<int>>();
    internal List<int> allPlayers = new List<int>();

    //internal List<int> zonesScore1 = new List<int> { 0, 0, 0 };
    //internal List<int> zonesScore2 = new List<int> { 0, 0, 0 };
    //internal List<int> zonesScore3 = new List<int> { 0, 0, 0 };
    //internal List<int> zonesScore4 = new List<int> { 0, 0, 0 };
    internal void Reset(int total)
    {
        allZones = new List<List<int>>();
        allPlayers = new List<int>();

        for (int i = 0; i < total; i++)
        {
            allZones.Add(new List<int> { 0, 0, 0, 0 });

            allPlayers.Add(0);
        }
    }
    int GetHigher(int id)
    {
        int currentHighest = 0;
        int currentID = 0;

        int sum = 0;
        for (int i = 0; i < allZones.Count; i++)
        {
            sum += allZones[id][i];

            if (allZones[id][i] > currentHighest)
            {
                currentHighest = allZones[id][i];
                currentID = i;
            }
        }
        if (sum <= 0)
        {
            return 0;
        }

        return currentID + 1;
    }
    internal int[] GetResult()
    {
        int[] result = new int[4];

        for (int i = 0; i < allZones.Count; i++)
        {
            result[i] = GetHigher(i);
        }

        return result;
    }
}