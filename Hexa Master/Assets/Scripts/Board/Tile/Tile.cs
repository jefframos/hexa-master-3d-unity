using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileModel tileModel;
    public TileView tileView;
    public float startY = 0;
    internal bool isFlag;
    public bool hasCard = false;
    internal EntityView entityAttached;
    //internal Card3D card;
    internal CardDynamicData cardDynamicData;


    //List<Effector> effectsList = new List<Effector>();

    private Collider collider;
    public bool IsAvailable { get => !IsBlock && !hasCard; }
    public int TeamID { get => cardDynamicData.TeamID; }
    public int ZoneID { get => tileModel.zone; }
    public List<Effector> EffectList { get => tileModel.effectsList; }

    private bool isBlock = false;
    public bool IsBlock { get => isBlock; set => isBlock = value; }

    internal float sin;
    internal bool isFloating;
    public void ResetTile()
    {
        entityAttached = null;
        IsBlock = false;
        //tileModel = null;
        //tileView = null;
        isFlag = false;
        hasCard = false;

        tileModel.zone = -1;

        collider = GetComponent<Collider>();
        collider.enabled = true;
        tileView.ResetView();
        sin = 0;

        transform.localPosition = Vector3.zero;
        isFloating = false;
        tileModel.effectsList = new List<Effector>();

    }

    internal void RemoveTileEffectView()
    {
        tileView.RemoveTileEffectView();
    }

    internal void StartFloating(float _sin)
    {
        return;
        isFloating = true;
        sin = _sin;
        UpdateFloatingPosition();
    }
    internal void UpdateFloatingPosition()
    {
        Vector3 targ = transform.localPosition;
        targ.y = startY + Mathf.Sin(sin) * 0.025f;
        sin += Time.deltaTime * 1.1f;
        transform.localPosition = targ;
    }
    void Update()
    {
        return;
        if (isFloating)
        {
            UpdateFloatingPosition();
        }

        if (entityAttached)
        {
            Vector3 targ = transform.localPosition;
            entityAttached.transform.localPosition = targ;
        }
       
    }
    internal void SetNeighborModel(NeighborModel neighborModel, CardDynamicData cardDynamic)
    {
        cardDynamic.ApplyPreAttackBuff();
        bool buffed = false;

        int attackAcc = neighborModel.distance;

        if(cardDynamic.BuffAttack > 0)
        {
            attackAcc += (int)(cardDynamic.BuffAttack / 10f);
            buffed = true;
        }

        //tileView.SetDistanceEffect(2);
        if (!ClassTypeHelper.UseDistanceAsMultiplier(cardDynamic.ClassType) && !buffed)
        {
            return;
        }
        if (attackAcc > 1 )
        {
            tileView.SetDistanceEffect(attackAcc);
            tileView.debugID.text = "+" + attackAcc;

        }
    }
    internal void Highlight(Color teamColor)
    {        
        tileView.tileMarker.Highlight(teamColor);
    }

    internal void Highlight()
    {
        tileView.tileMarker.Highlight();

    }

    void Start()
    {
        tileView.tile = this;        
    }
    public void SetBlock(bool v)
    {
        IsBlock = v;
        collider.enabled = false;
        tileView.SetBlock(IsBlock);
    }

    public void SetData(CardDynamicData _cardDynamicData)
    {
        cardDynamicData = _cardDynamicData;
        tileModel.cardDynamicData = cardDynamicData;
        hasCard = true;
        //collider.enabled = false;
    }

 
    internal void SetZone(int v)
    {
        
        tileModel.zone = v;
        tileView.SetZone(v);
    }

    internal void SetFlag(int zone)
    {
        IsBlock = true;
        isFlag = true;
        tileView.SetFlag();
    }

    internal void AddEffect(Effector effector)
    {
        tileModel.effectsList.Add(effector);        
    }

    internal void ForceClear()
    {
        tileView.OnOut();
        tileView.debugID.text = "";

        if (entityAttached)
        {
            entityAttached.HideFeedback();
        }
      
    }

    internal void UpdateTile()
    {
        tileView.UpdateTile(tileModel);
       
       
    }
}

