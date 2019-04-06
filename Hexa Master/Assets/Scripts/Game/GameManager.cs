using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public BoardInput boardInput;
    public BoardView boardView;
    BoardController boardController;
    public InGameHUD inGameHUD;
    public RoundManager roundManager;

    public NeighborsArroundModel currentNeighborsList;
    public Tile currentTile;
    public Card3D currentCard;
    private bool acting = false;
    public List<DeckView> deckViewList;
    private DeckView currentDeckView;
    private DeckInput currentDeckInput;

    private int currentTeam = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
        boardController = BoardController.Instance;
        boardInput.onTileOver.AddListener(OnTileOver);
        boardInput.onTileOut.AddListener(OnTileOut);
        boardInput.onTileSelected.AddListener(SelectTile);
        Invoke("StartGame", 0.2f);

        //currentDeckView
        //UpdateCurrentTeam();
    }

    void StartGame()
    {
        UpdateCurrentTeam();
        inGameHUD.UpdateCurrentRound(currentTeam + 1, 0, 0);
    }

    public void SetCurrentCard(Card3D card)
    {
        currentCard = card;
    }
    public void SelectTile(Tile tile)
    {
        if (currentDeckInput.mouseOverDeck)
        {
            return;
        }
        currentTile = tile;
        if(roundManager.CanPlance(tile, currentCard))
        {
            //currentCard.cardDynamicData.teamID = 1 + currentTeam * 3;
            roundManager.DoRound(tile, currentNeighborsList, currentCard);
            currentDeckInput.SetBlock();
            currentDeckView.RemoveCurrentCard();
            currentDeckView.SetBlock();
            acting = true;
            boardController.PlaceCard(currentCard, tile);
            boardView.PlaceCard(currentCard, tile, () =>
            {
                acting = false;
                currentDeckInput.SetUnblock();
                currentDeckView.SetUnblock(0.75f);

                Invoke("UpdateCurrentTeam", 0.5f);

            });

            currentTeam++;
            currentTeam %= deckViewList.Count; //2;// decksTransform.Count;

            //UpdateCurrentTeam();
        }

    }
    void UpdateCurrentTeam()
    {
        currentDeckView = deckViewList[currentTeam];
        currentDeckInput = currentDeckView.GetComponent<DeckInput>();
        for (int i = 0; i < deckViewList.Count; i++)
        {
            if(i == currentTeam)
            {
                deckViewList[i].gameObject.SetActive(true);
            }
            else
            {
                deckViewList[i].gameObject.SetActive(false);
            }
        }
    }

    void OnTileOut(Tile tile)
    {
        boardView.ClearAllNeighbors();
    }
    void OnTileOver(Tile tile)
    {
        boardView.ClearAllNeighbors();

        if (!tile.hasCard && currentCard && !acting)
        {
            currentTile = tile;
            currentNeighborsList = boardController.GetNeighbours(tile.tileModel, 2);
            boardView.HighlightAllNeighbors(currentNeighborsList, currentCard);
        }        

        tile.tileView.OnOver();
    }

    

}
