using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flex UI Data")]
public class FlexUIData : ScriptableObject
{
    public Sprite buttonSprite;
    public SpriteState buttonSpriteState;

    public Color defaultColor;
    public Color confirmColor;
    public Color declineColor;
    public Color warningColor;
}