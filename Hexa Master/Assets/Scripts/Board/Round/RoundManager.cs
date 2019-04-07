using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    //Start is called before the first frame update
    public InGameHUD inGameHUD;

    [System.Serializable]
    public class EnemiesAttackData
    {
        public int dist;
        public Tile tile;
        public Card3D card;
        public CardStaticData cardStatic;
        public CardDynamicData cardDynamic;
        public SideType sideAttack;
    }

    void Start()
    {
        inGameHUD.DEBUG.gameObject.SetActive(true);
    }

    //Update is called once per frame
    void Update()
    {

    }

    internal bool CanPlance(Tile tile, Card3D currentCard)
    {
        if (!currentCard)
        {
            return false;
        }
        return true;
    }

    internal void DoRound(Tile tile, NeighborsArroundModel currentNeighborsList, Card3D currentCard)
    {
        List<List<NeighborModel>> arroundsList = currentNeighborsList.GetCardArrounds(currentCard);
        CardDynamicData currentCardDynamicData = currentCard.cardDynamicData;
        GetAttackLists(arroundsList, currentCardDynamicData, out List<EnemiesAttackData> enemiesActiveList, out List<EnemiesAttackData> enemiesPassiveList);
        string debug = "";
        if (enemiesActiveList.Count > 0)
            debug += "Active List\n";
        for (int i = 0; i < enemiesActiveList.Count; i++)
        {
            enemiesActiveList[i].cardDynamic.teamID = currentCardDynamicData.teamID;
            enemiesActiveList[i].tile.entityAttached.ApplyTeamColor();
            Rebound(enemiesActiveList[i].tile);
            debug += enemiesActiveList[i].cardStatic.name;
            debug += "\n";
        }
        if (enemiesPassiveList.Count > 0)
            debug += "Passive List\n";
        for (int i = 0; i < enemiesPassiveList.Count; i++)
        {
            enemiesPassiveList[i].cardDynamic.teamID = currentCardDynamicData.teamID;
            enemiesPassiveList[i].tile.entityAttached.ApplyTeamColor();
            debug += enemiesPassiveList[i].cardStatic.name;
            debug += "\n";
        }

        inGameHUD.DEBUG.text = debug;

    }
    void GetAttackLists(List<List<NeighborModel>> arroundsList, CardDynamicData currentCardDynamicData, out List<EnemiesAttackData> enemiesActiveList, out List<EnemiesAttackData> enemiesPassiveList)
    {
        enemiesActiveList = new List<EnemiesAttackData>();
        enemiesPassiveList = new List<EnemiesAttackData>();

       
        bool found = false;
        for (int i = 0; i < arroundsList.Count; i++)
        {
            found = false;
            for (int j = 0; j < arroundsList[i].Count; j++)
            {
                //if (!found) continue;
                if (arroundsList[i][j].tile && arroundsList[i][j].tile.hasCard)
                {
                    if (!found && DetectPossibleAttack(arroundsList[i][j].tile.tileModel.card, currentCardDynamicData))
                    {
                        if(currentCardDynamicData.attackType == AttackType.AttackFirstFindOnly)
                        {
                            found = true;
                        }
                        EnemiesAttackData enemyData = new EnemiesAttackData
                        {
                            tile = arroundsList[i][j].tile,
                            cardStatic = arroundsList[i][j].tile.tileModel.card.cardStaticData,
                            cardDynamic = arroundsList[i][j].tile.tileModel.card.cardDynamicData,
                            dist = arroundsList[i][j].distance,
                            sideAttack = CardsDataManager.Instance.GetOppositeSide(arroundsList[i][j].side)
                        };

                        bool isPassive = !enemyData.cardDynamic.sideList.Contains(enemyData.sideAttack);          

                        if (isPassive)
                        {
                            enemiesPassiveList.Add(enemyData);
                        }
                        else
                        {
                            enemiesActiveList.Add(enemyData);

                        }
                    }

                }
            }
        }        
    }

    //internal List<Tile> Rebound(Tile tile)
    internal void Rebound(Tile tile)
    {
        NeighborsArroundModel currentNeighborsList = BoardController.Instance.GetNeighbours(tile.tileModel, 2);
        currentNeighborsList.AddListsOnBasedOnSideList(tile.entityAttached.cardDynamicData);
        List<NeighborModel> allArrounds = currentNeighborsList.GetAllEntitiesArroundOnly();
        for (int i = 0; i < allArrounds.Count; i++)
        {
            allArrounds[i].tile.entityAttached.cardDynamicData.teamID = tile.entityAttached.cardDynamicData.teamID;
            allArrounds[i].tile.entityAttached.ApplyTeamColor();
        }

    }

    internal bool DetectPossibleAttack(Card3D target, CardDynamicData currentCardDynamicData)
    {
        return target.cardDynamicData.teamID != currentCardDynamicData.teamID;

    }
}
