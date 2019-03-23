using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileView : MonoBehaviour
{
    public Material material;
    public TextMeshPro debug;
    public TextMeshPro debugID;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
    }

    public void OnOver()
    {
        material.color = Color.red;
    }

    public void OnOut()
    {
        material.color = Color.white;
    }

    public void OnHighlight()
    {
        material.color = Color.blue;
    }
    public void OnClear()
    {
        material.color = Color.white;
        debug.text = "";
    }
    
}
