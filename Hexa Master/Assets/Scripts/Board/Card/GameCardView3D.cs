using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCardView3D : MonoBehaviour
{
    public SpriteRenderer spriteSelected;
    public Color colorSelect;
    public SpriteRenderer charSprite;
    private CardStaticData cardStaticData;
    public TextMeshPro attackLabel;
    public TextMeshPro defenseLabel;
    public TextMeshPro rangeLabel;
    public List<GameObject> starsList;

    public SpriteRenderer[] allRenderers;
    public List<int> allRenderersOrder;

    public TextMeshPro[] allTextMesh;
    public List<int> allTextMeshOrder;

    public ParticleSystem[] allParticles;
    public List<int> allParticlesOrder;

    private bool isSelected = false;

    // Start is called before the first frame update
    //void Start()
    //{

    //}
    public void SetData(CardStaticData _cardStaticData)
    {
        //Save the order of the sprites
        allRenderers = GetComponentsInChildren<SpriteRenderer>();
        allRenderersOrder = new List<int>();
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderersOrder.Add(allRenderers[i].sortingOrder);
        }

        //save the order of the text meshes
        allTextMesh = GetComponentsInChildren<TextMeshPro>();
        allTextMeshOrder = new List<int>();
        for (int i = 0; i < allTextMesh.Length; i++)
        {
            allTextMeshOrder.Add(allTextMesh[i].sortingOrder);
        }

        //Save the order of the sprites
        //allParticles = GetComponentsInChildren<ParticleSystem>();
        //allParticlesOrder = new List<int>();
        //for (int i = 0; i < allParticles.Length; i++)
        //{
        //    allParticlesOrder.Add(allParticles[i].re);
        //}

        cardStaticData = _cardStaticData;

        attackLabel.text = cardStaticData.stats.attack.ToString();
        defenseLabel.text = cardStaticData.stats.defense.ToString();
        rangeLabel.text = cardStaticData.stats.range.ToString();

        for (int i = 0; i < starsList.Count; i++)
        {
            starsList[i].SetActive(false);
        }

        for (int i = 0; i < cardStaticData.level + 1; i++)
        {
            starsList[i].SetActive(true);
        }

        var sp = Resources.Load<Sprite>("Cards/thumbs/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));


        charSprite.sprite = sp;
        isSelected = false;
    }
    public void SetOrder(int order)
    {
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].sortingOrder = allRenderersOrder[i] + order * 1000;
        }

        for (int i = 0; i < allTextMesh.Length; i++)
        {
            allTextMesh[i].sortingOrder = allTextMeshOrder[i] + order * 1000;
        }
    }
    public void OnOut()
    {
        if (isSelected)
        {
            return;
        }
        charSprite.color = Color.white;
    }
    public void OnOver()
    {
        if (isSelected)
        {
            return;
        }
        charSprite.color = Color.red;
    }
    public void OnSelect()
    {
        isSelected = true;
        spriteSelected.color = colorSelect;
    }
    public void OnUnSelect()
    {
        isSelected = false;
        spriteSelected.color = Color.white;
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
