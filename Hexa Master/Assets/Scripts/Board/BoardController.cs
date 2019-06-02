﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

[RequireComponent(typeof(BoardBuilder))]


public class BoardController : Singleton<BoardController>
{
    [System.Serializable]
    public class TileList
    {
        public int i = 0;
        public List<Tile> tiles;
    }


    List<PlayerData> inGamePlayers;
    public TMP_Dropdown dropdown;

    List<List<Tile>> tileList;
    List<CardDynamicData> entitiesPlaced;
    internal PlayerData currentPlayerData;

    public bool debugging = false;
    public bool debugging2 = false;

    internal BoardBuilder boardBuilder;
    ScoreData score;
    Tile[] flags = new Tile[3];
    // Start is called before the first frame update
    void Start()
    {
        boardBuilder = GetComponent<BoardBuilder>();
        score = new ScoreData();

    }
    public ScoreData GetScore()
    {
        score.CalcScore();

        return score;
    }
    public void SetBoard(List<List<Tile>> _tileList)
    {
        tileList = _tileList;
        entitiesPlaced = new List<CardDynamicData>();

        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j] && tileList[i][j].isFlag)
                {
                    int zone = tileList[i][j].tileModel.zone - 1;
                    flags[zone] = tileList[i][j];
                }
            }
        }
    }

    public void PlaceCard(CardDynamicData cardDynamicData, Tile tile)
    {
        tile.SetData(cardDynamicData);
        entitiesPlaced.Add(cardDynamicData);
        SimulateTile(cardDynamicData, tile);
    }

    public void SimulateTile(CardDynamicData cardDynamicData, Tile tile)
    {
     
        if (cardDynamicData.NeighborsArround != null && cardDynamicData.NeighborsArround.tileModel.id == tile.tileModel.id)
        {
            return;
        }
        NeighborsArroundModel neighborsArround = GetNeighbours(tile.tileModel, 1);
        cardDynamicData.NeighborsArround = neighborsArround;

    }
    public Tile GetRandomEmpryTile()
    {
        bool getTile = false;
        int i = 0;
        int j = 0;

        while (!getTile)
        {
            i = UnityEngine.Random.Range(0, tileList.Count);
            j = UnityEngine.Random.Range(0, tileList[i].Count);
            if (tileList[i][j].IsAvailable)
            {
                getTile = true;
            }
        }

        return tileList[i][j];
    }

    public NeighborModel GetTileOnSide(TileModel tile, SideType side, int distance = 1)
    {
        NeighborModel neighbor = new NeighborModel();
        // int adj =(tile.j % 2 == 0) ? 0 : -1;
        int adj = (tile.i % 2 == 0) ? -1 : 0;

        if (debugging && debugging2)
        {
            Debug.Log(tile);
        }
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

    internal void SetInGamePlayers(List<PlayerData> _inGamePlayers, int currentPlayer)
    {
        inGamePlayers = _inGamePlayers;
        currentPlayerData = inGamePlayers[currentPlayer];

        score = new ScoreData();
        score.BuildData(inGamePlayers, tileList);
    }

    internal void BuildBoard()
    {
        //boardBuilder.BuildBoard();

        if (!boardBuilder)
        {
            boardBuilder = GetComponent<BoardBuilder>();
        }
        if (dropdown != null)
        {
            boardBuilder.BuildBoardFromTilemap(dropdown.captionText.text);
        }
        else
        {
            boardBuilder.BuildBoardFromTilemap("map1.json");
        }
    }
    internal void ResetAllTiles()
    {
        if (tileList == null)
        {
            return;
        }
        entitiesPlaced = new List<CardDynamicData>();
        for (int i = 0; i < tileList.Count; i++)
        {
            for (int j = 0; j < tileList[i].Count; j++)
            {
                if (tileList[i][j] != null && tileList[i][j].hasCard && tileList[i][j].entityAttached)
                {
                    Destroy(tileList[i][j].entityAttached.gameObject);
                    tileList[i][j].ResetTile();

                }
            }
        }
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
    public NeighborsArroundModel GetNeighbours(TileModel tile, int range = 1, bool debug = false)
    {
        debugging2 = debug;

        //Debug.Log("verificar se tah pegando certo aqui, no neiboors data tah vindo vazio as cartas na segunda vez, por isso bugando");
        NeighborsArroundModel returnObject = new NeighborsArroundModel();
        returnObject.tileModel = tile;
        returnObject.topLeft.Add(GetTileOnSide(tile, SideType.TopLeft, 1));
        returnObject.topRight.Add(GetTileOnSide(tile, SideType.TopRight, 1));
        returnObject.left.Add(GetTileOnSide(tile, SideType.Left, 1));
        returnObject.right.Add(GetTileOnSide(tile, SideType.Right, 1));
        returnObject.bottomLeft.Add(GetTileOnSide(tile, SideType.BottomLeft, 1));
        returnObject.bottomRight.Add(GetTileOnSide(tile, SideType.BottomRight, 1));

        int dist = 0;
        if (range > 1)
        {
            for (int i = 0; i < range - 1; i++)
            {
                dist = i + 2;
                if (i < returnObject.topLeft.Count && returnObject.topLeft[i].tile != null)
                {
                    returnObject.topLeft.Add(GetTileOnSide(returnObject.topLeft[i].tile.tileModel, SideType.TopLeft, dist));
                }
                if (i < returnObject.topRight.Count && returnObject.topRight[i].tile != null)
                {
                    returnObject.topRight.Add(GetTileOnSide(returnObject.topRight[i].tile.tileModel, SideType.TopRight, dist));
                }
                if (i < returnObject.left.Count && returnObject.left[i].tile != null)
                {
                    returnObject.left.Add(GetTileOnSide(returnObject.left[i].tile.tileModel, SideType.Left, dist));
                }
                if (i < returnObject.right.Count && returnObject.right[i].tile != null)
                {
                    returnObject.right.Add(GetTileOnSide(returnObject.right[i].tile.tileModel, SideType.Right, dist));
                }
                if (i < returnObject.bottomLeft.Count && returnObject.bottomLeft[i].tile != null)
                {
                    returnObject.bottomLeft.Add(GetTileOnSide(returnObject.bottomLeft[i].tile.tileModel, SideType.BottomLeft, dist));
                }
                if (i < returnObject.bottomRight.Count && returnObject.bottomRight[i].tile != null)
                {
                    returnObject.bottomRight.Add(GetTileOnSide(returnObject.bottomRight[i].tile.tileModel, SideType.BottomRight, dist));
                }
            }

        }
        returnObject.AddListsOnList();

        if (debugging && debugging2)
        {
            Debug.Log(tile);
        }

        return returnObject;
    }

    internal void ApplyPassiveBuffs()
    {
        //Debug.Log("AddPos - AttackBuff " + cardsPlaced.Count);
    }
    internal void FinishRound(Card3D currentCard)
    {
        //Debug.Log("AddPos - AttackBuff ");

        for (int i = 0; i < entitiesPlaced.Count; i++)
        {
            entitiesPlaced[i].ResetAllBuffs();
        }

        for (int i = 0; i < entitiesPlaced.Count; i++)
        {
            entitiesPlaced[i].UpdateRound();
        }

        for (int i = 0; i < entitiesPlaced.Count; i++)
        {
            entitiesPlaced[i].FinishRoundBuffs();
        }
    }
    internal void AddPreAttackBuff(Card3D currentCard)
    {
        //Debug.Log("AddPre - AttackBuff");
    }
}
