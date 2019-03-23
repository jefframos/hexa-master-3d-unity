using System.Collections.Generic;
[System.Serializable]
public class NeighborsArroundModel
{
    public List<NeighborModel> topLeft = new List<NeighborModel>();
    public List<NeighborModel> topRight  = new List<NeighborModel>();
    public List<NeighborModel> left  = new List<NeighborModel>();
    public List<NeighborModel> right = new List<NeighborModel>();
    public List<NeighborModel> bottomLeft  = new List<NeighborModel>();
    public List<NeighborModel> bottomRight  = new List<NeighborModel>();
}
