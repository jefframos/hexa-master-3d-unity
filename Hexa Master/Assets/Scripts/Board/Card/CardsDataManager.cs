using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardsDataManager : Singleton<CardsDataManager>
{
    // Start is called before the first frame update
    public string gameDataFileName;
    public AllCards allCards;
    public InGameHUD InGameHUD;
    List<CardStaticData[]> allCardsList = null;// = new List<CardStaticData[]>();
    void Start()
    {
        LoadGameData();
    }

    private void LoadGameData()
    {
        JsonLoader jsonLoader = JsonLoader.Instance;

        string json = jsonLoader.LoadFromStreaming(gameDataFileName);

        if (json != null)
        {
            allCards = JsonUtility.FromJson<AllCards>(json);

        }
        else
        {
            allCards = new AllCards();
            allCards.CreateMock();
        }

    }

    public CardStaticData GetCardByID(int id)
    {
        if (allCardsList == null)
        {
            allCardsList = new List<CardStaticData[]>
            {
                allCards.level1,
                allCards.level2,
                allCards.level3,
                allCards.level4,
                allCards.level5
            };
        }

        for (int i = 0; i < allCardsList.Count; i++)
        {
            for (int j = 0; j < allCardsList[i].Length; j++)
            {
                if (allCardsList[i][j].number == id)
                {
                    return allCardsList[i][j];
                }
            }
        }

        return allCardsList[0][0];
    }
    public List<CardStaticData> GetDeckByType(uint tot, DeckType type)
    {
        List<CardStaticData> deck = new List<CardStaticData>();

        CardStaticData[] level1Copy;
        CardStaticData[] level2Copy;

        switch (type)
        {
            case DeckType.NONE:
                level1Copy = allCards.GetShuffleCopy(allCards.alliance);
                level2Copy = allCards.GetShuffleCopy(allCards.alliance);
                break;
            case DeckType.ALLIANCE:
                level1Copy = allCards.GetShuffleCopy(allCards.alliance);
                level2Copy = allCards.GetShuffleCopy(allCards.alliance);
                break;
            case DeckType.HORDE:
                level1Copy = allCards.GetShuffleCopy(allCards.horde);
                level2Copy = allCards.GetShuffleCopy(allCards.horde);
                break;
            case DeckType.HUMAN:
                level1Copy = allCards.GetShuffleCopy(allCards.human);
                level2Copy = allCards.GetShuffleCopy(allCards.human);
                break;
            case DeckType.DWARF:
                level1Copy = allCards.GetShuffleCopy(allCards.dwarf);
                level2Copy = allCards.GetShuffleCopy(allCards.dwarf);
                break;
            case DeckType.ELF:
                level1Copy = allCards.GetShuffleCopy(allCards.elf);
                level2Copy = allCards.GetShuffleCopy(allCards.elf);
                break;
            case DeckType.ORC:
                level1Copy = allCards.GetShuffleCopy(allCards.orc);
                level2Copy = allCards.GetShuffleCopy(allCards.orc);
                break;
            default:
                level1Copy = allCards.GetShuffleCopy(allCards.alliance);
                level2Copy = allCards.GetShuffleCopy(allCards.alliance);
                break;
        }

        List<CardStaticData[]> allCardsRandomList = new List<CardStaticData[]>
        {
            level1Copy,
            level2Copy
        };

        int aux = 0;
        for (int i = 0; i < tot; i++)
        {
            int id = Random.Range(0, allCardsRandomList.Count);
            CardStaticData[] tempArray = allCardsRandomList[id];
            aux = i;
            if (aux > tempArray.Length - 1)
            {
                aux = Random.Range(0, tempArray.Length);
            }
            deck.Add(tempArray[aux]);
        }

        return deck;
    }

    //public List<CardStaticData> GetAllianceDeck(uint tot)
    //{
    //    List<CardStaticData> deck = new List<CardStaticData>();

    //    CardStaticData[] level1Copy = allCards.GetShuffleCopy(allCards.alliance);
    //    CardStaticData[] level2Copy = allCards.GetShuffleCopy(allCards.alliance);

    //    Debug.Log(level1Copy.Length);

    //    List<CardStaticData[]> allCardsRandomList = new List<CardStaticData[]>
    //    {
    //        level1Copy,
    //        level2Copy
    //    };

    //    int aux = 0;
    //    for (int i = 0; i < tot; i++)
    //    {
    //        int id = Random.Range(0, allCardsRandomList.Count);
    //        CardStaticData[] tempArray = allCardsRandomList[id];
    //        aux = i;
    //        if (aux > tempArray.Length - 1)
    //        {
    //            aux = Random.Range(0, tempArray.Length);
    //        }
    //        deck.Add(tempArray[aux]);
    //    }

    //    return deck;
    //}

    //public List<CardStaticData> GetHordeDeck(uint tot)
    //{
    //    List<CardStaticData> deck = new List<CardStaticData>();

    //    CardStaticData[] level1Copy = allCards.GetShuffleCopy(allCards.horde);
    //    CardStaticData[] level2Copy = allCards.GetShuffleCopy(allCards.horde);
    //    CardStaticData[] level3Copy = allCards.GetShuffleCopy(allCards.horde);


    //    List<CardStaticData[]> allCardsRandomList = new List<CardStaticData[]>
    //    {
    //        level1Copy,
    //        level2Copy,
    //        level3Copy
    //    };

    //    int aux = 0;
    //    for (int i = 0; i < tot; i++)
    //    {
    //        int id = Random.Range(0, allCardsRandomList.Count);
    //        CardStaticData[] tempArray = allCardsRandomList[id];
    //        aux = i;
    //        if (aux > tempArray.Length - 1)
    //        {
    //            aux = Random.Range(0, tempArray.Length);
    //        }
    //        deck.Add(tempArray[aux]);
    //    }

    //    return deck;
    //}

    public List<CardStaticData> GetRandomDeck(uint tot, int[] levels)
    {

        List<CardStaticData> deck = new List<CardStaticData>();

        CardStaticData[] level1Copy = allCards.GetShuffleCopy(allCards.level1);
        CardStaticData[] level2Copy = allCards.GetShuffleCopy(allCards.level2);
        CardStaticData[] level3Copy = allCards.GetShuffleCopy(allCards.level3);
        CardStaticData[] level4Copy = allCards.GetShuffleCopy(allCards.level4);
        CardStaticData[] level5Copy = allCards.GetShuffleCopy(allCards.level5);

        List<CardStaticData[]> allCardsRandomList = new List<CardStaticData[]>
        {
            level1Copy,
            level2Copy,
            level3Copy,
            level4Copy,
            level5Copy
        };

        int aux = 0;
        for (int i = 0; i < tot; i++)
        {
            int id = levels[Random.Range(0, levels.Length)];
            CardStaticData[] tempArray = allCardsRandomList[id - 1];
            aux = i;
            if (aux > tempArray.Length - 1)
            {
                aux = Random.Range(0, tempArray.Length);
            }
            deck.Add(tempArray[aux]);
        }

        return deck;
    }

    public SideType GetOppositeSide(SideType side)
    {
        SideType oppSide = SideType.TopRight;
        switch (side)
        {
            case SideType.BottomLeft:
                oppSide = SideType.TopRight;
                break;
            case SideType.BottomRight:
                oppSide = SideType.TopLeft;
                break;
            case SideType.Left:
                oppSide = SideType.Right;
                break;
            case SideType.Right:
                oppSide = SideType.Left;
                break;
            case SideType.TopLeft:
                oppSide = SideType.BottomRight;
                break;
            case SideType.TopRight:
                oppSide = SideType.BottomLeft;
                break;
            default:
                break;
        }
        return oppSide;
    }
    // Update is called once per frame
    //void Update()
    //{

    //}

    internal bool IsPassiveAttack(CardDynamicData cardDynamic, SideType sideAttack)
    {
        return !cardDynamic.SideList.Contains(sideAttack);
    }
}
