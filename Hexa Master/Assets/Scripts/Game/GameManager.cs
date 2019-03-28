using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardInput boardInput;
    BoardController boardController;
    public NeighborsArroundModel currentNeighborsList;
    public Tile currentTile;
    Card3D currentCard;
    // Start is called before the first frame update
    void Start()
    {
        boardController = BoardController.Instance;
        boardInput.onTileOver.AddListener(OnTileOver);
        boardInput.onTileOut.AddListener(OnTileOut);
        boardInput.onTileSelected.AddListener(SelectTile);
    }

    public void SetCurrentCard(Card3D card)
    {
        currentCard = card;
    }
    public void SelectTile(Tile tile)
    {
        Debug.Log("CACACA");
        currentTile = tile;
        if (currentCard)
        {
            currentTile.SetCard(currentCard);
            boardController.AddEntity(currentCard, tile);
        }

    }
    void OnTileOut(Tile tile)
    {
        ClearAllNeighbors();
    }
    void OnTileOver(Tile tile)
    {
        //if (currentNeighborsList)
        //{
        ClearAllNeighbors();
        //}
        currentTile = tile;
        currentNeighborsList = boardController.GetNeighbours(tile.tileModel, 2);

        HighlightAllNeighbors();

        tile.tileView.OnOver();

        //Debug.Log("TILE OVER "+tile.tileModel.i+" - "+ tile.tileModel.j);
        //Debug.Log(currentNeighborsList);
    }
    void HighlightAllNeighbors()
    {
        if (currentCard == null)
        {
            return;
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopLeft))
        {
            HighlightList(currentNeighborsList.topLeft, "TL");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopRight))
        {
            HighlightList(currentNeighborsList.topRight, "TR");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Left))
        {
            HighlightList(currentNeighborsList.left, "L");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Right))
        {
            HighlightList(currentNeighborsList.right, "R");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomLeft))
        {
            HighlightList(currentNeighborsList.bottomLeft, "BL");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomRight))
        {
            HighlightList(currentNeighborsList.bottomRight, "BR");
        }
    }
    void HighlightList(List<NeighborModel> list, string debug = "")
    {
        //for (int i = 0; i < list.Count; i++)
        for (int i = 0; i < list.Count; i++)
        {
            if (i < currentCard.cardStaticData.stats.range)
            {
                NeighborModel neighbor = list[i];
                if (neighbor.tile)
                {
                    neighbor.tile.tileView.OnHighlight();
                    neighbor.tile.tileView.debug.text = debug;
                }
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
