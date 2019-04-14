using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TileMarkerView : MonoBehaviour
{
    public Renderer markView;

    public Color standardColor;
    public Color highlightColor;
    public Color drawColor;
    public Color loseColor;
    public Color winColor;

    Color targetColor;
    // Start is called before the first frame update
    void Start()
    {
        markView.material.color = standardColor;
    }

    //// Update is called once per frame
    void Update()
    {
        markView.material.color = Color.Lerp(markView.material.color, targetColor, 0.1f);
    }

    internal void Deactive()
    {
        markView.material.color = standardColor;
        markView.gameObject.SetActive(false);
    }

    internal void Highlight()
    {
        targetColor = highlightColor;
        //markView.material.color = standardColor;
        markView.gameObject.SetActive(true);
    }

    internal void DrawPreview()
    {
        targetColor = drawColor;
        //markView.material.color = drawColor;
        markView.gameObject.SetActive(true);
    }
    internal void WinPreview()
    {
        targetColor = winColor;
        //markView.material.color = winColor;
        markView.gameObject.SetActive(true);
    }
    internal void LosePreview()
    {
        targetColor = loseColor;
        //markView.material.color = loseColor;
        markView.gameObject.SetActive(true);
    }
}
