
using UnityEngine;

[CreateAssetMenu(menuName = "Color Scheme")]
public class BoardColorScheme : ScriptableObject
{
    public Color[] Zones;
    public Color[] ZonesFlags;
    public Color[] Blocker;
    public Color[] Standard;

}

