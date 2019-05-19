﻿[System.Serializable]
public class NeighborModel
{
    public int i;
    public int j;
    public Tile tile;
    public int distance;
    public SideType side;
    public TileMarkerView TileMarker { get => tile.tileView.tileMarker; }
}
