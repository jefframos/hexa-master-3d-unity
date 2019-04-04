using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public BoardInput boardInput;
    public DeckInput deckInput;
    public DeckView deckView;
    public BoardView boardView;
    BoardController boardController;
    public NeighborsArroundModel currentNeighborsList;
    public Tile currentTile;
    Card3D currentCard;
    private bool acting = false;
    public List<GameObject> decksTransform;
    private int currentTeam = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
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
        currentTile = tile;
        if (currentCard)
        {
            Debug.Log(currentTeam);
            currentCard.cardDynamicData.teamID = 1 + currentTeam * 3;
            deckInput.SetBlock();
            deckView.RemoveCurrentCard();
            deckView.SetBlock();
            acting = true;
            boardController.PlaceCard(currentCard, tile);
            boardView.PlaceCard(currentCard, tile, ()=> {
                acting = false;
                deckInput.SetUnblock();
                deckView.SetUnblock(0.75f);
            });      

            currentTeam++;
            currentTeam %= 2;// decksTransform.Count;
        }

    }
    void UpdateCurrentTeam()
    {
        for (int i = 0; i < decksTransform.Count; i++)
        {
            if(i == currentTeam)
            {
                decksTransform[i].SetActive(true);
            }
            else
            {
                decksTransform[i].SetActive(false);
            }
        }
    }

    void OnTileOut(Tile tile)
    {
        ClearAllNeighbors();
    }
    void OnTileOver(Tile tile)
    {
        ClearAllNeighbors();

        if (!tile.hasCard)
        {
            currentTile = tile;
            currentNeighborsList = boardController.GetNeighbours(tile.tileModel, 2);
            HighlightAllNeighbors();
        }        

        tile.tileView.OnOver();
    }

    void HighlightAllNeighbors()
    {
        currentNeighborsList.CapOnFirstBlock();
        if (currentCard == null ||acting)
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
