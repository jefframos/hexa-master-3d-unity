using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDynamicData
{
    public List<SideType> sideList;
    public List<SideType> oppositeSideList;
    public int teamID;
    internal AttackType attackType = AttackType.AttackFirstFindOnly;
    internal CardStaticData cardStaticData;
    CardsDataManager cardsDataManager;

    public float Attack { get => cardStaticData.stats.attack; }
    public float Defense { get => cardStaticData.stats.defense; }

    public void SetData(CardStaticData _cardStaticData)
    {
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
