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
    public Color overColor;

    Color targetColor;
    bool isOver = false;
    // Start is called before the first frame update
    internal void ResetMarker()
    {
        targetColor = standardColor;
        markView.material.color = standardColor;
    }

    //// Update is called once per frame
    void Update()
    {
        markView.material.color = Color.Lerp(markView.material.color, targetColor, 0.3f);
    }

    internal void Deactive()
    {
        Debug.Log("DEACTIVE");
        if (isOver)
        {
            return;
        }
        markView.material.color = standardColor;
        markView.gameObject.SetActive(false);
    }
    internal void OnOut()
    {
        markView.material.color = standardColor;
        isOver = false;
    }
    internal void OnOver()
    {
        targetColor = overColor;
        markView.gameObject.SetActive(true);
        isOver = true;
    }
    internal void Highlight()
    {
        targetColor = highlightColor;
        markView.gameObject.SetActive(true);
    }

    internal void DrawPreview()
    {
        targetColor = drawColor;
        markView.gameObject.SetActive(true);
    }
    internal void WinPreview()
    {
        targetColor = winColor;
        markView.gameObject.SetActive(true);
    }
    internal void LosePreview()
    {
        targetColor = loseColor;
        markView.gameObject.SetActive(true);
    }
}
