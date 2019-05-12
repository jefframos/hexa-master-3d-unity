using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDynamicData
{
    public List<SideType> sideList;
    public List<SideType> oppositeSideList;
    public int teamID;
    internal Color teamColor;
    internal AttackType attackType = AttackType.AttackFirstFindOnly;
    internal CardStaticData cardStaticData;
    CardsDataManager cardsDataManager;


    List<Effector> effectsList = new List<Effector>();
    List<Effector> tempEffectsList = new List<Effector>();

    public float Attack { get => cardStaticData.stats.attack + effectAttack; }
    public float Defense { get => cardStaticData.stats.defense + effectDefense; }
    public int Range { get => cardStaticData.stats.range + effectRange; }

    public float StaticAttack { get => cardStaticData.stats.attack; }
    public float StaticDefense { get => cardStaticData.stats.defense; }
    public int StaticRange { get => cardStaticData.stats.range; }

    float effectAttack = 0;
    public float EffectAttack { get => effectAttack; }
    float effectDefense = 0;
    public float EffectDefense { get => effectDefense; }
    int effectRange = 0;
    public int EffectRange { get => effectRange; }

    public void AddEffect(Effector effector)
    {
        effectsList.Add(effector);
        effectAttack = 0;
        effectDefense = 0;
        effectRange = 0;
        for (int i = 0; i < effectsList.Count; i++)
        {
            effectAttack += effectsList[i].attack;
            effectDefense += effectsList[i].defense;
            effectRange += effectsList[i].range;
        }
    }
    public void SetData(CardStaticData _cardStaticData)
    {
        effectsList = new List<Effector>();
        teamID = 1;
        cardsDataManager = CardsDataManager.Instance;
        cardStaticData = _cardStaticData;

        List<SideType> tempSideList = new List<SideType>();
        tempSideList.Add(SideType.BottomLeft);
        tempSideList.Add(SideType.BottomRight);
        tempSideList.Add(SideType.Left);
        tempSideList.Add(SideType.Right);
        tempSideList.Add(SideType.TopLeft);
        tempSideList.Add(SideType.TopRight);

        int maxSpdValue = 120;




        //Debug.Log(cardStaticData.stats.speed);

        decimal tot = Math.Floor((decimal)cardStaticData.stats.speed / maxSpdValue * tempSideList.Count);

        if (tot > tempSideList.Count)
            tot = tempSideList.Count;
        else if (tot <= 0)
            tot = 1;


        sideList = new List<SideType>();
        oppositeSideList = new List<SideType>();
        ArrayUtils.Shuffle(tempSideList);

        for (var i = 0; i < tot; i++)
        {
            sideList.Add(tempSideList[i]);

            oppositeSideList.Add(cardsDataManager.GetOppositeSide(tempSideList[i]));
        };

    }

}
