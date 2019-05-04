using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardStats
{
    public int hp;
    public int attack;
    public int defense;
    public int specAttack;
    public int specDefense;
    public int speed;
    public int range;
}
[System.Serializable]
public class CardStaticData
{
    public string name;
    public int number;
    public CardStats stats;
    public int level;
    public string weaknes;
    public string types;
    public string thumb_url;
    public string image_url;
    public string classType;
    public int race;
    public string folder = "thumbs";
}
