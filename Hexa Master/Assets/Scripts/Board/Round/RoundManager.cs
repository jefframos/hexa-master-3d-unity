using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    //Start is called before the first frame update
    public InGameHUD inGameHUD;
    List<CommandDefault> roundCommands;

    public class RoundEvent : UnityEvent<List<CommandDefault>> { };
    public RoundEvent onRoundReady = new RoundEvent();

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
        roundCommands = new List<CommandDefault>();
        List<List<NeighborModel>> arroundsList = currentNeighborsList.GetCardArrounds(currentCard);
        CardDynamicData currentCardDynamicData = currentCard.cardDynamicData;
        CardStaticData currentCardStaticData = currentCard.cardStaticData;
        GetAttackLists(arroundsList, currentCardDynamicData, out List<EnemiesAttackData> enemiesActiveList, out List<EnemiesAttackData> enemiesPassiveList);

        if (enemiesActiveList.Count > 8)
        {

        }
        else
        {
            for (int i = 0; i < enemiesPassiveList.Count; i++)
            {
                roundCommands.Add(AddPassiveAttackCommand(enemiesPassiveList[i], currentCardDynamicData.teamID));
            }


            for (int i = 0; i < enemiesActiveList.Count; i++)
            {
                if (enemiesActiveList[i].cardStatic.stats.defense < currentCardStaticData.stats.attack)
                {
                    enemiesActiveList[i].cardDynamic.teamID = currentCardDynamicData.teamID;
                    roundCommands.Add(AddAttackCommand(enemiesActiveList[i], currentCardDynamicData.teamID));
                    roundCommands.Add(AddReboundCommand(enemiesActiveList[i].tile, currentCardDynamicData.teamID));
                }
                else
                {
                    //enemiesActiveList[i].cardDynamic.teamID = currentCardDynamicData.teamID;
                    currentCardDynamicData.teamID = enemiesActiveList[i].cardDynamic.teamID;

                    EnemiesAttackData selfData = new EnemiesAttackData
                    {
                        tile = tile,
                        cardStatic = currentCardStaticData,
                        cardDynamic = currentCardDynamicData,
                        dist = 1,
                        sideAttack = SideType.BottomLeft
                    };

                    roundCommands.Add(AddAttackCommand(selfData, enemiesActiveList[i].cardDynamic.teamID));
                    roundCommands.Add(AddReboundCommand(selfData.tile, enemiesActiveList[i].cardDynamic.teamID));
                }
            }

            Debug.Log("ROUND DONE HERE");
            onRoundReady.Invoke(roundCommands);
        }
        

    }
    private CommandDefault AddReboundCommand(Tile tile, int teamID)
    {
        List<NeighborModel> allArrounds = Rebound(tile, teamID);
        CommandRebound.CommandReboundData data = new CommandRebound.CommandReboundData
        {
            allArrounds = allArrounds,
            teamTarget = teamID
        };

        CommandRebound command = new CommandRebound();
        command.SetData(data);
        return command;
    }

    private CommandDefault AddAttackCommand(EnemiesAttackData enemieData, int teamTarget)
    {
        CommandAttack.CommandAttackData data = new CommandAttack.CommandAttackData
        {
            attackData = enemieData,
            teamTarget = teamTarget,
            attackType = AttackType.Active
        };

        CommandAttack command = new CommandAttack();
        command.SetData(data);
        return command;
    }

    private CommandDefault AddPassiveAttackCommand(EnemiesAttackData enemieData, int teamTarget)
    {
        CommandAttack.CommandAttackData data = new CommandAttack.CommandAttackData
        {
            attackData = enemieData,
            teamTarget = teamTarget,
            attackType = AttackType.Passive
        };

        CommandAttack command = new CommandAttack();
        command.SetData(data);
        return command;
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
    internal List<NeighborModel> Rebound(Tile tile, int id)
    {
        NeighborsArroundModel currentNeighborsList = BoardController.Instance.GetNeighbours(tile.tileModel, 2);
        currentNeighborsList.AddListsOnBasedOnSideList(tile.entityAttached.cardDynamicData);
        List<NeighborModel> allArrounds = currentNeighborsList.GetAllEntitiesArroundOnly();
        for (int i = 0; i < allArrounds.Count; i++)
        {
            allArrounds[i].tile.entityAttached.cardDynamicData.teamID = id;// tile.entityAttached.cardDynamicData.teamID;
            //allArrounds[i].tile.entityAttached.ApplyTeamColor();
        }
        return allArrounds;
    }

    internal bool DetectPossibleAttack(Card3D target, CardDynamicData currentCardDynamicData)
    {
        return target.cardDynamicData.teamID != currentCardDynamicData.teamID;

    }
}
