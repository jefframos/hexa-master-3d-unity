﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

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
    public List<GameObject> blockersList;
    public Transform blockerContainer;
    private int maxColors = 32;
    public bool isBlock = false;
    internal Tile tile;
    public TileMarkerView tileMarker;
    public float standardColor = 22f;
    public int[] zonesColor;
    private GameObject blockGameObject;
    private GameObject flagGameObject;
    public GameObject flagPrefab;
    internal void ResetView()
    {
        if (blockGameObject)
        {
            Destroy(blockGameObject);
        }
        if (flagGameObject)
        {
            Destroy(flagGameObject);
        }
        
        tileMarker.gameObject.SetActive(false);
        ChangeColorId(standardColor);
    }

    // internal Tile tile;
    // Start is called before the first frame update
    void Awake()
    {
        if (mainMaterial == null)
            mainMaterial = mainRenderer.GetComponent<Renderer>().material;
        ChangeColorId(standardColor);

    }
    void Start()
    {
       
        
        debug.text = "";
        outline.enabled = false;
        tileMarker.gameObject.SetActive(false);
        //REVER ISSO, TAH BEM ESTRANHO AS TWEEN, PARECE UE TEM MUITA COISA CONFLITANDO
        //0.125
    }
    void ChangeColorId(float id)
    {
       // if(mainMaterial == null)
         //   mainMaterial = mainRenderer.GetComponent<Renderer>().material;
        Vector2 offs = mainMaterial.mainTextureOffset;
        offs.x = 8f / 256f * id;

        //Debug.Log("CHANGE COLOR " + offs.x);



        mainMaterial.mainTextureOffset = offs;
    }

    internal void SetZone(int v)
    {
        ChangeColorId(zonesColor[v-1]);
    }
    public void SetFlag()
    {
        flagGameObject = Instantiate(flagPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        flagGameObject.layer = LayerMask.NameToLayer("BoardLayerFront");
        flagGameObject.transform.localScale = new Vector3(0.2f, 0.1f, 0.2f);
        flagGameObject.transform.SetParent(blockerContainer);
        flagGameObject.transform.localPosition = Vector3.zero;
        foreach (Transform child in flagGameObject.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("BoardLayerFront");  // add any layer you want. 
        }

        Renderer flagRenderer = flagGameObject.GetComponent<Renderer>();
        ChangeColorId(0);
        for (int i = 0; i < flagRenderer.materials.Length; i++)
        {
            flagRenderer.materials[i].color = Color.white;
        }
        //flagRenderer.materials.color = Color.red;
    }

    internal void SetFlagColor(Color color)
    {
        
        Renderer flagRenderer = flagGameObject.GetComponent<Renderer>();

        for (int i = 0; i < flagRenderer.materials.Length; i++)
        {
            flagRenderer.materials[i].color = color;
        }
    }

    public void SetBlock(bool v)
    {
        if (v)
        {
            //ChangeColorId(28f);
            blockGameObject = Instantiate(blockersList[UnityEngine.Random.Range(0, blockersList.Count)], new Vector3(0, 0, 0), Quaternion.identity, blockerContainer);
            blockGameObject.layer = LayerMask.NameToLayer("BoardLayerFront");
            blockGameObject.transform.SetParent(blockerContainer);
            blockGameObject.transform.localPosition = new Vector3(0,0.01f,0);
            foreach (Transform child in blockGameObject.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("BoardLayerFront");  // add any layer you want. 
            }
            //cardTransform.transform.localPosition = new Vector3(5f, -2.5f, 0);
        }
    }
    public void OnOver()
    {
        if (tile.entityAttached)
        {
            tile.entityAttached.OnOver();
        }
        tileMarker.OnOver();
        //tileMarker.Highlight();
        mainMaterial.DOKill();
        //mainMaterial.DOColor(mouseOverColor, 0.5f);
        //outline.enabled = true;
    }

    public void OnOut()
    {
        if (tile.entityAttached)
        {
            tile.entityAttached.OnOut();
        }
        mainMaterial.DOKill();
        mainMaterial.DOColor(Color.white, 0.5f);
        //mainMaterial.color = Color.white;
        outline.enabled = false;

        tileMarker.Deactive();
        //tileMarker.gameObject.SetActive(false);
    }

    public void OnHighlight()
    {
        mainMaterial.DOKill();
        //mainMaterial.DOColor(highlightColor, 0.5f);

        //tileMarker.Highlight();
        //outline.enabled = true;
    }
    public void OnClear()
    {

        tileMarker.Deactive();

        mainMaterial.DOKill();
        //mainMaterial.color = Color.white;
        mainMaterial.DOColor(Color.white, 0.5f);
        outline.enabled = false;
        debug.text = "";
    }

}
