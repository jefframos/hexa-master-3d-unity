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
    void Start()
    {
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
        levels[0] = 1;
        levels[1] = 2;
        levels[2] = 3;
        List<CardStaticData> deck = cardsDataManager.GetRandomDeck(5, levels);

        for (int i = 0; i < deck.Count; i++)
        {
            CardStaticData data = deck[i];
            GameObject cardTransform = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, deckContainer);
            cardTransform.transform.localPosition = new Vector3(0, 0, 0);
            Card3D card = cardTransform.GetComponent<Card3D>();
            card.SetData(data);
            card.cardID = CARD_ID_COUNTER;
            CARD_ID_COUNTER++;

            handDeck.Add(card);
        }

        deckView.SetHandCards(handDeck, maxInHand);


    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
