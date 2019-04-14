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
    public List<NeighborModel> allEnemies = new List<NeighborModel>();
    public void AddListsOnList()
    {
        allLists = new List<List<NeighborModel>>();
        allLists.Add(topLeft);
        allLists.Add(topRight);
        allLists.Add(left);
        allLists.Add(right);
        allLists.Add(bottomLeft);
        allLists.Add(bottomRight);
    }
    public void AddListsOnBasedOnSideList(CardDynamicData data)
    {
        allLists = new List<List<NeighborModel>>();
        for (int i = 0; i < data.sideList.Count; i++)
        {
            if (data.sideList[i] == SideType.TopLeft)
            {
                allLists.Add(topLeft);

            }
            if (data.sideList[i] == SideType.TopRight)
            {
                allLists.Add(topRight);

            }
            if (data.sideList[i] == SideType.Left)
            {
                allLists.Add(left);

            }
            if (data.sideList[i] == SideType.Right)
            {
                allLists.Add(right);

            }
            if (data.sideList[i] == SideType.BottomLeft)
            {
                allLists.Add(bottomLeft);

            }
            if (data.sideList[i] == SideType.BottomRight)
            {
                allLists.Add(bottomRight);

            }
        }
        for (int i = 0; i < allLists.Count; i++)
        {
            Debug.Log(allLists[i].Count);
            Debug.Log(data.cardStaticData.stats.range);
            if(allLists[i].Count > data.cardStaticData.stats.range)
            {
                allLists[i].RemoveRange(data.cardStaticData.stats.range - 1, allLists[i].Count - data.cardStaticData.stats.range+1);

            }
        }
    }

    internal List<NeighborModel> GetOnlyEnemiesConnected()
    {
        List<NeighborModel> connectedEnemies = new List<NeighborModel>();
        for (int i = 0; i < allLists.Count; i++)
        {
            for (int j = 0; j < allLists[i].Count; j++)
            {
                if (allLists[i][j].tile && allLists[i][j].tile.hasCard)
                {
                    connectedEnemies.Add(allLists[i][j]);
                }
            }
        }
        return connectedEnemies;
    }

    public void CapOnFirstBlock()
    {
        CapListOnBlock(topLeft);
        CapListOnBlock(topRight);
        CapListOnBlock(left);
        CapListOnBlock(right);
        CapListOnBlock(bottomLeft);
        CapListOnBlock(bottomRight);
    }
    public List<NeighborModel> CapOnFirstFind()
    {
        allEnemies = new List<NeighborModel>();

        CapListOnFirstFind(topLeft);
        CapListOnFirstFind(topRight);
        CapListOnFirstFind(left);
        CapListOnFirstFind(right);
        CapListOnFirstFind(bottomLeft);
        CapListOnFirstFind(bottomRight);

        return allEnemies;
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
    void CapListOnBlock(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].distance = i + 1;
            if (list[i].tile && list[i].tile.isBlock)
            {
                list.RemoveRange(i, list.Count - i);
                break;
            }
        }
    }
    void CapListOnFirstFind(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].distance = i + 1;
            if (list[i].tile && (list[i].tile.isBlock || list[i].tile.hasCard))
            {
                if (list[i].tile.hasCard)
                {
                    allEnemies.Add(list[i]);
                    if (i < list.Count - 2)
                    {
                        //this line makes the last on the queue be the entity
                        list.RemoveRange(i + 1, list.Count - (i + 1));
                    }

                }
                else
                {
                    list.RemoveRange(i, list.Count - i);
                }
                break;
            }
        }
    }
    public List<List<NeighborModel>> GetCardArrounds(Card3D currentCard)
    {
        List<List<NeighborModel>> arroundsList = new List<List<NeighborModel>>();
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopLeft))
        {
            arroundsList.Add(topLeft);
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopRight))
        {
            arroundsList.Add(topRight);
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Left))
        {
            arroundsList.Add(left);
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Right))
        {
            arroundsList.Add(right);
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomLeft))
        {
            arroundsList.Add(bottomLeft);
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomRight))
        {
            arroundsList.Add(bottomRight);
        }
        AddListsOnList();
        return arroundsList;
    }
}
