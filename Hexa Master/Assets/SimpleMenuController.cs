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
            //collapseMenu.transform
            collapseMenu.DOLocalMoveX(startPos.x - 300f, 0.5f).SetEase(Ease.InBack);
            //collapseMenu.localPosition = new Vector2(startPos.x - 300f, startPos.y);
            collapsed = false;
            buttonText.text = "SHOW";
        }
        else
        {
            collapseMenu.DOLocalMoveX(startPos.x, 0.5f).SetEase(Ease.OutBack);
            collapsed = true;
            buttonText.text = "HIDE";
        }
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
