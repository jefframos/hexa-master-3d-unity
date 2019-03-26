﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    // Start is called before the first frame update
    public float cardsDistance = 2.5f;
    public float cardsRotation = -50f;
    public float cardsArc = -1.84f;
    private int maxInHand = 5;
    private Card3D cardInFocus;
    private Card3D cardInFocusOld;
    List<Card3D> handDeck;
    private float outTimer = 0;
    static float t = 0.0f;
    void Start()
    {
        handDeck = new List<Card3D>();
    }
    void FocusMode()
    {
        for (int i = 0; i < handDeck.Count; i++)
        {
            Card3D card = handDeck[i];
            float targetY = -1f;
            float focusDistance = cardsDistance * 0.8f;
            float targetScale = 1f;
            float targetAngle = 0f;
            if (card.cardID == cardInFocus.cardID)
            {
                targetScale = 1.5f;
                targetY = 1f;
                card.SetOrder(maxInHand + 1);
            }
            else
            {
                targetScale = 0.85f;
                card.SetOrder(i);
            }
            Vector3 scale = Vector3.Lerp(card.transform.localScale, new Vector3(targetScale, targetScale, targetScale), t);;
            card.transform.localScale = scale;

            Vector3 rot = card.transform.eulerAngles;
            float angle = Mathf.LerpAngle(card.transform.eulerAngles.z, targetAngle, t);
            rot.z = angle;
            card.transform.eulerAngles = rot;

            float sin = Mathf.Abs(Mathf.Sin(angle / 180 * Mathf.PI));
            Vector3 targetPosition = new Vector3(i * focusDistance - ((maxInHand - 1) * focusDistance / 2), 0, 0);
            targetPosition.y += sin * cardsArc + targetY;
            card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        if(outTimer > 0 && cardInFocus)
        {
            outTimer -= Time.deltaTime;
            if(outTimer <= 0)
            {
                cardInFocus = null;
                cardInFocusOld = null;
            }
        }
        t = 0.2f;//+= 0.5f * Time.deltaTime;
        if (cardInFocus)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("HOLDING CARD");
            }
            FocusMode();
        }
        else
        {
            for (int i = 0; i < handDeck.Count; i++)
            {
                StandardMode(i);
            }
        }
    }
    void ReleaseCard()
    {

    }
    void HoldingCard()
    {

    }

    void StandardMode(int i, bool debug = false)
    {
        Card3D card = handDeck[i];

        float targetScale = 0.9f;
        Vector3 scale = Vector3.Lerp(card.transform.localScale, new Vector3(targetScale, targetScale, targetScale), t); ;
        card.transform.localScale = scale;


        float tempRotation = cardsRotation / (maxInHand - 1) * (i) - (cardsRotation / 2);
        float angle = Mathf.LerpAngle(card.transform.eulerAngles.z, tempRotation, t);
        float sin = Mathf.Abs(Mathf.Sin(angle / 180 * Mathf.PI));


        Vector3 targetPosition = new Vector3(i * cardsDistance - ((maxInHand - 1) * cardsDistance / 2), 0, 0);
        targetPosition.y += sin * cardsArc;
        card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
        card.transform.eulerAngles = new Vector3(0, 0, angle);

        if (debug)
        {
            Debug.Log(targetPosition + " - " + sin);

        }

        card.SetOrder(i);
    }

    internal void SetHandCards(List<Card3D> deck, int max)
    {
        maxInHand = max;
        handDeck = deck;
    }
    public void FocusCardOut(Card3D card)
    {
        outTimer = 0.75f;
    }
    public void FocusCardOn(Card3D card)
    {
        if(cardInFocusOld == card)
        {
            return;
        }
        cardInFocusOld = card;
        outTimer = 0;
        cardInFocus = card;
        //card.transform.localScale = new Vector3(2, 5f, 2);
    }
}
