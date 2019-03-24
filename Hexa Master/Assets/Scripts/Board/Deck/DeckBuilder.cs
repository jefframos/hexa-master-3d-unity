using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform deckContainer;
    public GameObject cardPrefab;
    CardsDataManager cardsDataManager;
    InGameDeck inGameDeck;
    float timer = 1;
    void Start()
    {
        cardsDataManager = CardsDataManager.Instance;


        //cardsDataManager.allCards.level1[0];

        //inGameDeck = new InGameDeck()
        DeckLoading();

    }

    private void DeckLoading()
    {
        StartCoroutine(ShowDeck());
    }

    private IEnumerator ShowDeck()
    {
        yield return new WaitForSeconds(0.15f);
        if(cardsDataManager.allCards.level5.Length == 0)
        {
            DeckLoading();
        }
        else
        {
            CreateCard();
        }

    }

    void CreateCard()
    {
        int[] levels = new int[3];
        levels[0] = 1;
        levels[1] = 2;
        levels[2] = 3;
        List<CardStaticData> deck = cardsDataManager.GetRandomDeck(5, levels);

        for (int i = 0; i < deck.Count; i++)
        {
            CardStaticData data = deck[i];
            GameObject cardTransform = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            cardTransform.transform.SetParent(deckContainer);
            Card cardView = cardTransform.GetComponent<Card>();
            cardView.SetData(data);
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
