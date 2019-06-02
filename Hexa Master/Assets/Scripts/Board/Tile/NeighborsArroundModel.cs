using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeighborsArroundModel
{
    public List<NeighborModel> topLeft = new List<NeighborModel>();
    public List<NeighborModel> topRight = new List<NeighborModel>();
    public List<NeighborModel> left = new List<NeighborModel>();
    public List<NeighborModel> right = new List<NeighborModel>();
    public List<NeighborModel> bottomLeft = new List<NeighborModel>();
    public List<NeighborModel> bottomRight = new List<NeighborModel>();
    public List<List<NeighborModel>> allLists = new List<List<NeighborModel>>();
    public List<CardDynamicData> allEnemies = new List<CardDynamicData>();
    public List<CardDynamicData> allAllies = new List<CardDynamicData>();
    internal int allyCount = 0;
    internal int enemyCount = 0;
    internal TileModel tileModel;

    public void UpdateEntitiesCounter(CardDynamicData cardDynamicData)
    {
        allyCount = 0;
        enemyCount = 0;
        allEnemies = new List<CardDynamicData>();
        allAllies = new List<CardDynamicData>();
        for (int i = 0; i < allLists.Count; i++)
        {
            List<NeighborModel> sideList = allLists[i];
            if (sideList.Count > 0 && sideList[0].Exists)
            {
                if (sideList[0].tile.hasCard)
                {
                    if(cardDynamicData.TeamID == sideList[0].tile.cardDynamicData.TeamID)
                    {
                        allyCount++;
                        allAllies.Add(sideList[0].tile.cardDynamicData);

                    }
                    else
                    {
                        enemyCount++;
                        allEnemies.Add(sideList[0].tile.cardDynamicData);
                    }
                }
            }
        }
    }
    public void AddListsOnList()
    {
        allLists = new List<List<NeighborModel>>
        {
            topLeft,
            topRight,
            left,
            right,
            bottomLeft,
            bottomRight
        };
    }
    public void AddListsOnBasedOnSideList(CardDynamicData data)
    {
        allLists = new List<List<NeighborModel>>();
        for (int i = 0; i < data.SideList.Count; i++)
        {
            if (data.SideList[i] == SideType.TopLeft)
            {
                allLists.Add(topLeft);

            }
            if (data.SideList[i] == SideType.TopRight)
            {
                allLists.Add(topRight);

            }
            if (data.SideList[i] == SideType.Left)
            {
                allLists.Add(left);

            }
            if (data.SideList[i] == SideType.Right)
            {
                allLists.Add(right);

            }
            if (data.SideList[i] == SideType.BottomLeft)
            {
                allLists.Add(bottomLeft);

            }
            if (data.SideList[i] == SideType.BottomRight)
            {
                allLists.Add(bottomRight);

            }
        }
        //removed this and rebound is back, i dont remember why i added this at first
        //for (int i = 0; i < allLists.Count; i++)
        //{
        //    if(allLists[i].Count > data.cardStaticData.stats.range)
        //    {
        //        //allLists[i].RemoveRange(data.cardStaticData.stats.range-1, allLists[i].Count - data.cardStaticData.stats.range);
        //    }
        //}
    }

    internal void FilterListByType(CardDynamicData cardDynamicData)
    {
        switch (cardDynamicData.AttackType)
        {
            case AttackType.AttackFirstFindOnly:
                CapOnFirstBlock();
                CapOnFirstFind();
                //GetOnlyRangeTile(cardDynamicData.PreviewRange);
                break;
            case AttackType.AttackOnlyRangeFind:
                GetOnlyRangeTile(cardDynamicData.PreviewRange);
                break;
            case AttackType.AttackAllRangeFind:
                CapOnFirstBlock();
                break;
            case AttackType.NoAttack:
                break;
            default:
                break;
        }

    }

    internal List<NeighborModel> GetOnlyEntitiesConnected()
    {
        List<NeighborModel> connectedEntities = new List<NeighborModel>();
        for (int i = 0; i < allLists.Count; i++)
        {
            for (int j = 0; j < allLists[i].Count; j++)
            {
                if (allLists[i][j].tile && allLists[i][j].tile.hasCard)
                {
                    connectedEntities.Add(allLists[i][j]);
                }
            }
        }
        return connectedEntities;
    }
    // Cap on the first blocker on the way
    void CapOnFirstBlock()
    {
        CapListOnBlock(topLeft);
        CapListOnBlock(topRight);
        CapListOnBlock(left);
        CapListOnBlock(right);
        CapListOnBlock(bottomLeft);
        CapListOnBlock(bottomRight);
    }
    void CapListOnBlock(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].distance = i + 1;
            if (list[i].tile && list[i].tile.IsBlock)
            {
                list.RemoveRange(i, list.Count - i);
                break;
            }
        }
    }
    //Cap on the first card found
    public void CapOnFirstFind()
    {

        CapListOnFirstFind(topLeft);
        CapListOnFirstFind(topRight);
        CapListOnFirstFind(left);
        CapListOnFirstFind(right);
        CapListOnFirstFind(bottomLeft);
        CapListOnFirstFind(bottomRight);
        
    }
    void CapListOnFirstFind(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            list[i].distance = i + 1;
            if (list[i].tile && (list[i].tile.IsBlock || list[i].tile.hasCard))
            {
                list.RemoveRange(i + 1, list.Count - i - 1);
                break;
            }
        }
    }
    //Get only the tiles based on specific range

    public void GetOnlyRangeTile(int range)
    {

        GetOnlyRangeOnLyst(topLeft, range);
        GetOnlyRangeOnLyst(topRight, range);
        GetOnlyRangeOnLyst(left, range);
        GetOnlyRangeOnLyst(right, range);
        GetOnlyRangeOnLyst(bottomLeft, range);
        GetOnlyRangeOnLyst(bottomRight, range);
        
    }

    void GetOnlyRangeOnLyst(List<NeighborModel> list, int range)
    {

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].Exists)
            {
                if (list[i].distance != range || list[i].tile.IsBlock)
                {
                    list.RemoveRange(i, 1);
                }
            }

        }
    }

    public List<NeighborModel> GetAllEntitiesArroundOnly(int range = 1)
    {
        List<NeighborModel> rebounds = new List<NeighborModel>();
        for (int i = 0; i < allLists.Count; i++)
        {
            for (int j = 0; j < allLists[i].Count; j++)
            {
                if (allLists[i][j].tile && allLists[i][j].tile.hasCard)
                {

                    if (allLists[i][j].distance <= range)
                    {
                        rebounds.Add(allLists[i][j]);
                    }
                }
            }
        }
        return rebounds;
    }


    // public List<List<NeighborModel>> GetCardArrounds(Card3D currentCard)
    public List<List<NeighborModel>> GetCardArrounds(CardDynamicData currentCard)
    {
        List<List<NeighborModel>> arroundsList = new List<List<NeighborModel>>();
        if (currentCard.SideList.Contains(SideType.TopLeft))
        {
            arroundsList.Add(topLeft);
        }
        if (currentCard.SideList.Contains(SideType.TopRight))
        {
            arroundsList.Add(topRight);
        }
        if (currentCard.SideList.Contains(SideType.Left))
        {
            arroundsList.Add(left);
        }
        if (currentCard.SideList.Contains(SideType.Right))
        {
            arroundsList.Add(right);
        }
        if (currentCard.SideList.Contains(SideType.BottomLeft))
        {
            arroundsList.Add(bottomLeft);
        }
        if (currentCard.SideList.Contains(SideType.BottomRight))
        {
            arroundsList.Add(bottomRight);
        }
        AddListsOnList();
        return arroundsList;
    }
}
