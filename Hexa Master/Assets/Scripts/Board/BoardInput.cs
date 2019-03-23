using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardInput : MonoBehaviour
{
    public string col;
    public Tile currentTile;

    //public UnityEvent onTileOver = new UnityEvent<Tile>();

    [System.Serializable]
    public class TileEvent : UnityEvent<Tile>{};
    public TileEvent onTileOver = new TileEvent();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       

        col = "testing";
    
        //{
        RaycastHit hit;
        Tile temp;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            temp = hit.transform.GetComponent<Tile>();
            if(!currentTile || temp.tileModel.id != currentTile.tileModel.id)
            {
                OnTileOver(temp);
            }
            
        }
        //}

    }
    void OnTileOver(Tile tile)
    {
        if (currentTile)
        {
            currentTile.tileView.OnOut();
        }
        currentTile = tile;
        col = currentTile.tileModel.i + "-" + currentTile.tileModel.j + "-" + currentTile.tileModel.id;

        currentTile.tileView.OnOver();
        onTileOver.Invoke(currentTile);
    }
}
