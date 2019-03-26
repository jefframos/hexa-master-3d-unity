using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeckInput : MonoBehaviour
{
    public string col;
    public Card3D currentCard;
    public Camera deckCamera;

    [System.Serializable]
    public class CardEvent : UnityEvent<Card3D> { };
    public CardEvent onCardOver = new CardEvent();

    public CardEvent onCardOut = new CardEvent();

    private List<int> collidableLayers;
    private Card3D tempCard;
    // Start is called before the first frame update
    void Start()
    {
        collidableLayers = new List<int>();
        collidableLayers.Add(1 << LayerMask.NameToLayer("DeckLayer"));
    }
    void Update()
    {
        col = "testing";

        //{
        RaycastHit hit;

        tempCard = null;

        var ray = deckCamera.ScreenPointToRay(Input.mousePosition);


        for (int i = 0; i < collidableLayers.Count; i++)
        {

            if (Physics.Raycast(ray, out hit, collidableLayers[i]))
            {

                //TO DO ARRUMAR ISSO AQUI
                tempCard = hit.transform.GetComponentInChildren<Card3D>();

                if (tempCard != null)
                {
                    if (true)
                    {
                        OnCardOver(tempCard);
                    }
                }
            }
        }

        if (tempCard == null)
        {
            NoCardOver();
        }


    }
    void NoCardOver()
    {
        if (currentCard)
        {
            currentCard.cardView.OnOut();
            onCardOut.Invoke(currentCard);

        }
        currentCard = null;
    }
    void OnCardOver(Card3D card)
    {
        if (currentCard)
        {
            if (currentCard.cardID == card.cardID)
            {
                return;
            }
            onCardOut.Invoke(currentCard);
            currentCard.OnOut();
        }
        currentCard = card;
        currentCard.OnOver();
        onCardOver.Invoke(card);
    }


}
