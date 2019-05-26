using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDynamicData
{

    internal CardStaticData cardStaticData;
    CardsDataManager cardsDataManager;


    List<Effector> effectsList = new List<Effector>();
    List<Effector> tempEffectsList = new List<Effector>();

    public float Attack { get => cardStaticData.stats.attack + effectAttack + distanceFactor; }
    public float Defense { get => cardStaticData.stats.defense + effectDefense; }
    public int Range { get => cardStaticData.stats.range + effectRange; }

    public float StaticAttack { get => cardStaticData.stats.attack; }
    public float StaticDefense { get => cardStaticData.stats.defense; }
    public int StaticRange { get => cardStaticData.stats.range; }

    float effectAttack = 0;
    public float EffectAttack { get => effectAttack; }
    float effectDefense = 0;
    public float EffectDefense { get => effectDefense; }
    int effectRange = 0;
    public int EffectRange { get => effectRange; }

    float previewAttack = 0;
    public float PreviewAttack { get => previewAttack + distanceFactor; }
    float previewDefense = 0;
    public float PreviewDefense { get => previewDefense; }
    int previewRange = 0;
    public int PreviewRange { get => previewRange; }

    private List<SideType> sideList;
    public List<SideType> SideList { get => sideList; set => sideList = value; }

    private List<SideType> oppositeSideList;
    public List<SideType> OppositeSideList { get => oppositeSideList; set => oppositeSideList = value; }

    private int teamID;
    public int TeamID { get => teamID; set => teamID = value; }

    private Color teamColor;
    internal Color TeamColor { get => teamColor; set => teamColor = value; }

    private List<BuffData> preAttackBuff;
    public List<BuffData> PreAttackBuff { get => preAttackBuff; set => preAttackBuff = value; }

    private List<BuffData> posAttackBuff;
    public List<BuffData> PosAttackBuff { get => posAttackBuff; set => posAttackBuff = value; }

    private ClassType classType;
    public ClassType ClassType { get => classType;}

    private AttackType attackType = AttackType.AttackFirstFindOnly;
    internal AttackType AttackType { get => attackType;}


    internal int distanceFactor;
    internal void AddPreviewTile(TileModel tileModel)
    {
        previewAttack = Attack;
        previewDefense = Defense;
        previewRange = Range;
        for (int i = 0; i < tileModel.effectsList.Count; i++)
        {
            previewAttack += tileModel.effectsList[i].attack;
            previewDefense += tileModel.effectsList[i].defense;
            previewRange += tileModel.effectsList[i].range;
        }
    }

    internal void AddEffect(List<Effector> effectList)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            AddEffect(effectList[i]);
        }
    }

    public void AddEffect(Effector effector)
    {
        effectsList.Add(effector);
        effectAttack = 0;
        effectDefense = 0;
        effectRange = 0;
        for (int i = 0; i < effectsList.Count; i++)
        {
            effectAttack += effectsList[i].attack;
            effectDefense += effectsList[i].defense;
            effectRange += effectsList[i].range;
        }
        //Debug.Log(effectDefense + " - " + StaticDefense);
    }
    public void SetData(CardStaticData _cardStaticData)
    {
        effectsList = new List<Effector>();
        TeamID = 1;
        cardsDataManager = CardsDataManager.Instance;
        cardStaticData = _cardStaticData;

        preAttackBuff = new List<BuffData>();
        posAttackBuff = new List<BuffData>();

        classType = ClassTypeHelper.GetClassByString(cardStaticData.classType);
        attackType = ClassTypeHelper.GetAttackType(classType);


        List<SideType> tempSideList = new List<SideType>
        {
            SideType.BottomLeft,
            SideType.BottomRight,
            SideType.Left,
            SideType.Right,
            SideType.TopLeft,
            SideType.TopRight
        };

        int maxSpdValue = 120;


        decimal tot = Math.Floor((decimal)cardStaticData.stats.speed / maxSpdValue * tempSideList.Count);

        if (tot > tempSideList.Count)
            tot = tempSideList.Count;
        else if (tot <= 0)
            tot = 1;


        SideList = new List<SideType>();
        OppositeSideList = new List<SideType>();
        ArrayUtils.Shuffle(tempSideList);

        for (var i = 0; i < tot; i++)
        {
            SideList.Add(tempSideList[i]);

            OppositeSideList.Add(cardsDataManager.GetOppositeSide(tempSideList[i]));
        };

    }

    internal void ApplyDistanceFactor(int distance)
    {
        //if uses distance
        if(distance <= 1 || ClassType != ClassType.Ranger)
        {
            distanceFactor = 0;
            return;
        }
        distanceFactor = distance * 10;
    }
}
