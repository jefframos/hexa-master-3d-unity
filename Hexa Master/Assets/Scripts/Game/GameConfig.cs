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
    public ClassColorScheme classColorScheme;

    internal Color GetTeamColor(int teamID)
    {
        return teamColorData[teamID].color;
    }

    internal Color GetClassColor(ClassType classType)
    {
        for (int i = 0; i < classColorScheme.classColors.Length; i++)
        {
            if(classColorScheme.classColors[i].classType == classType)
            {
                return classColorScheme.classColors[i].classColor;
            }
        }
        return new Color();
    }
}
