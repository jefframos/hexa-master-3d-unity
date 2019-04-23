using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class FlexUIButton : FlexUI
{

    protected Button button;
    protected Image image;
    public Image icon;
    public TextMeshProUGUI label;

    public ButtonType buttonType;
    public string text;
    public Sprite iconSprite;

    public enum ButtonType
    {
        Default,
        Confirm,
        Decline,
        Warning
    }

    public override void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        //list = GetComponentInChildren<HorizontalLayoutGroup>();

        //if (list)
        //{
        //    Debug.Log("FOUND LIST");
        //    Transform _icon = list.transform.Find("Icon");
        //    if (_icon)
        //    {
        //        icon = _icon.GetComponent<Image>();
        //    }

        //    Transform _label = list.transform.Find("Label");
        //    if (_label)
        //    {
        //        label = _label.GetComponent<TextMeshProUGUI>();
        //        Debug.Log("FOUND LABEL");
        //    }
        //}
       

        base.Awake();
    }

    protected override void OnSkinUI()
    {
        button.transition = Selectable.Transition.SpriteSwap;
        button.targetGraphic = image;

        image.sprite = skinData.buttonSprite;
        image.type = Image.Type.Sliced;
        button.spriteState = skinData.buttonSpriteState;

        if (icon)
        {
            if (!iconSprite)
            {
                icon.gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = iconSprite;
            }
        }

        if (label)
        {
            if (text == "")
            {
                label.gameObject.SetActive(false);
            }
            else
            {
                label.text = text;
            }
            
        }

        switch (buttonType)
        {
            case ButtonType.Confirm:
                image.color = skinData.confirmColor;
                break;
            case ButtonType.Decline:
                image.color = skinData.declineColor;
                break;
            case ButtonType.Default:
                image.color = skinData.defaultColor;
                break;
            case ButtonType.Warning:
                image.color = skinData.warningColor;
                break;
        }


        base.OnSkinUI();
    }



}