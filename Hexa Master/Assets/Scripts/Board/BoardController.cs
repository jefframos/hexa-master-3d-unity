using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : Singleton<BoardController>
{
    [System.Serializable]
    public class TileList
    {
        public int i = 0;
        public List<Tile> tiles;
    }
    List<List<Tile>> tileList;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetBoard(List<List<Tile>> _tileList)
    {
        tileList = _tileList;
    }


    public NeighborModel GetTileOnSide(TileModel tile, SideType side, int distance = 1)
    {
        NeighborModel neighbor = new NeighborModel();
        // int adj =(tile.j % 2 == 0) ? 0 : -1;
        int adj = (tile.i % 2 == 0) ? -1 : 0;


        switch (side)
        {
            case SideType.TopLeft:
                neighbor.i = tile.i - 1;
                neighbor.j = tile.j + adj;
                neighbor.side = SideType.TopLeft;
                neighbor.distance = distance;

                break;
            case SideType.TopRight:
                neighbor.i = tile.i - 1;
                neighbor.j = tile.j + adj + 1;
                neighbor.side = SideType.TopRight;
                neighbor.distance = distance;

                break;
            case SideType.Left:
                neighbor.i = tile.i;
                neighbor.j = tile.j - 1;
                neighbor.side = SideType.Left;
                neighbor.distance = distance;

                break;
            case SideType.Right:
                neighbor.i = tile.i;
                neighbor.j = tile.j + 1;
                neighbor.side = SideType.Right;
                neighbor.distance = distance;

                break;
            case SideType.BottomLeft:
                neighbor.i = tile.i + 1;
                neighbor.j = tile.j + adj;
                neighbor.side = SideType.BottomLeft;
                neighbor.distance = distance;

                break;
            case SideType.BottomRight:
                neighbor.i = tile.i + 1;
                neighbor.j = tile.j + adj + 1;
                neighbor.side = SideType.BottomRight;
                neighbor.distance = distance;

                break;
        }

        neighbor.tile = GetTile(neighbor.i, neighbor.j);
        return neighbor;

    }
    public Tile GetTile(int i, int j)
    {
        if (i >= tileList.Count || i < 0)
        {
            return null;
        }
        if (j >= tileList[i].Count || j < 0)
        {
            return null;
        }
        return tileList[i][j];
    }
    public NeighborsArroundModel GetNeighbours(TileModel tile, int range = 1)
    {
        NeighborsArroundModel returnObject = new NeighborsArroundModel();

        returnObject.topLeft.Add(GetTileOnSide(tile, SideType.TopLeft, 1));
        returnObject.topRight.Add(GetTileOnSide(tile, SideType.TopRight, 1));
        returnObject.left.Add(GetTileOnSide(tile, SideType.Left, 1));
        returnObject.right.Add(GetTileOnSide(tile, SideType.Right, 1));
        returnObject.bottomLeft.Add(GetTileOnSide(tile, SideType.BottomLeft, 1));
        returnObject.bottomRight.Add(GetTileOnSide(tile, SideType.BottomRight, 1));


        if (range > 1)
        {
            for (int i = 0; i < range; i++)
            {
                if (i < returnObject.topLeft.Count && returnObject.topLeft[i].tile != null)
                {
                    returnObject.topLeft.Add(GetTileOnSide(returnObject.topLeft[i].tile.tileModel, SideType.TopLeft, 1));
                }
                if (i < returnObject.topRight.Count && returnObject.topRight[i].tile != null)
                {
                    returnObject.topRight.Add(GetTileOnSide(returnObject.topRight[i].tile.tileModel, SideType.TopRight, 1));
                }
                if (i < returnObject.left.Count && returnObject.left[i].tile != null)
                {
                    returnObject.left.Add(GetTileOnSide(returnObject.left[i].tile.tileModel, SideType.Left, 1));
                }
                if (i < returnObject.right.Count && returnObject.right[i].tile != null)
                {
                    returnObject.right.Add(GetTileOnSide(returnObject.right[i].tile.tileModel, SideType.Right, 1));
                }
                if (i < returnObject.bottomLeft.Count && returnObject.bottomLeft[i].tile != null)
                {
                    returnObject.bottomLeft.Add(GetTileOnSide(returnObject.bottomLeft[i].tile.tileModel, SideType.BottomLeft, 1));
                }
                if (i < returnObject.bottomRight.Count && returnObject.bottomRight[i].tile != null)
                {
                    returnObject.bottomRight.Add(GetTileOnSide(returnObject.bottomRight[i].tile.tileModel, SideType.BottomRight, 1));
                }
            }

        }

        return returnObject;
    }
}
