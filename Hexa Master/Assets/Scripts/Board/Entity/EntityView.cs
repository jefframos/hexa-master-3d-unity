﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class EntityView : MonoBehaviour
{
    public SpriteRenderer charSprite;
    public TextMeshPro statsLabel;
    AttackZonesCardView attackZones;
    public Renderer teamMaterial;
    private float sin;
    private bool floating;
    private float startY;
    public CardDynamicData cardDynamicData;
    public CardStaticData cardStaticData;
    public GameObject getAttackedParticla;
    // Start is called before the first frame update
    void Start()
    {
        sin = 0;
        floating = false;
        getAttackedParticla.SetActive(false);


    }
    
    public void SetData(CardStaticData _cardStaticData, CardDynamicData _cardDynamicData)
    {
        cardStaticData = _cardStaticData;
        cardDynamicData = _cardDynamicData;
        startY = charSprite.transform.localPosition.y;
        var sp = Resources.Load<Sprite>("Cards/thumbs/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));
        charSprite.sprite = sp;
        charSprite.transform.DOMoveY(charSprite.transform.localPosition.y + 1f, 0.75f).From().SetEase(Ease.OutBounce).OnComplete(()=> {
            EnableFloating();
        });
        statsLabel.text = (cardStaticData.stats.attack / 10) + " / " + (cardStaticData.stats.defense / 10);

        attackZones = GetComponentInChildren<AttackZonesCardView>();
        attackZones.SetZones(cardDynamicData.sideList);
        attackZones.SetInGameMode();

        ApplyTeamColor();
    }
    public void ApplyTeamColor()
    {
        Vector2 offs = teamMaterial.material.mainTextureOffset;
        offs.x = 8f / 256f * (float)cardDynamicData.teamID;
        teamMaterial.material.mainTextureOffset = offs;
    }
    public void EnableFloating()
    {
        floating = true;
    }

    internal void GetAttack()
    {
        getAttackedParticla.SetActive(true);
        Invoke("RemoveParticles", 1f);

    }
    void RemoveParticles()
    {
        getAttackedParticla.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (floating)
        {
            Vector3 targ = charSprite.transform.localPosition;
            targ.y = startY + Mathf.Sin(sin) * 0.075f;
            sin += Time.deltaTime * 1.1f;
            charSprite.transform.localPosition = targ;
        }
      
    }

    internal void OnOver()
    {
        Color targColor = Color.white;
        targColor.a = 0.25f;
        charSprite.color = targColor;
    }

    internal void OnOut()
    {
        charSprite.color = Color.white;
    }
}
