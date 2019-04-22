using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileModel tileModel;
    public TileView tileView;
    public float rnd = 0;
    public bool isBlock = false;
    public bool hasCard = false;
    internal EntityView entityAttached;
    //internal Card3D card;
    internal CardDynamicData cardDynamicData;

    public bool IsAvailable { get => !isBlock && !hasCard;}

    public void Reset()
    {
        //entityAttached = null;
        //card = null;
        //tileModel = null;
        //tileView = null;
        //isBlock = false;
        //hasCard = false;
    }
    void Start()
    {
        tileView.tile = this;
    }
    public void SetBlock(bool v)
    {
        isBlock = v;
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
        //tileView.entityAttached =
    }

}

