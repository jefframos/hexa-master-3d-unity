using System.Collections.Generic;
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
}
