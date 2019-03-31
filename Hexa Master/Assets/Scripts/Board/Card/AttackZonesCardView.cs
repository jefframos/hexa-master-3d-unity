using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZonesCardView : MonoBehaviour
{
    [System.Serializable]
    public class AttackZoneViewData
    {
        public SideType type;
        public GameObject target;
    }
    public List<AttackZoneViewData> zones;
    public List<MeshRenderer> renderers;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    public void SetInGameMode()
    {
        renderers = new List<MeshRenderer>();
        for (int i = 0; i < zones.Count; i++)
        {
            AttackZoneViewData element = zones[i];
            renderers.Add(element.target.GetComponent<MeshRenderer>());

        }
        Color tartgColor = Color.white;
        tartgColor.a = 0.35f;
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.color = tartgColor;
        }
    }
    public void setZones(List<SideType> sides)
    {   
        for (int i = 0; i < zones.Count; i++)
        {
            AttackZoneViewData element = zones[i];
            if (sides.Contains(element.type))
            {
                element.target.SetActive(true);
            }
            else
            {
                element.target.SetActive(false);
            }

        }
    }

    internal void setDeckLayer()
    {
        //foreach (Transform child in GetComponentsInChildren<Transform>(true))
        //{
        //    child.gameObject.layer = LayerMask.NameToLayer("DeckLayer");  // add any layer you want. 
        //}
    }
}
