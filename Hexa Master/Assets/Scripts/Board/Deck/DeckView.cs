using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class DeckStateConfig
    {
        public float cardsDistance = 1.2f;
        public float cardsRotation = -50f;
        public float cardsArc = -1.84f;
        public float cardScale = 1f;
        public float targetY = 0f;
        public Vector3 targetBlocker = new Vector3(0, -0.05f, 0.45f);
    }

    internal bool isBot;
    public DeckStateConfig standardState;
    public DeckStateConfig outState;
    public DeckStateConfig focusState;
    public DeckStateConfig selectedState;

    private DeckStateConfig currentState;

    public Transform blockTransform;

    private DeckInput deckInput;
    public DeckBuilder deckBuilder;

    private int maxInHand = 5;
    public Card3D cardSelected;
    private Card3D cardInFocus;
    private Card3D cardInFocusOld;
    private float outTimer = 0;
    private bool blockMode = false;
    //private bool overMode = false;
    static float t = 0.0f;
    List<Card3D> handDeck;
    internal StandardBot bot;

    public List<Card3D> HandDeck { get => handDeck; set => handDeck = value; }

    void Start()
    {
        HandDeck = new List<Card3D>();
        blockMode = false;

        deckInput = GetComponent<DeckInput>();

        deckInput.onBlockBoard.AddListener(DeckOut);
        deckInput.onUnblockBoard.AddListener(DeckOver);

        currentState = outState;

        t = 0.115f;
    }
    
    internal void DeckOver(Card3D card)
    {
        //overMode = true;
        currentState = outState;// standardState;
    }

    internal void DeckOut(Card3D card)
    {
        //overMode = false;
        currentState = standardState;// outState;
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
        
        {
            for (int i = 0; i < HandDeck.Count; i++)
            {
                if (cardInFocus && HandDeck[i].cardID == cardInFocus.cardID)
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
            Focus();
            blockTransform.localPosition = focusState.targetBlocker;
        }
        else
        {
            blockTransform.localPosition = currentState.targetBlocker;
        }
    }
    void Focus()
    {
        Card3D card = cardInFocus;
        float targetY = focusState.targetY;
        float focusDistance = focusState.cardsDistance;//cardsDistance * 0.8f;
        float targetScale = focusState.cardScale;//cardScale * 1.5f;
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
    //remove card from hand to board
    public void RemoveCurrentCard()
    {
        if (cardSelected)
        {
            HandDeck.Remove(cardSelected);
            Card3D newCard = deckBuilder.GetCard();
            if (newCard)
            {
                HandDeck.Add(newCard);
            }
        }
    }
    //change maximum cards in hand, used to calculate position
    internal void changeCardsInHandTotal(int v)
    {
        maxInHand = v;
    }


    void StandardMode(int i, bool debug = false)
    {
        Card3D card = HandDeck[i];       
        float addY = currentState.targetY;
        int order = i;
        float angleMult = 1f;
        float targetScale = currentState.cardScale;
        if (cardSelected && card.cardID == cardSelected.cardID)
        {
            addY = selectedState.targetY;
            order = maxInHand;
            angleMult = selectedState.cardsRotation;
            targetScale = selectedState.cardScale;
        }
        if (blockMode)
        {
            addY = -2f;
        }
        Vector3 scale = Vector3.Lerp(card.transform.localScale, new Vector3(targetScale, targetScale, targetScale), t); ;
        card.transform.localScale = scale;

        float tempCardsInHand = Mathf.Max((maxInHand - 1), 0);
        float tempRotation = 0;
        if(tempCardsInHand > 0)
        {
            tempRotation = currentState.cardsRotation / tempCardsInHand * (i) - (currentState.cardsRotation / 2);
            tempRotation *= angleMult;
        }
        
        float angle = Mathf.LerpAngle(card.transform.eulerAngles.z, tempRotation, t);
        float sin = Mathf.Abs(Mathf.Sin(angle / 180 * Mathf.PI));

        float tempDistance = currentState.cardsDistance;
        Vector3 targetPosition = new Vector3(i * tempDistance - (tempCardsInHand * tempDistance / 2), addY, 0);
        targetPosition.y += sin * sin * currentState.cardsArc;

        card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
        card.transform.eulerAngles = new Vector3(65f, 0, angle);        
        card.SetOrder(order);
    }

    internal void SetHandCards(List<Card3D> deck, int max)
    {
        maxInHand = max;
        HandDeck = deck;

        for (int i = 0; i < HandDeck.Count; i++)
        {
            HandDeck[i].transform.localPosition = new Vector3(0, -1f, 0);
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
        outTimer = 0.25f;
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
    }
}
