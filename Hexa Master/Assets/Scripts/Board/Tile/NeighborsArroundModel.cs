﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeighborsArroundModel
{
    public List<NeighborModel> topLeft = new List<NeighborModel>();
    public List<NeighborModel> topRight  = new List<NeighborModel>();
    public List<NeighborModel> left  = new List<NeighborModel>();
    public List<NeighborModel> right = new List<NeighborModel>();
    public List<NeighborModel> bottomLeft  = new List<NeighborModel>();
    public List<NeighborModel> bottomRight  = new List<NeighborModel>();
    public void CapOnFirstBlock()
    {

        CapList(topLeft);
        CapList(topRight);
        CapList(left);
        CapList(right);
        CapList(bottomLeft);
        CapList(bottomRight);
    }
    void CapList(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].tile && list[i].tile.isBlock)
            {
                list.RemoveRange(i, list.Count - i);
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
        return arroundsList;
    }
}
