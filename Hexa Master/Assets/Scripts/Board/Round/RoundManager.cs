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
        public SideType side;
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

    }
    void GetAttackLists(List<List<NeighborModel>> arroundsList, CardDynamicData currentCardDynamicData, out List<EnemiesAttackData> enemiesActiveList, out List<EnemiesAttackData> enemiesPassiveList)
    {
        enemiesActiveList = new List<EnemiesAttackData>();
        enemiesPassiveList = new List<EnemiesAttackData>();

        string debug = "currentcardopposites \n";

        for (int k = 0; k < currentCardDynamicData.oppositeSideList.Count; k++)
        {
            debug += currentCardDynamicData.oppositeSideList[k].ToString();
            debug += "\n";

        }

        Debug.Log("STUCK HERE, something wrong getting the enemies arround");

        for (int i = 0; i < arroundsList.Count; i++)
        {
            //for (int j = 0; j < arroundsList[i].Count; j++)
            for (int j = 0; j < arroundsList[i].Count; j++)
            {
                if (j == 0) continue;
                if (arroundsList[i][j].tile && arroundsList[i][j].tile.hasCard)
                {
                    if (DetectPossibleAttack(arroundsList[i][j].tile.tileModel.card, currentCardDynamicData))
                    {
                        EnemiesAttackData enemyData = new EnemiesAttackData();
                        enemyData.tile = arroundsList[i][j].tile;
                        enemyData.cardStatic = arroundsList[i][j].tile.tileModel.card.cardStaticData;
                        enemyData.cardDynamic = arroundsList[i][j].tile.tileModel.card.cardDynamicData;
                        enemyData.dist = arroundsList[i][j].distance;
                        enemyData.side = arroundsList[i][j].side;

                        Debug.Log(enemyData.side);
                        Debug.Log(enemyData.cardDynamic.sideList[0]);

                        bool isPassive = !enemyData.cardDynamic.sideList.Contains(enemyData.side);
                        //for (int k = 0; k < currentCardDynamicData.oppositeSideList.Count; k++)
                        //{
                        //    //debug += currentCardDynamicData.oppositeSideList[k].ToString();
                        //    //if (enemyData.cardDynamic.oppositeSideList.Contains(currentCardDynamicData.sideList[k]))
                        //    if (enemyData.cardDynamic.sideList.Contains(currentCardDynamicData.oppositeSideList[k]))
                        //    {
                        //        isPassive = false;
                        //        //break;
                        //    }

                        //}

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

        inGameHUD.DEBUG.text = debug;
    }

    internal bool DetectPossibleAttack(Card3D target, CardDynamicData currentCardDynamicData)
    {
        return target.cardDynamicData.teamID != currentCardDynamicData.teamID;

    }
}
