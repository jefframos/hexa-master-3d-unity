using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class TileData
    {
        public float width = 1;
        public float height = 1;
        public float scale = 1;
    }
    [System.Serializable]
    public class BoardData
    {
        public int lin = 10;
        public int col = 10;
    }

    public GameObject TilePrefab;
    public BoardData boardData;
    public TileData tileData;

    float starterWidth = -90;
    float starterHeight = -90;
    float starterScale = -90;
    public List<List<Tile>> tileList;
    void Start()
    {
        //starterWidth = tileData.width;
        //starterHeight = tileData.height;

        int accum = 0;

        tileList = new List<List<Tile>>();
        for (int i = 0; i < boardData.lin; i++)
        {
            List<Tile> tiles = new List<Tile>();

            for (int j = 0; j < boardData.col; j++)
            {

                GameObject tileTransform = Instantiate(TilePrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);

                Tile tile = tileTransform.GetComponent<Tile>();
                tile.tileModel.i = i;
                tile.tileModel.j = j;
                tile.tileModel.id = accum;
                accum++;
                tile.tileView.debugID.text = i + "-" + j;
                tiles.Add(tile);
            }
            tileList.Add(tiles);
        }

        UpdateTilesPosition();
        BoardController.Instance.SetBoard(tileList);

    }
    void UpdateTilesPosition()
    {
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
                targetX = -boardData.col / 2 + j;
                targetZ = boardData.lin / 2 - i;
                //targetZ = boardData.lin - i;
                if (i % 2 != 0)
                {
                    targetX += 0.5f;
                }
                Tile tile = element[j];
                tile.transform.position = new Vector3(targetX * tileData.width + tileData.width / 4, 0, targetZ * tileData.height);
                tile.transform.localScale = new Vector3(tileData.scale, tileData.scale, tileData.scale);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTilesPosition();
    }
}
