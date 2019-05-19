using System;
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
    private CardDynamicData cardDynamicData;
    public TextMeshPro attackLabel;
    public TextMeshPro defenseLabel;
    public TextMeshPro rangeLabel;
    public TextMeshPro nameLabel;
    public List<GameObject> starsList;

    public SpriteRenderer[] allRenderers;
    public List<int> allRenderersOrder;

    public TextMeshPro[] allTextMesh;
    public List<int> allTextMeshOrder;

    public ParticleSystem[] allParticles;
    public List<int> allParticlesOrder;

    private bool isSelected = false;

    private AttackZonesCardView attackZonesView;

    private bool orderSaved = false;


    public void SetData(CardStaticData _cardStaticData, CardDynamicData _cardDynamicData)
    {
        if (!orderSaved)
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
            orderSaved = true;
        }
        //Save the order of the sprites
        //allParticles = GetComponentsInChildren<ParticleSystem>();
        //allParticlesOrder = new List<int>();
        //for (int i = 0; i < allParticles.Length; i++)
        //{
        //    allParticlesOrder.Add(allParticles[i].re);
        //}

        cardStaticData = _cardStaticData;
        cardDynamicData = _cardDynamicData;

        attackLabel.text = (cardDynamicData.StaticAttack / 10).ToString();
        defenseLabel.text = (cardDynamicData.StaticDefense / 10).ToString();
        rangeLabel.text = cardDynamicData.StaticRange.ToString();

        for (int i = 0; i < starsList.Count; i++)
        {
            starsList[i].SetActive(false);
        }

        for (int i = 0; i < cardStaticData.level + 1; i++)
        {
            starsList[i].SetActive(true);
        }
        var sp = Resources.Load<Sprite>("Cards/"+ cardStaticData.folder+"/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));

        nameLabel.text = cardStaticData.name;
        //GameObject attackZones = Instantiate(attackZonesPrefab, new Vector3(0, 0, 0), Quaternion.identity, attackZonesParent);
        //cardTransform.transform.localPosition = new Vector3(5f, -2.5f, 0);
        attackZonesView = GetComponentInChildren<AttackZonesCardView>();
        attackZonesView.SetZones(cardDynamicData.sideList);
        //attackZonesView.SetTeamID(cardDynamicData.teamID);
        attackZonesView.SetTeamColor(cardDynamicData.teamColor);
        
        attackZonesView.setDeckLayer();
        charSprite.sprite = sp;
        if(cardStaticData.folder == "new")
        {
            charSprite.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            charSprite.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        }
        isSelected = false;
    }

    internal void ResetCard()
    {
        isSelected = false;
        spriteSelected.color = Color.white;
        SetOrder(0);
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
        //charSprite.color = Color.white;
    }
    public void OnOver()
    {
        if (isSelected)
        {
            return;
        }
        //charSprite.color = Color.red;
    }
    public void OnSelect()
    {
        isSelected = true;
        spriteSelected.color = cardDynamicData.teamColor;
    }
    public void OnUnSelect()
    {
        isSelected = false;
        spriteSelected.color = Color.white;
    }
}
