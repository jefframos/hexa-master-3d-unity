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
    void Start()
    {
        maxInHandDefault = maxInHand;
        cardsDataManager = CardsDataManager.Instance;
        deckView = GetComponent<DeckView>();

        //cardsDataManager.allCards.level1[0];

        //inGameDeck = new InGameDeck()
        handDeck = new List<Card3D>();
        DeckLoading();

    }

    private void DeckLoading()
    {
        StartCoroutine(ShowDeck());
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
        int[] levels = new int[3];
        levels[0] = 4;
        levels[1] = 3;
        levels[2] = 3;
        deck = cardsDataManager.GetRandomDeck((uint)deckLenght, levels);
        currentCardID = 0;

        for (int i = 0; i < deck.Count; i++)
        {
            if(i >= maxInHand)
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
            Debug.Log(n);

            deckView.changeCardsInHandTotal(maxInHandDefault - n);
            return null;
        }
        CardStaticData data = deck[currentCardID];
        GameObject cardTransform = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, deckContainer);
        cardTransform.transform.localPosition = new Vector3(5f, -2.5f, 0);
        Card3D card = cardTransform.GetComponent<Card3D>();
        card.SetData(data, teamID);
        card.cardID = CARD_ID_COUNTER;
        CARD_ID_COUNTER++;
        currentCardID++;
        return card;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
