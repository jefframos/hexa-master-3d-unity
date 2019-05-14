using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
    public int maxPlayers = 2;
    private DeckView currentDeckView;
    private DeckInput currentDeckInput;
    public List<int> entitiesOnStart;
    private int currentTeam = 0;
    bool IsBotRound { get => deckViewList[currentTeam].bot != null && !ignoredBots.Contains((int)currentTeam); }
    StandardBot CurrentBot { get => deckViewList[currentTeam].bot; }
    public static float GAME_TIME_SCALE = 1f;
    public float tweenScale = 1f;
    bool isPause = false;
    List<float> ignoredBots;
    internal List<PlayerData> playerDataList;
    public SimpleMenuController simpleMenuController;
    public TMP_Dropdown dropdown;


    void Update()
    {
        if (isPause)
        {
            DOTween.timeScale = 0;
        }
        else
        {
            DOTween.timeScale = tweenScale;

        }
    }
    public void PlayBots()
    {
        ignoredBots = new List<float>();
        StartGame();
    }
    public void PlaySingle()
    {
        ignoredBots = new List<float> { 0 };
        StartGame();
    }
    public void PlayVs()
    {
        ignoredBots = new List<float> { 0, 1 };
        StartGame();
    }
    public void UpdateTimeScale(float time)
    {
        GAME_TIME_SCALE = time;
        tweenScale = time;
    }
    // Start is called before the first frame update
    void Start()
    {
        GAME_TIME_SCALE = tweenScale;
        Application.targetFrameRate = 300;
        playerDataList = new List<PlayerData>();
        for (int i = 0; i < 4; i++)
        {
            PlayerData playerData = new PlayerData();

            switch (i)
            {
                case 0:
                    playerData.deckType = DeckType.HUMAN;
                    break;
                case 1:
                    playerData.deckType = DeckType.ELF;
                    break;
                case 2:
                    playerData.deckType = DeckType.ORC;
                    break;
                case 3:
                    playerData.deckType = DeckType.DWARF;
                    break;
                default:
                    break;
            }

            playerData.teamID = i;// + 1;
            playerDataList.Add(playerData);
        }
        maxPlayers = dropdown.value + 2;
        boardController = BoardController.Instance;
        boardInput.onTileOver.AddListener(OnTileOver);
        boardInput.onTileOut.AddListener(OnTileOut);
        boardInput.onTileSelected.AddListener(SelectTile);
        //Invoke("StartGame", 0.2f);

        //commandList = new CommandList();
        commandList.ResetQueue();
        commandList.onFinishQueue.AddListener(OnFinishCommandQueue);
        //currentDeckView
        //UpdateCurrentTeam();

        roundManager.onRoundReady.AddListener(OnRoundReady);
        roundManager.onMultipleAttack.AddListener(OnMultipleAttack);
        multipleAttackSelector.onMultiplesReady.AddListener(MultipleAttackReady);

        boardController.BuildBoard();
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
    public void TogglePause()
    {
        if (!isPause)
        {
            PauseGame();
        }
        else
        {
            UnPause();
        }
    }
    public void UnPause()
    {
        GAME_TIME_SCALE = tweenScale;
        isPause = false;
    }
    public void PauseGame()
    {
        GAME_TIME_SCALE = 0;
        isPause = true;


    }
    public void DestroyGame()
    {
        PauseGame();
        commandList.Destroy();
        if (currentCard)
        {
            Destroy(currentCard.gameObject);
        }

        boardController.ResetAllTiles();
        boardView.Destroy();

        for (int i = 0; i < deckViewList.Count; i++)
        {
            deckViewList[i].deckBuilder.DestroyDeck();
        }

        currentCard = null;
        currentTile = null;

    }
    public void StartGame()
    {
        simpleMenuController.Hide();
        DestroyGame();
        maxPlayers = dropdown.value + 2;
        boardController.BuildBoard();
        boardInput.enabled = true;
        currentTeam = 0;

        currentCard = null;
        currentTile = null;

        boardController.ResetAllTiles();

        ArrayUtils.Shuffle(playerDataList);
        List<PlayerData> inGamePlayers = new List<PlayerData>();
        for (int i = 0; i < deckViewList.Count; i++)
        {
            if (i < maxPlayers)
            {
                //playerDataList[i].teamID = i + 1;
                deckViewList[i].gameObject.SetActive(true);
                deckViewList[i].ResetDeck();
                deckViewList[i].deckBuilder.InitDeck(playerDataList[i]);
                inGamePlayers.Add(playerDataList[i]);
            }
            else
            {
                deckViewList[i].gameObject.SetActive(false);
            }

        }

        inGameHUD.BuildHud(inGamePlayers);


        boardController.SetInGamePlayers(inGamePlayers, currentTeam);

        UpdateCurrentTeam();


        inGameHUD.UpdateCurrentRound(currentTeam + 1, 0, 0);

        if (entitiesOnStart != null)
        {
            for (int i = 0; i < entitiesOnStart.Count; i++)
            {
                //AddCardOnBoardById(entitiesOnStart[i]);

            }
        }

        UnPause();
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
        currentTeam %= maxPlayers;//deckViewList.Count;

    }
    //finish command list, normally after a round
    void OnFinishCommandQueue()
    {
        commandList.ResetQueue();
        acting = false;

        boardInput.enabled = true;
        Invoke("UpdateCurrentTeam", 0.1f / tweenScale);

        Debug.LogWarning("FINISH COMMAND");

    }
    //click on tile on board
    public void SelectTile(Tile tile)
    {
        if (currentDeckInput == null)
        {
            return;
        }
        if (currentDeckInput.mouseOverDeck)
        {
            return;
        }
        currentTile = tile;
        if (currentCard == null)
        {
            return;
        }
        if (roundManager.CanPlance(tile, currentCard.cardDynamicData))
        {
            currentTile.ForceClear();
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
                commandList.ResetQueue();
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

        ScoreData score = boardController.GetScore();

        inGameHUD.UpdateScore(score);

        //Debug.Log("Zone 1 " + score.zonesScore1[0] + " - " + score.zonesScore2[0]);
        //Debug.Log("Zone 2 " + score.zonesScore1[1] + " - " + score.zonesScore2[1]);
        //Debug.Log("Zone 3 " + score.zonesScore1[2] + " - " + score.zonesScore2[2]);

        //inGameHUD.UpdateCurrentRound(currentTeam + 1, score.player1, score.player2);

        if (IsBotRound)
        {
            Invoke("WaitNextMove", 0.5f / tweenScale);
            //WaitNextMove();
        }
        else
        {
            currentDeckInput.SetUnblock();
            currentDeckView.SetUnblock(0.15f / tweenScale);
        }

        boardController.currentPlayerData = playerDataList[currentTeam];

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
        currentCard.cardDynamicData.AddPreviewTile(tile.tileModel);
        currentNeighborsList = boardController.GetNeighbours(tile.tileModel, currentCard.cardDynamicData.PreviewRange);
        currentNeighborsList.CapOnFirstFind();
        currentNeighborsList.AddListsOnBasedOnSideList(currentCard.cardDynamicData);
    }
    void OnTileOver(Tile tile)
    {

        boardView.ClearAllNeighbors();

        if (!tile.hasCard && currentCard && !acting)
        {
            currentTile = tile;
            UpdateNeighboursList(tile);
            boardView.HighlightAllNeighbors(currentNeighborsList, currentCard);
        }

        //tile.tileView.OnOver();
    }
}
