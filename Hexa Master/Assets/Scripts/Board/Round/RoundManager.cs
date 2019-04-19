﻿using System;
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

    public class MultipleAttackEvent : UnityEvent<List<EnemiesAttackData>> { };

    public MultipleAttackEvent onMultipleAttack = new MultipleAttackEvent();

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

    internal void DoRound(Tile tile, NeighborsArroundModel currentNeighborsList, CardDynamicData cardDynamicData)
    {
        roundCommands = new List<CommandDefault>();
        List<List<NeighborModel>> arroundsList = currentNeighborsList.GetCardArrounds(cardDynamicData);

        if (arroundsList == null || arroundsList.Count == 0)
        {
            onRoundReady.Invoke(roundCommands);
            return;
        }
        CardDynamicData currentCardDynamicData = cardDynamicData;
        CardStaticData currentCardStaticData = cardDynamicData.cardStaticData;
        GetAttackLists(arroundsList, currentCardDynamicData, out List<EnemiesAttackData> enemiesActiveList, out List<EnemiesAttackData> enemiesPassiveList);

        for (int i = 0; i < enemiesPassiveList.Count; i++)
        {
            roundCommands.Add(AddPassiveAttackCommand(enemiesPassiveList[i], currentCardDynamicData.teamID, tile));
        }
        if (enemiesActiveList.Count > 0)
        {
            if (enemiesActiveList.Count <= 1)
            {
                //onMultipleAttack.Invoke(enemiesActiveList);
                GenerateRoundCommands(enemiesActiveList[0], cardDynamicData, tile);
                onRoundReady.Invoke(roundCommands);
            }
            else
            {
                onMultipleAttack.Invoke(enemiesActiveList);
            }
        }
        else
        {
            onRoundReady.Invoke(roundCommands);
        }
    }
    public void GenerateRoundCommands(List<EnemiesAttackData> targets, CardDynamicData cardDynamicData, Tile tile)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            GenerateRoundCommands(targets[i], cardDynamicData, tile);
        }

        onRoundReady.Invoke(roundCommands);
    }
    /// Generate Command List for the target    
    public void GenerateRoundCommands(EnemiesAttackData targetAttack, CardDynamicData cardDynamicData, Tile tile)
    {
        CardDynamicData currentCardDynamicData = cardDynamicData;
        CardStaticData currentCardStaticData = cardDynamicData.cardStaticData;

        if (targetAttack.cardDynamic.teamID == currentCardDynamicData.teamID)
        {
            return;
        }

        if (targetAttack.cardStatic.stats.defense < currentCardStaticData.stats.attack)
        {
            targetAttack.cardDynamic.teamID = currentCardDynamicData.teamID;
            roundCommands.Add(AddAttackCommand(targetAttack, currentCardDynamicData.teamID, tile));
            roundCommands.Add(AddReboundCommand(targetAttack.tile, currentCardDynamicData.teamID));
        }
        else if (targetAttack.dist <= 1)
        {
            //enemiesActiveList[i].cardDynamic.teamID = currentCardDynamicData.teamID;
            currentCardDynamicData.teamID = targetAttack.cardDynamic.teamID;

            EnemiesAttackData selfData = new EnemiesAttackData
            {
                tile = tile,
                cardStatic = currentCardStaticData,
                cardDynamic = currentCardDynamicData,
                dist = targetAttack.dist,
                sideAttack = SideType.BottomLeft
            };
            roundCommands.Add(AddMockAttackCommand(targetAttack, currentCardDynamicData.teamID, tile));

            roundCommands.Add(AddAttackCommand(selfData, targetAttack.cardDynamic.teamID, targetAttack.tile));
            roundCommands.Add(AddReboundCommand(selfData.tile, targetAttack.cardDynamic.teamID, true));

            //if reach here, should stop the others

        }
        else
        {
            roundCommands.Add(AddAttackCommand(targetAttack, currentCardDynamicData.teamID, tile, true));
        }

    }

    #region Commands
    private CommandDefault AddReboundCommand(Tile tile, int teamID, bool debug = false)
    {
        List<NeighborModel> allArrounds = Rebound(tile, teamID, debug);
        CommandRebound.CommandReboundData data = new CommandRebound.CommandReboundData
        {
            allArrounds = allArrounds,
            teamTarget = teamID
        };

        CommandRebound command = new CommandRebound();
        command.SetData(data);
        return command;
    }

    private CommandDefault AddMockAttackCommand(EnemiesAttackData enemyData, int teamTarget, Tile attacker)
    {
        enemyData.attacker = attacker;
        CommandAttack.CommandAttackData data = new CommandAttack.CommandAttackData
        {
            attackData = enemyData,
            teamTarget = teamTarget,
            attackType = AttackType.Active,
            entityAttack = attacker.entityAttached,
            entityDefense = enemyData.tile.entityAttached,
            isCounter = true,
            isMock = true

        };

        CommandAttack command = new CommandAttack();
        command.SetData(data);
        return command;
    }

    private CommandDefault AddAttackCommand(EnemiesAttackData enemyData, int teamTarget, Tile attacker, bool isBlock = false)
    {
        enemyData.attacker = attacker;
        CommandAttack.CommandAttackData data = new CommandAttack.CommandAttackData
        {
            attackData = enemyData,
            teamTarget = teamTarget,
            attackType = AttackType.Active,
            entityAttack = attacker.entityAttached,
            entityDefense = enemyData.tile.entityAttached,
            isBlock = isBlock

        };

        CommandAttack command = new CommandAttack();
        command.SetData(data);
        return command;
    }

    private CommandDefault AddPassiveAttackCommand(EnemiesAttackData enemyData, int teamTarget, Tile attacker)
    {
        enemyData.attacker = attacker;
        CommandAttack.CommandAttackData data = new CommandAttack.CommandAttackData
        {
            attackData = enemyData,
            teamTarget = teamTarget,
            attackType = AttackType.Passive,
            entityAttack = attacker.entityAttached,
            entityDefense = enemyData.tile.entityAttached
        };

        CommandAttack command = new CommandAttack();
        command.SetData(data);
        return command;
    }
    #endregion

    #region get lists
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
                    if (!found && DetectPossibleAttack(arroundsList[i][j], currentCardDynamicData))
                    {
                        if (currentCardDynamicData.attackType == AttackType.AttackFirstFindOnly)
                        {
                            found = true;
                        }
                        EnemiesAttackData enemyData = new EnemiesAttackData
                        {
                            tile = arroundsList[i][j].tile,
                            cardStatic = arroundsList[i][j].tile.tileModel.cardDynamicData.cardStaticData,
                            cardDynamic = arroundsList[i][j].tile.tileModel.cardDynamicData,
                            dist = arroundsList[i][j].distance,
                            sideAttack = CardsDataManager.Instance.GetOppositeSide(arroundsList[i][j].side)
                        };

                        bool isPassive = CardsDataManager.Instance.IsPassiveAttack(enemyData.cardDynamic, enemyData.sideAttack);
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
    internal List<NeighborModel> Rebound(Tile tile, int teamID, bool debug = false)
    {
        NeighborsArroundModel currentNeighborsList = BoardController.Instance.GetNeighbours(tile.tileModel, 2, true);

        if (BoardController.Instance.debugging && BoardController.Instance.debugging2)
        {
            Debug.Log(currentNeighborsList);
        }


        currentNeighborsList.AddListsOnBasedOnSideList(tile.entityAttached.cardDynamicData);


        List<NeighborModel> allArrounds = currentNeighborsList.GetAllEntitiesArroundOnly();

        //Debug.Log("REVIEW ESSE REBOUND, NAO FUNCIONA SEMPRE =/ " + tile.entityAttached.cardStaticData.name + " - "+allArrounds.Count);

        if(allArrounds.Count == 0)
        {
            Debug.Log(currentNeighborsList);
            //tile.tileView.debugID
        }

        if (BoardController.Instance.debugging && BoardController.Instance.debugging2 || debug)
        {
            Debug.Log(currentNeighborsList);
        }

        for (int i = 0; i < allArrounds.Count; i++)
        {
            Vector3 pos = allArrounds[i].tile.transform.localPosition;
            //pos.y = 1.5f;
            allArrounds[i].tile.transform.localPosition = pos;

            Debug.Log("-" + allArrounds[i].tile.cardDynamicData.cardStaticData.name + " - " + allArrounds[i].tile.cardDynamicData.teamID + " - to: - " + teamID);


            allArrounds[i].tile.cardDynamicData.teamID = teamID;// tile.entityAttached.cardDynamicData.teamID;
            //allArrounds[i].tile.entityAttached.ApplyTeamColor();
        }
        return allArrounds;
    }
    #endregion
    internal bool DetectPossibleAttack(NeighborModel neighborModel, CardDynamicData currentCardDynamicData)
    {
        CardDynamicData cardDynamicData = neighborModel.tile.tileModel.cardDynamicData;

        if(neighborModel.distance > currentCardDynamicData.cardStaticData.stats.range)
        {
            return false;
        }

        return cardDynamicData.teamID != currentCardDynamicData.teamID;

    }
}
