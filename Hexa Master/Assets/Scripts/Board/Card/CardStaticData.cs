using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CardStats
{
    public int hp = 100;
    public int attack = 70;
    public int defense = 60;
    public int specAttack = 60;
    public int specDefense = 60;
    public int speed = 60;
    public int range = 1;
}
[System.Serializable]
public class CardStaticData
{
    public string name = "ELF";
    public int number = 999;
    public CardStats stats = new CardStats();
    public int level = 2;
    public string weaknes;
    public string types;
    public string thumb_url = "elf-paladin.png";
    public string image_url = "elf-paladin.png";
    public string classType;
    public int race;
    public string folder = "new";
}
