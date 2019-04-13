using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    // Start is called before the first frame update

    public class InteractiveObjectEvent : UnityEvent<InteractiveObject> { };
    public InteractiveObjectEvent onClick = new InteractiveObjectEvent();
    public InteractiveObjectEvent onOver = new InteractiveObjectEvent();
    public InteractiveObjectEvent onOut = new InteractiveObjectEvent();
    internal bool isActive = true;
    bool isOver;
    bool tempOver;
    private List<int> collidableLayers;
    internal int id;
    void Start()
    {
        isOver = false;
        collidableLayers = new List<int>
        {
            1 << LayerMask.NameToLayer("Interactive")
        };
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //return;
        for (int i = 0; i < collidableLayers.Count; i++)
        {

            if (Physics.Raycast(ray, out hit, 100f, collidableLayers[i]))
            {
                tempOver = false;
                if (hit.transform == transform)
                {
                    tempOver = true;
                    MouseOver();

                    if (Input.GetMouseButtonDown(0))
                    {
                        Click();
                    }

                }
                if (!tempOver)
                {
                    MouseOut();
                }

            }
        }
    }
    private void Click()
    {
        onClick.Invoke(this);
    }
    private void MouseOver()
    {
        if (!isOver)
        {
            isOver = true;
            onOver.Invoke(this);
        }
    }
    private void MouseOut()
    {
        if (isOver)
        {
            isOver = false;
            onOut.Invoke(this);
        }
    }
}
