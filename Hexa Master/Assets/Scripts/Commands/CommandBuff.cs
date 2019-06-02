using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommandBuff : CommandDefault
{
    internal class CommandBuffData
    {
        internal CardDynamicData cardDynamicData;        
    }
    CommandBuffData data;
    float timer = 0.5f;
    public override void SetData(object obj)
    {
        param = obj;
        data = obj as CommandBuffData;
        timer = 0.5f;
    }
    public override void Update()
    {
        //timer -= Time.deltaTime;
        //if (timer <= 0)
        //{
        //    Kill();
        //}
        base.Update();
    }
 
    public override void Play()
    {
        //Kill();
        Debug.Log("BUFFS pre attack, criar um sistema pra preview isso aqui");
        data.cardDynamicData.ApplyPreAttackBuff();
        //for (int i = 0; i < data.cardDynamicData.NeighborsArround.allLists.Count; i++)
        //{
        //    List<NeighborModel> sideList = data.cardDynamicData.NeighborsArround.allLists[i];
        //    if (sideList.Count > 0 && sideList[0].Exists)
        //    {
        //        if (sideList[0].tile.hasCard)
        //        {
        //            Debug.Log(sideList[0].tile.entityAttached.cardStaticData.name);
        //        }
        //    }
        //}


        base.Play();

        Kill();
    }

}
