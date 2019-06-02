using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapInput : MonoBehaviour
{
    // Start is called before the first frame update
   
    public Grid grid; //  You can also use the Tilemap object
    public Camera camera;
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
       // grid.LocalToWorld
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        //Vector3Int coordinate = grid.WorldToCell(grid.LocalToWorld(mouseWorldPos));
        Debug.Log(coordinate + " - "+ mouseWorldPos.x + " - " + mouseWorldPos.y);
    }
  
}
