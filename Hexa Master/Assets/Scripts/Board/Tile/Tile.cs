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
    Card3D card;
    public void SetBlock(bool v)
    {
        isBlock = v;
        tileView.setBlock(isBlock);
    }
    public void SetCard(Card3D _card)
    {
        card = _card;
        tileModel.card = card;
        hasCard = true;
        //tileView.entityAttached =
    }
    
}

