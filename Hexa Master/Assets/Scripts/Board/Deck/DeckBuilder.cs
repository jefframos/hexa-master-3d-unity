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
    List<Card3D> handDeck;
    float timer = 1;
    public int maxInHand = 5;
    public float cardsDistance = 3f;
    public float cardsRotation = -50f;
    public float cardsaaaa = 4;
    void Start()
    {
        cardsDataManager = CardsDataManager.Instance;


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
            GameObject cardTransform = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, deckContainer);
            cardTransform.transform.localPosition = new Vector3(i * cardsDistance - ((maxInHand - 1) * cardsDistance / 2), 0, 0);
            Card3D card = cardTransform.GetComponent<Card3D>();
            card.SetData(data);

            //float tempRotation = cardsRotation / (maxInHand - 1) * (i) - (cardsRotation / 2);
            //Vector3 targetPosition = new Vector3(i * cardsDistance - ((maxInHand - 1) * cardsDistance / 2), 0, 0);
            //float sin = Mathf.Sin(tempRotation / 180 * Mathf.PI);
            //targetPosition.y += sin * 8;
            //Debug.Log(targetPosition + " - " + sin);
            //card.transform.localPosition = targetPosition;
            //card.transform.eulerAngles = new Vector3(0, 0, tempRotation); ;

            handDeck.Add(card);
            UpdateCardPosition(i);
        }


    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < handDeck.Count; i++)
        {
            UpdateCardPosition(i);

        }
    }
    void UpdateCardPosition(int i, bool debug = false)
    {
        Card3D card = handDeck[i];

        float tempRotation = cardsRotation / (maxInHand - 1) * (i) - (cardsRotation / 2);
        Vector3 targetPosition = new Vector3(i * cardsDistance - ((maxInHand - 1) * cardsDistance / 2), 0, 0);

        float sin = Mathf.Abs(Mathf.Sin(tempRotation / 180 * Mathf.PI));

        targetPosition.y += sin * cardsaaaa;

        card.transform.localPosition = targetPosition;
        card.transform.eulerAngles = new Vector3(0, 0, tempRotation);

        if (debug)
        {
            Debug.Log(targetPosition + " - " + sin);

        }

        card.SetOrder(i);
    }
}
