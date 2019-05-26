using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardView : MonoBehaviour
{

    [System.Serializable]
    public class TileData
    {
        public float width = 1;
        public float height = 0.85f;
        public float scale = 1;
    }
    public TileData tileData;
    float starterWidth = -90;
    float starterHeight = -90;
    float starterScale = -90;
    private int lin;
    private int col;
    private bool built;
    private BoardController boardController;
    public List<List<Tile>> tileList;
    public Transform boardContainer;
    public GameObject entityPrefab;
    NeighborsArroundModel currentNeighborsList;
    // Start is called before the first frame update
    void Start()
    {
        built = false;
        boardController = BoardController.Instance;
        currentNeighborsList = new NeighborsArroundModel();
    }

    public void HighlightAllNeighbors(NeighborsArroundModel _currentNeighborsList, CardDynamicData cardDynamicData)
    {
        currentNeighborsList = _currentNeighborsList;
        currentNeighborsList.FilterListByType(cardDynamicData);
       

        for (int i = 0; i < currentNeighborsList.allLists.Count; i++)
        {
            for (int j = 0; j < currentNeighborsList.allLists[i].Count; j++)
            {
                if (currentNeighborsList.allLists[i][j].tile )
                {
                    if (!currentNeighborsList.allLists[i][j].tile.hasCard)
                    {
                        currentNeighborsList.allLists[i][j].tile.Highlight(boardController.currentPlayerData.teamColor);

                    }
                    currentNeighborsList.allLists[i][j].tile.SetNeighborModel(currentNeighborsList.allLists[i][j], cardDynamicData);
                }
            }
        }

        List<NeighborModel> enemies = currentNeighborsList.GetOnlyEntitiesConnected();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].distance <= cardDynamicData.PreviewRange)
            {
                NeighborModel enemy = enemies[i];
                if (enemy.tile.cardDynamicData.TeamID != cardDynamicData.TeamID)
                {

                    if (CardsDataManager.Instance.IsPassiveAttack(enemy.tile.cardDynamicData, CardsDataManager.Instance.GetOppositeSide(enemy.side)))
                    {
                        enemy.TileMarker.WinPreview();
                        enemy.tile.entityAttached.WinFeedback();
                    }
                    else
                    {
                        RoundManager.ResultType result = GameManager.Instance.roundManager.GetResult(enemy.tile.cardDynamicData, enemy, cardDynamicData);
                        switch (result)
                        {
                            case RoundManager.ResultType.IGNORE:
                                break;
                            case RoundManager.ResultType.WIN:
                                enemy.TileMarker.WinPreview();
                                enemy.tile.entityAttached.WinFeedback();
                                break;
                            case RoundManager.ResultType.LOSE:
                                enemy.TileMarker.LosePreview();
                                enemy.tile.entityAttached.LoseFeedback();
                                break;
                            case RoundManager.ResultType.DRAW:
                                enemy.TileMarker.DrawPreview();
                                enemy.tile.entityAttached.BlockFeedback();
                                break;
                            case RoundManager.ResultType.BLOCK:
                                enemy.TileMarker.DrawPreview();
                                enemy.tile.entityAttached.BlockFeedback();
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
        }

        if (cardDynamicData.SideList.Contains(SideType.TopLeft))
        {
            HighlightList(currentNeighborsList.topLeft, cardDynamicData, "TL");
        }
        if (cardDynamicData.SideList.Contains(SideType.TopRight))
        {
            HighlightList(currentNeighborsList.topRight, cardDynamicData, "TR");
        }
        if (cardDynamicData.SideList.Contains(SideType.Left))
        {
            HighlightList(currentNeighborsList.left, cardDynamicData, "L");
        }
        if (cardDynamicData.SideList.Contains(SideType.Right))
        {
            HighlightList(currentNeighborsList.right, cardDynamicData, "R");
        }
        if (cardDynamicData.SideList.Contains(SideType.BottomLeft))
        {
            HighlightList(currentNeighborsList.bottomLeft, cardDynamicData, "BL");
        }
        if (cardDynamicData.SideList.Contains(SideType.BottomRight))
        {
            HighlightList(currentNeighborsList.bottomRight, cardDynamicData, "BR");
        }
    }

    internal void Destroy()
    {
        built = false;
        if (tileList != null)
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                for (int j = 0; j < tileList[i].Count; j++)
                {
                    if (tileList[i][j] != null)
                    {
                        GamePool.Instance.ReturnTile(tileList[i][j].gameObject);

                    }
                }
            }
        }


        tileList = new List<List<Tile>>();
    }

    public void HighlightList(List<NeighborModel> list, CardDynamicData cardDynamicData, string debug = "")
    {
        //for (int i = 0; i < list.Count; i++)
        for (int i = 0; i < list.Count; i++)
        {
            if (i < cardDynamicData.Range)
            {
                NeighborModel neighbor = list[i];
                if (neighbor.tile)
                {
                    neighbor.tile.tileView.OnHighlight();
                    //neighbor.tile.tileView.debug.text = debug;
                }
            }

        }
    }

    public void ClearAllNeighbors()
    {
        ClearList(currentNeighborsList.topLeft);
        ClearList(currentNeighborsList.topRight);
        ClearList(currentNeighborsList.left);
        ClearList(currentNeighborsList.right);
        ClearList(currentNeighborsList.bottomLeft);
        ClearList(currentNeighborsList.bottomRight);
    }
    public void ClearList(List<NeighborModel> list)
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

    //END HIGHLIGHTINGS

    internal void SetTiles(List<List<Tile>> _tileList, int _lin, int _col)
    {
        lin = _lin;
        col = _col;
        tileList = _tileList;
        built = true;
        UpdateTilesPosition();
        Invoke("StartFloatingAllTiles", tileList.Count * 0.15f + 0.5f);
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j])
                {

                    Tile tile = tileList[i][j];
                    tile.StartFloating(j);

                    tile.transform.DOLocalMoveY(-0.5f, 0.5f)
                        .SetDelay(i * 0.15f)
                        .From()
                        .SetEase(Ease.OutBack)
                        .OnStart(() =>
                    {
                        tile.gameObject.SetActive(true);
                    });
                    tile.gameObject.SetActive(false);
                    tile.isFloating = false;

                }

            }
        }
    }
    void StartFloatingAllTiles()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j])
                {
                    tileList[i][j].isFloating = true;
                }
            }
        }
    }

    void UpdateTilesPosition()
    {
        // return;
        if (!built)
        {
            return;
        }
        if (starterWidth == tileData.width
            && starterHeight == tileData.height
             && starterScale == tileData.scale)
        {
            return;
        }
        starterWidth = tileData.width;
        starterHeight = tileData.height;

        float targetX = 0;
        float targetZ = 0;
        for (int i = 0; i < tileList.Count; i++)
        {
            List<Tile> element = tileList[i];
            for (int j = 0; j < element.Count; j++)
            {
                targetX = -(col) / 2 + j;
                if (col % 2 != 0)
                {
                    targetX -= tileData.width - tileData.height + tileData.width / 2;
                }
                else
                {
                    targetX += tileData.width - tileData.height + tileData.width / 2;//0.15f;
                }
                targetZ = lin / 2 - i;
                //targetZ = boardData.lin - i;
                if (i % 2 != 0)
                {
                    targetX += 0.5f;
                }
                if (element[j])
                {
                    Tile tile = element[j];

                    tile.transform.position = new Vector3(targetX * tileData.width + tileData.width / 4, tile.startY, targetZ * tileData.height);
                    //tile.transform.localScale = new Vector3(tileData.scale, tileData.scale, tileData.scale);
                }

            }
        }
    }
    internal CommandDefault PlaceEntity(CardStaticData cardStaticData, CardDynamicData cardDynamicData, Tile tile)
    {
        CommandAddEntity.CommandEntityData data = new CommandAddEntity.CommandEntityData
        {
            cardDynamicData = cardDynamicData,
            cardStaticData = cardStaticData,
            tile = tile,
            entityPrefab = entityPrefab,
            boardView = this
        };

        CommandAddEntity commandAdd = new CommandAddEntity();
        commandAdd.SetData(data);

        return commandAdd;
    }
    internal CommandDefault PlaceCard(Card3D currentCard, Tile tile)
    {
        CommandPlaceCard.CommandPlaceCardData data = new CommandPlaceCard.CommandPlaceCardData
        {
            currentCard = currentCard,
            tile = tile,
            boardContainer = boardContainer
        };

        CommandPlaceCard commandPlace = new CommandPlaceCard();
        commandPlace.SetData(data);

        return commandPlace;

    }
    public EntityView AddEntity(Card3D card, Tile tile)
    {
        GameObject cardTransform = Instantiate(entityPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        cardTransform.transform.localPosition = tile.transform.localPosition;
        EntityView entity = cardTransform.GetComponent<EntityView>();
        entity.SetData(card.cardStaticData, card.cardDynamicData, tile);

        return entity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTilesPosition();
    }
}
