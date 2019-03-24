using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardInput boardInput;
    BoardController boardController;
    public NeighborsArroundModel currentNeighborsList;
    public TileModel currentTile;

    // Start is called before the first frame update
    void Start()
    {
        boardController = BoardController.Instance;
        boardInput.onTileOver.AddListener(OnTileOver);
    }
    void OnTileOver(Tile tile)
    {
        //if (currentNeighborsList)
        //{
        ClearAllNeighbors();
        //}
        currentTile = tile.tileModel;
        currentNeighborsList = boardController.GetNeighbours(tile.tileModel);

        HighlightAllNeighbors();

        tile.tileView.OnOver();

        //Debug.Log("TILE OVER "+tile.tileModel.i+" - "+ tile.tileModel.j);
        //Debug.Log(currentNeighborsList);
    }
    void HighlightAllNeighbors()
    {
        HighlightList(currentNeighborsList.topLeft, "TL");
        HighlightList(currentNeighborsList.topRight, "TR");
        HighlightList(currentNeighborsList.left, "L");
        HighlightList(currentNeighborsList.right, "R");
        HighlightList(currentNeighborsList.bottomLeft, "BL");
        HighlightList(currentNeighborsList.bottomRight, "BR");
    }
    void HighlightList(List<NeighborModel> list, string debug = "")
    {
        for (int i = 0; i < list.Count; i++)
        {
            NeighborModel neighbor = list[i];
            if (neighbor.tile)
            {
                neighbor.tile.tileView.OnHighlight();
                neighbor.tile.tileView.debug.text = debug;
            }
        }
    }

    void ClearAllNeighbors()
    {
        ClearList(currentNeighborsList.topLeft);
        ClearList(currentNeighborsList.topRight);
        ClearList(currentNeighborsList.left);
        ClearList(currentNeighborsList.right);
        ClearList(currentNeighborsList.bottomLeft);
        ClearList(currentNeighborsList.bottomRight);
    }
    void ClearList(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            NeighborModel neighbor = list[i];
            if (neighbor.tile)
            {
                neighbor.tile.tileView.OnClear();

            }
        }
    }

}
