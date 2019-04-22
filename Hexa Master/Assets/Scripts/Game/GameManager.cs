using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public BoardInput boardInput;
    public BoardView boardView;
    BoardController boardController;
    public InGameHUD inGameHUD;
    public RoundManager roundManager;
    public MultipleAttackSelector multipleAttackSelector;

    public CommandList commandList;

    public NeighborsArroundModel currentNeighborsList;
    public Tile currentTile;
    public Card3D currentCard;
    private bool acting = false;
    public List<DeckView> deckViewList;
    private DeckView currentDeckView;
    private DeckInput currentDeckInput;
    public List<int> entitiesOnStart;
    private int currentTeam = 0;
    bool IsBotRound { get => deckViewList[currentTeam].bot != null; }
    StandardBot CurrentBot { get => deckViewList[currentTeam].bot; }

    public float tweenScale = 1f;
    void Update()
    {
        DOTween.timeScale = tweenScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
        boardController = BoardController.Instance;
        boardInput.onTileOver.AddListener(OnTileOver);
        boardInput.onTileOut.AddListener(OnTileOut);
        boardInput.onTileSelected.AddListener(SelectTile);
        Invoke("StartGame", 0.2f);

        //commandList = new CommandList();
        commandList.Reset();
        commandList.onFinishQueue.AddListener(OnFinishCommandQueue);
        //currentDeckView
        //UpdateCurrentTeam();

        roundManager.onRoundReady.AddListener(OnRoundReady);
        roundManager.onMultipleAttack.AddListener(OnMultipleAttack);
        multipleAttackSelector.onMultiplesReady.AddListener(MultipleAttackReady);
    }
    void AddCardOnBoardById(int id)
    {
        CardStaticData cardStaticData = CardsDataManager.Instance.GetCardByID(id);
        CardDynamicData cardDynamicData = new CardDynamicData();
        cardDynamicData.SetData(cardStaticData);
        cardDynamicData.teamID = 5;

        Tile tile = boardController.GetRandomEmpryTile();
        boardController.PlaceCard(cardDynamicData, tile);
        boardView.PlaceEntity(cardStaticData, cardDynamicData, tile).Play();
    }
    void StartGame()
    {
        UpdateCurrentTeam();
        inGameHUD.UpdateCurrentRound(currentTeam + 1, 0, 0);

        if (entitiesOnStart != null)
        {
            for (int i = 0; i < entitiesOnStart.Count; i++)
            {
                //AddCardOnBoardById(entitiesOnStart[i]);

            }
        }
        //AddCardOnBoardById(637);
        //AddCardOnBoardById(290);
        //AddCardOnBoardById(23);
    }

    public void SetCurrentCard(Card3D card)
    {
        currentCard = card;
    }
    void MultipleAttackReady(List<EnemiesAttackData> attackList)
    {
        roundManager.GenerateRoundCommands(attackList, currentCard.cardDynamicData, currentTile);
    }
    void OnMultipleAttack(List<EnemiesAttackData> attackList)
    {
        List<EntityView> entities = new List<EntityView>();
        for (int i = 0; i < attackList.Count; i++)
        {
            EnemiesAttackData element = attackList[i];
            entities.Add(element.tile.entityAttached);
        }
        if (IsBotRound)
        {
            MultipleAttackReady(CurrentBot.ChooseBestAttack(attackList));
        }
        else
        {
            multipleAttackSelector.SetEntities(attackList);

        }
    }

    //finish to calc the round
    void OnRoundReady(List<CommandDefault> roundCommands)
    {
        for (int i = 0; i < roundCommands.Count; i++)
        {
            commandList.AddCommand(roundCommands[i]);
        }
        commandList.Play();
        currentTeam++;
        currentTeam %= deckViewList.Count;



    }
    //finish command list, normally after a round
    void OnFinishCommandQueue()
    {
        commandList.Reset();
        acting = false;

        boardInput.enabled = true;
        Invoke("UpdateCurrentTeam", 0.1f);

    }
    //click on tile on board
    public void SelectTile(Tile tile)
    {
        if (currentDeckInput.mouseOverDeck)
        {
            return;
        }
        currentTile = tile;
        if (roundManager.CanPlance(tile, currentCard.cardDynamicData))
        {
            boardView.ClearAllNeighbors();
            boardInput.enabled = false;
            currentDeckInput.SetBlock();
            currentDeckView.RemoveCurrentCard();
            currentDeckView.SetBlock();
            acting = true;
            boardController.PlaceCard(currentCard.cardDynamicData, tile);

            commandList.AddCommand(boardView.PlaceCard(currentCard, tile));
            commandList.AddCommand(boardView.PlaceEntity(currentCard.cardStaticData, currentCard.cardDynamicData, tile)).AddCallback(() =>
            {
                commandList.Reset();
                roundManager.DoRound(tile, currentNeighborsList, currentCard.cardDynamicData);

            });


            commandList.Play(true);
        }

    }
    void UpdateCurrentTeam()
    {
        currentDeckView = deckViewList[currentTeam];
        currentDeckInput = currentDeckView.GetComponent<DeckInput>();
        for (int i = 0; i < deckViewList.Count; i++)
        {
            if (i == currentTeam)
            {
                deckViewList[i].gameObject.SetActive(true);
            }
            else
            {
                deckViewList[i].gameObject.SetActive(false);
            }
        }

        BoardController.ScoreData score = boardController.GetScore();
        inGameHUD.UpdateCurrentRound(currentTeam + 1, score.player1, score.player2);

        if (IsBotRound)
        {
            Invoke("WaitNextMove", 0.5f);
        }
        else
        {
            currentDeckInput.SetUnblock();
            currentDeckView.SetUnblock(0.15f);
        }

    }

    void WaitNextMove()
    {
        if (IsBotRound)
        {
            CurrentBot.RoundManager = roundManager;
            CurrentBot.ChooseMove();
        }
    }

    void OnTileOut(Tile tile)
    {
        boardView.ClearAllNeighbors();
    }
    internal void UpdateNeighboursList(Tile tile)
    {
        currentNeighborsList = boardController.GetNeighbours(tile.tileModel, currentCard.cardStaticData.stats.range);
    }
    void OnTileOver(Tile tile)
    {
      
        boardView.ClearAllNeighbors();

        if (!tile.hasCard && currentCard && !acting)
        {
            currentTile = tile;
            UpdateNeighboursList(tile);
            //currentNeighborsList = boardController.GetNeighbours(tile.tileModel, currentCard.cardStaticData.stats.range);
            boardView.HighlightAllNeighbors(currentNeighborsList, currentCard);
        }

        tile.tileView.OnOver();
    }
}
