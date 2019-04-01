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
    public List<GameObject> blockersList;
    public Transform blockerContainer;
    private int maxColors = 32;
    public bool isBlock = false;
    public EntityView entityAttached;
    // Start is called before the first frame update
    void Start()
    {
        entityAttached = null;
        mainMaterial = mainRenderer.GetComponent<Renderer>().material;
        Vector2 offs = mainMaterial.mainTextureOffset;
        offs.x = maxColors / 256f * 3f;
        mainMaterial.mainTextureOffset = offs;
        debug.text = "";
        outline.enabled = false;
        //REVER ISSO, TAH BEM ESTRANHO AS TWEEN, PARECE UE TEM MUITA COISA CONFLITANDO
        //0.125
    }
    public void setBlock(bool v)
    {
        if (v)
        {
            GameObject blockTransform = Instantiate(blockersList[Random.Range(0, blockersList.Count)], new Vector3(0, 0, 0), Quaternion.identity, blockerContainer);
            blockTransform.layer = LayerMask.NameToLayer("BoardLayerFront");
            foreach (Transform child in blockTransform.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("BoardLayerFront");  // add any layer you want. 
            }
            //cardTransform.transform.localPosition = new Vector3(5f, -2.5f, 0);
        }
    }
    public void OnOver()
    {
        if (entityAttached)
        {
            entityAttached.OnOver();
        }
        mainMaterial.DOKill();
        mainMaterial.DOColor(mouseOverColor, 0.5f);
        //outline.enabled = true;
    }

    public void OnOut()
    {
        if (entityAttached)
        {
            entityAttached.OnOut();
        }
        mainMaterial.DOKill();
        mainMaterial.DOColor(Color.white, 0.5f);
        //mainMaterial.color = Color.white;
        outline.enabled = false;
    }

    public void OnHighlight()
    {
        mainMaterial.DOKill();
        mainMaterial.DOColor(highlightColor, 0.5f);
        //outline.enabled = true;
    }
    public void OnClear()
    {
        mainMaterial.DOKill();
        //mainMaterial.color = Color.white;
        mainMaterial.DOColor(Color.white, 0.5f);
        outline.enabled = false;
        debug.text = "";
    }

}
