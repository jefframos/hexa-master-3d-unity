using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileModel tileModel;
    public TileView tileView;
    public float startY = 0;
    public bool isBlock = false;
    internal bool isFlag;
    public bool hasCard = false;
    internal EntityView entityAttached;
    //internal Card3D card;
    internal CardDynamicData cardDynamicData;
    
    private Collider collider;
    public bool IsAvailable { get => !isBlock && !hasCard; }
    public int TeamID { get => cardDynamicData.teamID; }
    internal float sin;
    internal bool isFloating;
    public void ResetTile()
    {
        entityAttached = null;
        isBlock = false;
        //tileModel = null;
        //tileView = null;
        isFlag = false;
        hasCard = false;

        tileModel.zone = -1;

        collider = GetComponent<Collider>();
        collider.enabled = true;
        tileView.ResetView();
        sin = 0;

        transform.localPosition = Vector3.zero;

        isFloating = false;

    }
    internal void StartFloating(float _sin)
    {
        isFloating = true;
        sin = _sin;
        UpdateFloatingPosition();
    }
    internal void UpdateFloatingPosition()
    {
        Vector3 targ = transform.localPosition;
        targ.y = startY + Mathf.Sin(sin) * 0.025f;
        sin += Time.deltaTime * 1.1f;
        transform.localPosition = targ;
    }
    void Update()
    {
        if (isFloating)
        {
            UpdateFloatingPosition();
        }

        if (entityAttached)
        {
            Vector3 targ = transform.localPosition;
            entityAttached.transform.localPosition = targ;
        }
       
    }
    void Start()
    {
        tileView.tile = this;
        
    }
    public void SetBlock(bool v)
    {
        isBlock = v;
        collider.enabled = false;
        tileView.SetBlock(isBlock);
    }
    //public bool TileFree()
    //{
    //    return !isBlock && !hasCard;
    //}
    //public void SetCard(Card3D _card)
    //{
    //    card = _card;
    //    tileModel.card = card;
    //    hasCard = true;
    //    //tileView.entityAttached =
    //}

    public void SetData(CardDynamicData _cardDynamicData)
    {
        cardDynamicData = _cardDynamicData;
        tileModel.cardDynamicData = cardDynamicData;
        hasCard = true;
        collider.enabled = false;
        //tileView.entityAttached =
    }

    internal void SetZone(int v)
    {
        
        tileModel.zone = v;
        tileView.SetZone(v);
    }

    internal void SetFlag(int zone)
    {
        isBlock = true;
        isFlag = true;
        tileView.SetFlag();
    }
}

