using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileModel tileModel;
    public TileView tileView;
    Card3D card;

    public void SetCard(Card3D _card)
    {
        card = _card;
        tileModel.card = card;
    }
}

