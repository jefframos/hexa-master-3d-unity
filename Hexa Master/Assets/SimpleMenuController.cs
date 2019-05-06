using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SimpleMenuController : MonoBehaviour
{
    public Transform collapseMenu;
    public Text buttonText;
    public bool collapsed = true;
    Vector2 startPos;
    public void ToggleMenu()
    {
        if (collapsed)
        {
            Hide();
        }
        else
        {
            Unhide();
        }
    }
    internal void Hide()
    {
        collapseMenu.DOLocalMoveX(startPos.x - 300f, 0.5f).SetEase(Ease.InBack);
        collapsed = false;
        buttonText.text = "SHOW";
    }

    internal void Unhide()
    {
        collapseMenu.DOLocalMoveX(startPos.x, 0.5f).SetEase(Ease.OutBack);
        collapsed = true;
        buttonText.text = "HIDE";
    }
    // Start is called before the first frame update
    void Start()
    {
        startPos = collapseMenu.localPosition;


    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
