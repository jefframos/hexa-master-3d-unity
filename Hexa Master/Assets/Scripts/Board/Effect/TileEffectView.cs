using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileEffectView : MonoBehaviour
{
    public TextMeshPro text;

    public List<Color> colors;

    internal void ResetTile()
    {
        text.text = "";
    }

    internal void SetDistanceFactor(int distance)
    {
        text.text = "+" + distance;

        text.color = colors[Math.Min(distance - 2, colors.Count - 1)];
    }
}
