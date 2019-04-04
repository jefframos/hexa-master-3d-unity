using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    // Start is called before the first frame update
    public DeckBuilder deckBuilder;
    public float cardsDistance = 2.5f;
    public float cardsRotation = -50f;
    public float cardsArc = -1.84f;
    public float cardScale = 1f;
    private int maxInHand = 5;
    public Card3D cardSelected;
    private Card3D cardInFocus;
    private Card3D cardInFocusOld;
    List<Card3D> handDeck;
    private float outTimer = 0;
    private bool blockMode = false;
    static float t = 0.0f;
    void Start()
    {
        handDeck = new List<Card3D>();
        blockMode = false;
    }

    internal void SetBlock()
    {
        blockMode = true;
    }

    internal void SetUnblock()
    {
        blockMode = false;
    }
    internal void SetUnblock(float v)
    {
        Invoke("SetUnblock", v);
    }

    void LateUpdate()
    {
        if (outTimer > 0 && cardInFocus)
        {
            outTimer -= Time.deltaTime;
            if (outTimer <= 0)
            {
                cardInFocus = null;
                cardInFocusOld = null;
            }
        }
        t = 0.15f;//+= 0.5f * Time.deltaTime;
        
        {
            for (int i = 0; i < handDeck.Count; i++)
            {
                if (cardInFocus && handDeck[i].cardID == cardInFocus.cardID)
                {
                }
                else
                {
                    StandardMode(i);
                }
            }
        }

        if (cardInFocus)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("HOLDING CARD");
            }
            Focus();
            //FocusMode();
        }
    }
    void Focus()
    {
        //if(cardSelected && cardSelected == cardInFocus)
        //{
        //    return;
        //}
        Card3D card = cardInFocus;
        float targetY = 1.5f;
        float focusDistance = cardsDistance * 0.8f;
        float targetScale = cardScale * 1.5f;
        float targetAngle = 0f;
        
        card.SetOrder(maxInHand + 1);

        Vector3 scale = Vector3.Lerp(card.transform.localScale, new Vector3(targetScale, targetScale, targetScale), t); ;
        card.transform.localScale = scale;

        Vector3 rot = card.transform.eulerAngles;
        float angle = Mathf.LerpAngle(card.transform.eulerAngles.z, targetAngle, t);
        rot.z = angle;
        card.transform.eulerAngles = rot;

        float sin = Mathf.Abs(Mathf.Sin(angle / 180 * Mathf.PI));
        Vector3 targetPosition = card.transform.localPosition;
        targetPosition.y = targetY;// sin * cardsArc + 0.5f;// + targetY;
        card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
    }
    void ReleaseCard()
    {

    }
    void HoldingCard()
    {

    }

    public void RemoveCurrentCard()
    {
        if (cardSelected)
        {
            handDeck.Remove(cardSelected);
            Card3D newCard = deckBuilder.GetCard();
            if (newCard)
            {
                handDeck.Add(newCard);
            }
            Debug.Log("REMOVE");
        }
    }

    internal void changeCardsInHandTotal(int v)
    {
        maxInHand = v;

        Debug.Log(maxInHand);
    }


    void StandardMode(int i, bool debug = false)
    {
        //return;
        Card3D card = handDeck[i];

        float addY = 0;
        int order = i;
        if (cardSelected && card.cardID == cardSelected.cardID)
        {
            addY = 0.8f;
            order = maxInHand;
        }
        if (blockMode)
        {
            addY = -2f;
        }
        float targetScale = cardScale;
        Vector3 scale = Vector3.Lerp(card.transform.localScale, new Vector3(targetScale, targetScale, targetScale), t); ;
        card.transform.localScale = scale;

        float tempCardsInHand = Mathf.Max((maxInHand - 1), 0);
        float tempRotation = 0;
        if(tempCardsInHand > 0)
        {
            tempRotation = cardsRotation / tempCardsInHand * (i) - (cardsRotation / 2);
        }
        
        float angle = Mathf.LerpAngle(card.transform.eulerAngles.z, tempRotation, t);
        float sin = Mathf.Abs(Mathf.Sin(angle / 180 * Mathf.PI));

        //Debug.Log(sin);
        //float adj
        Vector3 targetPosition = new Vector3(i * cardsDistance - (tempCardsInHand * cardsDistance / 2), addY, 0);
        targetPosition.y += sin * sin * cardsArc;// + sin * -5f;

        //Debug.Log(targetPosition);
        card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
        card.transform.eulerAngles = new Vector3(65f, 0, angle);

        
        card.SetOrder(order);
    }

    internal void SetHandCards(List<Card3D> deck, int max)
    {
        maxInHand = max;
        handDeck = deck;

        for (int i = 0; i < handDeck.Count; i++)
        {
            handDeck[i].transform.localPosition = new Vector3(0, -1f, 0);
        }
    }
    public void CardSelect(Card3D card)
    {
        if(cardSelected == card)
        {
            cardSelected.cardView.OnUnSelect();
            cardSelected = null;
        }
        else
        {
            if(cardSelected != null)
            {
                cardSelected.cardView.OnUnSelect();
            }
            cardSelected = card;
            cardSelected.cardView.OnSelect();
        }
        cardInFocus = null;


    }
    public void FocusCardOut(Card3D card)
    {
        outTimer = 0.75f;
    }
    public void FocusCardOn(Card3D card)
    {
        if (cardInFocusOld == card)
        {
            return;
        }
        cardInFocusOld = card;
        outTimer = 0;
        cardInFocus = card;
        //card.transform.localScale = new Vector3(2, 5f, 2);
    }
}
