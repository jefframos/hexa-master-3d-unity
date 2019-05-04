using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : Singleton<GameConfig>
{
    [System.Serializable]
    public class TeamColorData
    {
        public Color color;
    }
    // Start is called before the first frame update
    public List<TeamColorData> teamColorData;


    internal Color GetTeamColor(int teamID)
    {
        return teamColorData[teamID].color;
    }
}
