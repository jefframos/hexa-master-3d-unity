using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DeckInput))]
[RequireComponent(typeof(DeckView))]

public class StandardBot : MonoBehaviour
{
    DeckInput deckInput;
    DeckView deckView;
    BoardController boardController;
    GameManager gameManager;
    internal class MoveData
    {
        internal Card3D card;
        internal Tile tile;        
        internal int points;
    }

    private RoundManager roundManager;
    private List<List<NeighborModel>> arroundsList;

    internal RoundManager RoundManager {set => roundManager = value; }

    List<MoveData> moveDataList;

    // Start is called before the first frame update
    internal void InitBot()
    {
        deckInput = GetComponent<DeckInput>();
        //deckInput.SetBlock();
        deckView = GetComponent<DeckView>();
        boardController = BoardController.Instance;
        gameManager = GameManager.Instance;
    }
    internal void ChooseMove()
    {

        if(deckView.HandDeck.Count <= 0)
        {
            return;
        }

        deckInput.SetBlock();

        List<MoveData> handMoveData = new List<MoveData>();

        for (int i = 0; i < deckView.HandDeck.Count; i++)
        {
            handMoveData.Add(GetBetterMoveFor(deckView.HandDeck[i]));
        }

        handMoveData.Sort((d1, d2) => d2.points.CompareTo(d1.points));
        MoveData moveData = handMoveData[0];
        Act(moveData);

    }
    void Act(MoveData moveData)
    {
        //this card
        gameManager.SetCurrentCard(moveData.card);
        deckView.CardSelect(moveData.card);

        //this tile
        gameManager.UpdateNeighboursList(moveData.tile);
        gameManager.SelectTile(moveData.tile);
        //deckView.RemoveCurrentCard();
    }
    MoveData GetBetterMoveFor(Card3D card)
    {
        moveDataList = new List<MoveData>();
        CardDynamicData cardDynamic = card.cardDynamicData;
        MoveData moveData;
        for (int i = 0; i < boardController.boardBuilder.boardStaticData.lin; i++)
        {
            for (int j = 0; j < boardController.boardBuilder.boardStaticData.col; j++)
            {
                moveData = new MoveData
                {
                    card = card,
                    tile = boardController.GetTile(i, j)
                };
                if (moveData.tile && moveData.tile.IsAvailable)
                {
                    TestTile(moveData.tile.tileModel, moveData, cardDynamic);
                    moveDataList.Add(moveData);
                }
            }
        }

        ArrayUtils.Shuffle(moveDataList);

        moveDataList.Sort((d1, d2) => d2.points.CompareTo(d1.points));

        return moveDataList[0];
    }

    void TestTile(TileModel tileModel, in MoveData moveData, CardDynamicData cardDynamic)
    {
        cardDynamic.AddPreviewTile(tileModel);
        NeighborsArroundModel currentNeighborsList = boardController.GetNeighbours(tileModel, cardDynamic.PreviewRange);
        currentNeighborsList.FilterListByType(cardDynamic);
        //currentNeighborsList.CapOnFirstBlock();
        //currentNeighborsList.CapOnFirstFind();
        //currentNeighborsList.AddListsOnBasedOnSideList(currentCard.cardDynamicData);
        arroundsList = currentNeighborsList.GetCardArrounds(cardDynamic);
        roundManager.GetAttackLists(arroundsList, cardDynamic, out List<EnemiesAttackData> enemiesActiveList, out List<EnemiesAttackData> enemiesPassiveList);

        moveData.points += enemiesPassiveList.Count * 5;
        for (int i = 0; i < enemiesActiveList.Count; i++)
        {
            RoundManager.ResultType result = roundManager.GetResult(enemiesActiveList[i].cardDynamic, enemiesActiveList[i].neighborModel, cardDynamic);
            switch (result)
            {
                case RoundManager.ResultType.IGNORE:
                    break;
                case RoundManager.ResultType.WIN:
                    moveData.points += 20 + enemiesActiveList[i].cardDynamic.SideList.Count + enemiesActiveList[i].dist * 2;
                    break;
                case RoundManager.ResultType.LOSE:
                    moveData.points -= 100;
                    break;
                case RoundManager.ResultType.DRAW:
                    break;
                case RoundManager.ResultType.BLOCK:
                    break;
                default:
                    break;
            }
        }
       
    }

    internal List<EnemiesAttackData> ChooseBestAttack(List<EnemiesAttackData> entities)
    {
        //List<EntityView> entities = new List<EntityView>();

        return entities;
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
