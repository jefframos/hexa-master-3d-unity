using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeckInput : MonoBehaviour
{
    public string col;
    public Card3D currentCardOver;
    public Card3D currentCardSelected;
    private Card3D tempCard;

    public Camera deckCamera;

    [System.Serializable]
    public class CardEvent : UnityEvent<Card3D> { };
    public CardEvent onCardOver = new CardEvent();

    public CardEvent onCardOut = new CardEvent();

    public CardEvent onCardSelect = new CardEvent();

    private List<int> collidableLayers;
    private bool block = false;
    // Start is called before the first frame update
    void Start()
    {
        collidableLayers = new List<int>();
        collidableLayers.Add(1 << LayerMask.NameToLayer("DeckLayer"));
    }
    void Update()
    {
        

        if (block)
        {
            col = "block";
            return;
        }
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
                    if (Input.GetMouseButtonDown(0))
                    {
                        CardSelect(tempCard);
                    }
                        
                     OnCardOver(tempCard);
                   
                }
            }
        }

        if (tempCard == null)
        {
            NoCardOver();
        }


    }

    internal void SetUnblock(float v)
    {
        Invoke("SetUnblock", v);
    }

    public void SetUnblock()
    {
        block = false;
    }
    public void SetBlock()
    {
        block = true;
    }
    void CardSelect(Card3D card)
    {
        if(currentCardSelected != card)
        {
            currentCardSelected = card;

        }
        onCardSelect.Invoke(currentCardSelected);
    }
    void NoCardOver()
    {
        if (currentCardOver)
        {
            currentCardOver.cardView.OnOut();
            onCardOut.Invoke(currentCardOver);

        }
        currentCardOver = null;
    }
    void OnCardOver(Card3D card)
    {
        if (currentCardOver)
        {
            if (currentCardOver.cardID == card.cardID)
            {
                return;
            }
            onCardOut.Invoke(currentCardOver);
            currentCardOver.OnOut();
        }
        currentCardOver = card;
        currentCardOver.OnOver();
        onCardOver.Invoke(card);
    }


}
