using System.Collections;
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
    public TextMeshPro effectLabel;
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
    TileEffectView tileEffectView;
    Color currentZoneColor;

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
        effectLabel.text = "";
        effectLabel.gameObject.SetActive(false);
        tileMarker.gameObject.SetActive(false);
        ChangeColorId(standardColor);
        ChangeColor(Color.white);
        tileMarker.ResetMarker();
        outline.enabled = false;
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
        //outline.enabled = false;
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



        //mainMaterial.mainTextureOffset = offs;
    }

    internal void SetDistanceEffect(int distance)
    {
        GameObject tileEffectTransform = GamePool.Instance.GetTileEffect();
        tileEffectTransform.transform.SetParent(transform);
        tileEffectTransform.SetActive(true);
        tileEffectTransform.transform.localPosition = new Vector3();
        TileEffectView tileEffect = tileEffectTransform.GetComponent<TileEffectView>();
        tileEffect.ResetTile();
        tileEffect.SetDistanceFactor(distance);

        tileEffectView = tileEffect;
    }

    internal void SetZone(int v)
    {
        //ChangeColorId(zonesColor[v-1]);

        outline.enabled = true;
        outline.OutlineWidth = 1.5f;



    }
    public void SetFlag()
    {
        flagGameObject = Instantiate(flagPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        flagGameObject.layer = LayerMask.NameToLayer("BoardLayerFront");
        flagGameObject.transform.localScale = new Vector3(0.2f, 0.05f, 0.2f);
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
    internal void RemoveTileEffectView()
    {
        ChangeColor(currentZoneColor);
        effectLabel.gameObject.SetActive(false);
    }
    internal void UpdateTile(TileModel tileModel)
    {
        //Debug.Log("ADICIONAR MAIS ATAQUE BASEADO NA DISTANCIA DO ATAQUE");
        effectLabel.text = "";
        if (tileModel.effectsList.Count > 0)
        {
            ChangeColor(Color.white);
            effectLabel.gameObject.SetActive(true);
        }
        else
        {
            effectLabel.gameObject.SetActive(false);
            return;
        }
        float effectAttack = 0;
        float effectDefense = 0;
        int effectRange = 0;
        for (int i = 0; i < tileModel.effectsList.Count; i++)
        {
            effectAttack += tileModel.effectsList[i].attack;
            effectDefense += tileModel.effectsList[i].defense;
            effectRange += tileModel.effectsList[i].range;
        }
        
        if (effectAttack > 0)
        {
            effectLabel.text += "ATT + " + effectAttack / 10 + "\n";
        }
        if (effectDefense > 0)
        {
            effectLabel.text += "DEF + " + effectDefense / 10 + "\n";
        }
        if (effectRange > 0)
        {
            effectLabel.text += "RNG + " + effectRange + "\n";
        }

    }

    internal void ChangeColor(Color color)
    {
        mainMaterial.color = color;
        outline.OutlineColor = color;
    }

    internal void SetFlagColor(Color color)
    {
        
        Renderer flagRenderer = flagGameObject.GetComponent<Renderer>();
        for (int i = 0; i < flagRenderer.materials.Length; i++)
        {
            flagRenderer.materials[i].DOColor(color, 0.5f);
        }
    }

    public void SetBlock(bool v)
    {
        if (v)
        {
            //outline.enabled = true;
            //outline.OutlineColor = Color.red;
            //ChangeColorId(28f);
            //blockGameObject = Instantiate(blockersList[UnityEngine.Random.Range(0, blockersList.Count)], new Vector3(0, 0, 0), Quaternion.identity, blockerContainer);
            //blockGameObject.layer = LayerMask.NameToLayer("BoardLayerFront");
            //blockGameObject.transform.SetParent(blockerContainer);
            //blockGameObject.transform.localPosition = new Vector3(0,0.01f,0);
            //foreach (Transform child in blockGameObject.GetComponentsInChildren<Transform>(true))
            //{
            //    child.gameObject.layer = LayerMask.NameToLayer("BoardLayerFront");  // add any layer you want. 
            //}

            //cardTransform.transform.localPosition = new Vector3(5f, -2.5f, 0);
        }
    }

    internal void SetZoneColor(Color color)
    {
        currentZoneColor = color;
        ChangeColor(color);
    }

    public void OnOver()
    {
        if (tile.entityAttached)
        {
            tile.entityAttached.OnOver();
        }
        tileMarker.OnOver();
    }

    public void OnOut()
    {
        if (tile.entityAttached)
        {
            tile.entityAttached.OnOut();
        }


        ReturnTileEffect();

        tileMarker.OnOut();
        tileMarker.Deactive();
        //tileMarker.gameObject.SetActive(false);
    }

    public void OnHighlight()
    {
        mainMaterial.DOKill();
    }
    public void OnClear()
    {
        tileMarker.Deactive();
        ReturnTileEffect();
        debug.text = "";
    }

    void ReturnTileEffect()
    {
        if (tileEffectView)
        {
            GamePool.Instance.ReturnTileEffect(tileEffectView.gameObject);

            tileEffectView = null;
        }
    }

}
