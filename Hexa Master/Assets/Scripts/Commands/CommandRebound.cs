using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRebound : CommandDefault
{
    internal class CommandReboundData
    {
        internal List<NeighborModel> allArrounds;
        internal int teamTarget;
    }
    CommandReboundData data;
    float timer = 0.5f;
    int targetTeamID = -1;
    public override void SetData(object obj)
    {
        param = obj;
        data = obj as CommandReboundData;
        timer = 0.5f;
    }
    public override void Update()
    {
        timer -= Time.deltaTime * GameManager.GAME_TIME_SCALE;
        if (timer <= 0)
        {
            Kill();
        }
        base.Update();
    }
    public override void Play()
    {
        //attackType
        for (int i = 0; i < data.allArrounds.Count; i++)
        {
            data.allArrounds[i].tile.entityAttached.ApplyTeamColor();
        }

        base.Play();

    }
}
