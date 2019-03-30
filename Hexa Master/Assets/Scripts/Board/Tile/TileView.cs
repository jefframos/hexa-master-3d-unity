using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class TileView : MonoBehaviour
{
    public Renderer mainRenderer;
    public TextMeshPro debug;
    public TextMeshPro debugID;
    public List<Renderer> renderers;
    public QuickOutline outline;
    public Color highlightColor;
    public Color mouseOverColor;
    private Material mainMaterial;
    private int maxColors = 32;
    //public
    // Start is called before the first frame update
    void Start()
    {
        mainMaterial = mainRenderer.GetComponent<Renderer>().material;
        Vector2 offs = mainMaterial.mainTextureOffset;
        offs.x = maxColors / 256f * 3f;
        mainMaterial.mainTextureOffset = offs;
        debug.text = "";
        outline.enabled = false;
        //REVER ISSO, TAH BEM ESTRANHO AS TWEEN, PARECE UE TEM MUITA COISA CONFLITANDO
        //0.125
    }

    public void OnOver()
    {
        mainMaterial.DOColor(mouseOverColor, 0.5f);
        outline.enabled = true;
    }

    public void OnOut()
    {
        mainMaterial.DOColor(Color.white, 0.5f);
        outline.enabled = false;
    }

    public void OnHighlight()
    {

        mainMaterial.DOColor(highlightColor, 0.5f);
        outline.enabled = true;
    }
    public void OnClear()
    {
        mainMaterial.DOColor(Color.white, 0.5f);
        outline.enabled = false;
        debug.text = "";
    }

}
