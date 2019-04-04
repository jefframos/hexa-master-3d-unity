using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardInput : MonoBehaviour
{
    public string col;
    public Tile currentTile;
    public Tile currentTileSelected;
    public Camera boardCamera;
    //public UnityEvent onTileOver = new UnityEvent<Tile>();

    [System.Serializable]
    public class TileEvent : UnityEvent<Tile> { };
    public TileEvent onTileOver = new TileEvent();
    public TileEvent onTileOut = new TileEvent();
    public TileEvent onTileSelected = new TileEvent();


    private List<int> collidableLayers;
    private Tile tempTile;

    void Start()
    {
        collidableLayers = new List<int>();
        collidableLayers.Add(1 << LayerMask.NameToLayer("BoardLayer"));
    }

    // Update is called once per frame
    void Update()
    {


        col = "testing";

        //{
        RaycastHit hit;

        tempTile = null;

        var ray = boardCamera.ScreenPointToRay(Input.mousePosition);

        //return;
        for (int i = 0; i < collidableLayers.Count; i++)
        {

            if (Physics.Raycast(ray, out hit, collidableLayers[i]))
            {

                tempTile = hit.transform.GetComponent<Tile>();

                if (tempTile != null)
                {

                    if (tempTile.isBlock)
                    {
                       
                        NoTileOver();
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            TileSelect(tempTile);
                        }
                        OnTileOver(tempTile);
                    }
                    
                    
                }
            }
        }

        if (tempTile == null && currentTile)
        {
            NoTileOver();
        }

        //}

    }
    void TileSelect(Tile tile)
    {
        if (tile.hasCard)
        {
            return;
        }
        if (currentTileSelected != tile)
        {
            currentTileSelected = tile;

        }
        onTileSelected.Invoke(currentTileSelected);
    }
    void NoTileOver()
    {
        if (currentTile)
        {
            currentTile.tileView.OnOut();
        }
        onTileOut.Invoke(currentTile);
        currentTile = null;
    }
    void OnTileOver(Tile tile)
    {
        if (currentTile)
        {
            if (currentTile.tileModel.id == tile.tileModel.id)
            {
                return;
            }
            currentTile.tileView.OnOut();

        }

        currentTile = tile;
        col = currentTile.tileModel.i + "-" + currentTile.tileModel.j + "-" + currentTile.tileModel.id;

        currentTile.tileView.OnOver();
        onTileOver.Invoke(currentTile);
    }
}
