using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommandAttack : CommandDefault
{
    internal class CommandAttackData
    {
        internal EnemiesAttackData attackData;
        internal CardDynamicData currentCardDynamicData;
        internal EntityView entityAttack;
        internal EntityView entityDefense;
        internal AttackType attackType;
        internal int teamTarget;
        internal bool isBlock;
        internal bool isCounter;
        internal bool isMock;
    }
    CommandAttackData data;
    float timer = 0.5f;
    int targetTeamID = -1;
    public override void SetData(object obj)
    {
        param = obj;
        data = obj as CommandAttackData;
        timer = 0.5f;
    }
    public override void Update()
    {
        //timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Kill();
        }
        base.Update();
    }
    void UpdateTeams()
    {
        //attackType
        if (!data.isBlock && !data.isCounter)
        {
            if (data.attackType == AttackType.Passive)
            {
                data.attackData.cardDynamic.teamID = data.teamTarget;
                data.attackData.tile.entityAttached.ApplyTeamColor();
                Debug.Log("PASSIVE");
            }
            else
            {
                data.attackData.cardDynamic.teamID = data.teamTarget;
                data.attackData.tile.entityAttached.ApplyTeamColor();
                Debug.Log("ACTIVE");
            }
        }
    }
    public override void Play()
    {
       
       
        Vector3 target = data.entityDefense.transform.position - data.entityAttack.transform.position;
        target += data.entityAttack.charSprite.transform.position;
        Vector3 targetStartPosition = data.entityAttack.charSprite.transform.position;
        data.entityAttack.charSprite.transform.DOMove(target, 0.25f).SetEase(Ease.InBack).OnComplete(() => {
            if (data.isBlock)
            {
                data.entityDefense.BlockAttack();

            }
            else if (data.isCounter)
            {
                data.entityDefense.CounterAttack();
            }
            else
            {
                data.entityDefense.GetAttack();
            }
            UpdateTeams();
            //Debug.Log(data.isBlock);
        });
        data.entityAttack.charSprite.transform.DOMove(targetStartPosition, 0.35f).SetEase(Ease.OutBack).SetDelay(0.25f).OnComplete(()=> {
            Kill();
        });

        base.Play();
        
    }

}
