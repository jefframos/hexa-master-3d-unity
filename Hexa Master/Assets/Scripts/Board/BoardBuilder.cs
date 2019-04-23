using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{

    [System.Serializable]
    public class BoardData
    {
        public int lin = 10;
        public int col = 10;
    }
    public GameObject TilePrefab;
    public BoardData boardData;
    public List<List<Tile>> tileList;
    public BoardView boardView;
    public int maxBlocks = 3;
    public void BuildBoard()
    {
        int accum = 0;
        //int blockCounter = 0;
        int tempCol = boardData.col;
        List<float> rndPos = new List<float> { -0.1f, 0f, 0.1f };
        tileList = new List<List<Tile>>();
        List<Tile> allTiles = new List<Tile>();
        for (int i = 0; i < boardData.lin; i++)
        {
            List<Tile> tiles = new List<Tile>();

            tempCol = boardData.col;
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
                tile.rnd = 0;// rndPos[(int)Random.Range(0,2)];
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
            if(i >= allTiles.Count)
            {
                break;
            }
            allTiles[i].SetBlock(true);
        }
        BoardController.Instance.SetBoard(tileList);
        boardView.SetTiles(tileList, boardData.lin, boardData.col);

    }
}
