using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //in future make the player proper pack
    internal List<CardStaticData> playerPack;
    internal List<CardStaticData> playerInGameDeck;
    internal DeckType deckType;
    internal int teamID;
    internal Color teamColor;
    CardsDataManager cardsDataManager;
    int attack = 0;
    public int Attack { get  {
            if(attack <= 0)
            {
                for (int i = 0; i < playerInGameDeck.Count; i++)
                {
                    attack += playerInGameDeck[i].stats.attack / 10;
                }
            }
            return attack;
        } }

    int defense = 0;
    public int Defense
    {
        get
        {
            if (defense <= 0)
            {
                for (int i = 0; i < playerInGameDeck.Count; i++)
                {
                    defense += playerInGameDeck[i].stats.defense / 10;
                }
            }
            return defense;
        }
    }

    int agility = 0;
    public int Agility
    {
        get
        {
            if (agility <= 0)
            {
                for (int i = 0; i < playerInGameDeck.Count; i++)
                {
                    agility += playerInGameDeck[i].stats.speed / 10;
                }
            }
            return agility;
        }
    }
    
    // Start is called before the first frame update
    internal void LoadDeck(uint deckLenght)
    {

        teamColor = GameConfig.Instance.GetTeamColor(teamID - 1);

        cardsDataManager = CardsDataManager.Instance;
        int[] levels = new int[3];
        levels[0] = 2;
        levels[1] = 3;
        levels[2] = 4;

        playerInGameDeck = cardsDataManager.GetDeckByType((uint)deckLenght, deckType);

        //switch (deckType)
        //{
        //    case DeckType.NONE:
        //        playerInGameDeck = cardsDataManager.GetRandomDeck((uint)deckLenght, levels);
        //        break;
        //    case DeckType.ALLIANCE:
        //        playerInGameDeck = cardsDataManager.GetAllianceDeck((uint)deckLenght);
        //        break;
        //    case DeckType.HORDE:
        //        playerInGameDeck = cardsDataManager.GetHordeDeck((uint)deckLenght);
        //        break;
        //    default:
        //        playerInGameDeck = cardsDataManager.GetAllianceDeck((uint)deckLenght);
        //        break;
        //}
    }
}
