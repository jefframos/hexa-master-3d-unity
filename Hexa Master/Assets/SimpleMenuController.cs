using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMenuController : MonoBehaviour
{
    public GameObject collapseMenu;
    public Text buttonText;
    public void ToggleMenu()
    {
        if (collapseMenu.activeSelf)
        {
            collapseMenu.SetActive(false);
            buttonText.text = "SHOW";
        }
        else
        {
            collapseMenu.SetActive(true);
            buttonText.text = "HIDE";
        }
    }
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
