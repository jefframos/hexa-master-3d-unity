using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    
    public static uint CARD_ID_COUNTER = 0;
    // Start is called before the first frame update
    DeckView deckView;
    public Transform deckContainer;
    public GameObject cardPrefab;
    CardsDataManager cardsDataManager;
    List<Card3D> handDeck;
    public int maxInHand = 5;
    public int deckLenght = 15;
    public int teamID = 1;
    private int maxInHandDefault = 5;
    private int currentCardID = 0;
    List<CardStaticData> deck;
    public List<int> starterIDS;
    public int[] levels;
    public bool ignoreStarters = false;

    internal PlayerData playerData;

    internal void InitDeck(PlayerData _playerData)
    {
        playerData = _playerData;
        teamID = playerData.teamID;// - 1;
        handDeck = new List<Card3D>();
        maxInHandDefault = maxInHand;
        cardsDataManager = CardsDataManager.Instance;
        deckView = GetComponent<DeckView>();
        DeckLoading();
    }

    private void DeckLoading()
    {
        CreateCards();
        //StartCoroutine(ShowDeck());
    }

    private IEnumerator ShowDeck()
    {
        yield return new WaitForSeconds(0.15f);
        if (cardsDataManager.allCards.level5.Length == 0)
        {
            DeckLoading();
        }
        else
        {
            CreateCards();
        }

    }

    void CreateCards()
    {
        //Debug.Log("CREATE CARDS");

        if (levels.Length <= 0)
        {
            int[] levels = new int[3];
            levels[0] = 2;
            levels[1] = 3;
            levels[2] = 4;

        }

        if (!ignoreStarters)
        {
            deck = cardsDataManager.GetRandomDeck((uint)(deckLenght - starterIDS.Count), levels);
            for (int i = 0; i < starterIDS.Count; i++)
            {
                deck.Insert(0, cardsDataManager.GetCardByID(starterIDS[i]));
            }
        }
        else
        {
            playerData.LoadDeck(12);
            deck = playerData.playerInGameDeck;
            ArrayUtils.Shuffle(deck);
        }


        if (deck.Count < 12)
        {
            Debug.Log(deckLenght);
            Debug.Log(levels);
            Debug.Log("ERROR ON DECK");

        }
        currentCardID = 0;


        for (int i = 0; i < deck.Count; i++)
        {
            if (i >= maxInHand)
            {
                break;
            }
            Card3D card = GetCard();
            handDeck.Add(card);
        }

        deckView.SetHandCards(handDeck, maxInHand);
    }

    public Card3D GetCard()
    {

        if (currentCardID >= deck.Count)
        {
            currentCardID++;
            int n = currentCardID - deck.Count;
            deckView.changeCardsInHandTotal(maxInHandDefault - n);
            return null;
        }
        CardStaticData data = deck[currentCardID];

        GameObject cardGo = GamePool.Instance.GetCard();
        cardGo.SetActive(true);
        cardGo.transform.SetParent(deckContainer);
        cardGo.transform.localPosition = new Vector3(5f, -2.5f, 0);
        Card3D card = cardGo.GetComponent<Card3D>();
        card.SetData(data, teamID);
        card.cardID = CARD_ID_COUNTER;
        CARD_ID_COUNTER++;
        currentCardID++;
        return card;
    }

    internal void DestroyDeck()
    {
        if (deckView)
        {
            deckView.DestroyCards();

        }
        //throw new NotImplementedException();
    }


}
