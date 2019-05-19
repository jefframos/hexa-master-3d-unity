using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{

    [System.Serializable]
    public class BoardStaticData
    {
        public int lin = 8;
        public int col = 11;
    }
    public BoardColorScheme boardColorScheme;
    public GameObject TilePrefab;
    public BoardStaticData boardStaticData;
    public List<List<Tile>> tileList;
    public BoardView boardView;
    public int maxBlocks = 3;
    public BoardDataLoader boardDataLoader;
    public void BuildBoardFromTilemap(string mapUrl)
    {
        BoardData boardData = boardDataLoader.LoadBoard(mapUrl);
        BoardStaticData tempStaticData = new BoardStaticData
        {
            lin = boardData.height,
            col = boardData.width
        };
        int accum = 0;
        int accum2 = 0;
        int tempCol = tempStaticData.col;
        tileList = new List<List<Tile>>();
        List<Tile> allTiles = new List<Tile>();
        for (int i = 0; i < tempStaticData.lin; i++)
        {
            List<Tile> tiles = new List<Tile>();

            tempCol = tempStaticData.col;
            if (i % 2 != 0)
            {
                //tempCol--;
            }
            accum = i * tempStaticData.col;
            for (int j = 0; j < tempCol; j++)
            {
                int dataId = boardData.layers[0].data[accum + j];
                BoardData.TileMapType tileType = boardData.GetType(dataId);

                if (tileType != BoardData.TileMapType.NONE)
                {
                    //Debug.Log(boardData.layers[0].data[accum]);
                    GameObject tileTransform = GamePool.Instance.GetTile();
                    tileTransform.transform.SetParent(transform);
                    tileTransform.SetActive(true);
                    Tile tile = tileTransform.GetComponent<Tile>();
                    tile.ResetTile();
                    tile.tileModel.i = i;
                    tile.tileModel.j = j;
                    tile.tileModel.id = accum2;
                    accum2++;
                    tile.startY = 0;// rndPos[(int)Random.Range(0,2)];
                    tile.tileView.debugID.text = i + "-" + j;
                    tiles.Add(tile);
                    allTiles.Add(tile);

                    if (tileType == BoardData.TileMapType.BLOCK)
                    {
                        tile.SetBlock(true);
                        tile.tileView.ChangeColor(boardColorScheme.Blocker[Random.Range(0, boardColorScheme.Blocker.Length)]);
                    }
                    else if (tileType == BoardData.TileMapType.FLAG)
                    {
                        int zone = boardData.GetZone(dataId);
                        tile.SetZone(boardData.GetZone(dataId));
                        tile.SetFlag(zone);

                        tile.tileView.ChangeColor(boardColorScheme.ZonesFlags[zone - 1]);
                    }
                    else if (tileType == BoardData.TileMapType.ZONE)
                    {
                        int zone = boardData.GetZone(dataId);
                        tile.SetZone(boardData.GetZone(dataId));



                        if (zone == 1)
                        {
                            tile.startY = 0.22f;
                        }
                        //tile.rnd = zone * 0.15f;// - 0.3f;

                        if (Random.Range(0f, 1f) < 0.2f)
                        {
                            var attack = Mathf.FloorToInt(Random.Range(0, 40) / 10);
                            var defense = Mathf.FloorToInt(Random.Range(0, 40) / 10);
                            var range =Random.Range(0, 3);
                            if (attack + defense + range > 0)
                            {

                                Effector tempEffec = new Effector
                                {
                                    attack = attack * 10,
                                    defense = defense * 10,
                                    range = range,
                                };
                                tile.AddEffect(tempEffec);
                                
                            }
                        }
                        tile.tileView.SetZoneColor(boardColorScheme.Zones[zone - 1]);

                    }
                    else
                    {
                        tile.tileView.SetZoneColor(boardColorScheme.Standard[Random.Range(0, boardColorScheme.Standard.Length)]);
                    }

                    tile.UpdateTile();
                }
                else
                {
                    tiles.Add(null);
                }
                //accum++;

            }
            tileList.Add(tiles);
        }

        BoardController.Instance.SetBoard(tileList);
        boardView.SetTiles(tileList, boardStaticData.lin, boardStaticData.col);


    }
    public void BuildBoard()
    {

        int accum = 0;
        //int blockCounter = 0;
        int tempCol = boardStaticData.col;
        List<float> rndPos = new List<float> { -0.1f, 0f, 0.1f };

        tileList = new List<List<Tile>>();
        List<Tile> allTiles = new List<Tile>();
        for (int i = 0; i < boardStaticData.lin; i++)
        {
            List<Tile> tiles = new List<Tile>();

            tempCol = boardStaticData.col;
            if (i % 2 != 0)
            {
                tempCol--;
            }
            for (int j = 0; j < tempCol; j++)
            {

                GameObject tileTransform = GamePool.Instance.GetTile();
                tileTransform.transform.SetParent(transform);
                tileTransform.SetActive(true);
                Tile tile = tileTransform.GetComponent<Tile>();
                tile.ResetTile();
                tile.tileModel.i = i;
                tile.tileModel.j = j;
                tile.tileModel.id = accum;
                tile.startY = 0;// rndPos[(int)Random.Range(0,2)];
                accum++;
                tile.tileView.debugID.text = i + "-" + j;
                //if (Random.Range(0, 1f) < 0.15f && blockCounter < maxBlocks)
                //{
                //    tile.SetBlock(true);
                //    blockCounter++;
                //}
                tiles.Add(tile);
                allTiles.Add(tile);
            }
            tileList.Add(tiles);
        }
        ArrayUtils.Shuffle(allTiles);
        for (int i = 0; i < maxBlocks; i++)
        {
            if (i >= allTiles.Count)
            {
                break;
            }
            allTiles[i].SetBlock(true);
        }
        BoardController.Instance.SetBoard(tileList);
        boardView.SetTiles(tileList, boardStaticData.lin, boardStaticData.col);

    }
}
