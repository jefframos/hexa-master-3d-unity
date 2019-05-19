using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAttackData
{
    public int dist;
    public Tile attacker;
    public Tile tile;
    public Card3D card;
    public NeighborModel neighborModel;
    public CardStaticData cardStatic;
    public CardDynamicData cardDynamic;
    public SideType sideAttack;
}
