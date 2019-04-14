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

    public void HighlightAllNeighbors(NeighborsArroundModel _currentNeighborsList, Card3D currentCard)
    {
        currentNeighborsList = _currentNeighborsList;
        currentNeighborsList.CapOnFirstFind();
        currentNeighborsList.AddListsOnBasedOnSideList(currentCard.cardDynamicData);

        for (int i = 0; i < currentNeighborsList.allLists.Count; i++)
        {
            for (int j = 0; j < currentNeighborsList.allLists[i].Count; j++)
            {
                if (currentNeighborsList.allLists[i][j].tile && !currentNeighborsList.allLists[i][j].tile.hasCard)
                {
                    currentNeighborsList.allLists[i][j].tile.tileView.tileMarker.Highlight();

                }
            }
        }

        List<NeighborModel> enemies = currentNeighborsList.GetOnlyEnemiesConnected();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].distance <= currentCard.cardDynamicData.cardStaticData.stats.range)
            {
                NeighborModel enemy = enemies[i];
                if (enemy.tile.cardDynamicData.teamID != currentCard.cardDynamicData.teamID)
                {

                    bool isPassive = false;
                    if (CardsDataManager.Instance.IsPassiveAttack(enemy.tile.cardDynamicData, CardsDataManager.Instance.GetOppositeSide(enemy.side)))
                    {
                        isPassive = true;
                    }
                    if (!isPassive && enemy.tile.cardDynamicData.cardStaticData.stats.defense >= currentCard.cardDynamicData.cardStaticData.stats.attack)
                    {
                        if (enemies[i].distance > 1)
                        {
                            enemy.tile.tileView.tileMarker.DrawPreview();
                        }
                        else
                        {
                            enemy.tile.tileView.tileMarker.LosePreview();

                        }
                    }
                    else
                    {
                        enemy.tile.tileView.tileMarker.WinPreview();
                    }
                }

            }

            //Vector3 pos = enemy.tile.transform.localPosition;
            //enemy.tile.transform.localPosition = pos;
        }
        //if (currentCard == null || acting)
        //{
        //    return;
        //}
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopLeft))
        {
            HighlightList(currentNeighborsList.topLeft, currentCard, "TL");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopRight))
        {
            HighlightList(currentNeighborsList.topRight, currentCard, "TR");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Left))
        {
            HighlightList(currentNeighborsList.left, currentCard, "L");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Right))
        {
            HighlightList(currentNeighborsList.right, currentCard, "R");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomLeft))
        {
            HighlightList(currentNeighborsList.bottomLeft, currentCard, "BL");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomRight))
        {
            HighlightList(currentNeighborsList.bottomRight, currentCard, "BR");
        }
    }
    public void HighlightList(List<NeighborModel> list, Card3D currentCard, string debug = "")
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
    }

    void UpdateTilesPosition()
    {
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
                targetX = -col / 2 + j;
                if (col % 2 != 0)
                {
                    targetX -= tileData.width - tileData.height;
                }
                else
                {
                    targetX += tileData.width - tileData.height;//0.15f;
                }
                targetZ = lin / 2 - i;
                //targetZ = boardData.lin - i;
                if (i % 2 != 0)
                {
                    targetX += 0.5f;
                }
                Tile tile = element[j];
                tile.transform.position = new Vector3(targetX * tileData.width + tileData.width / 4, tile.rnd, targetZ * tileData.height);
                tile.transform.localScale = new Vector3(tileData.scale, tileData.scale, tileData.scale);
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
        entity.SetData(card.cardStaticData, card.cardDynamicData);

        return entity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTilesPosition();
    }
}
