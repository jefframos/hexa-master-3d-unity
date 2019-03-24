using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardInput : MonoBehaviour
{
    public string col;
    public Tile currentTile;
    public Card3D currentCard;

    //public UnityEvent onTileOver = new UnityEvent<Tile>();

    [System.Serializable]
    public class TileEvent : UnityEvent<Tile>{};
    public TileEvent onTileOver = new TileEvent();

    [System.Serializable]
    public class CardEvent : UnityEvent<Card3D> { };
    public CardEvent onCardOver = new CardEvent();

    private List<int> collidableLayers;
    // Start is called before the first frame update
    void Start()
    {
        collidableLayers = new List<int>();
        collidableLayers.Add(1 << LayerMask.NameToLayer("BoardLayer"));
        collidableLayers.Add(1 << LayerMask.NameToLayer("DeckLayer"));
    }

    // Update is called once per frame
    void Update()
    {
       

        col = "testing";
    
        //{
        RaycastHit hit;

        Tile tempTile;
        Card3D tempCard;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        for (int i = 0; i < collidableLayers.Count; i++)
        {

            if (Physics.Raycast(ray, out hit, collidableLayers[i]))
            {
                //Debug.Log(collidableLayers[i]);
                if(i == 0)
                {
                    tempTile = hit.transform.GetComponent<Tile>();
                    if (!currentTile || tempTile.tileModel.id != currentTile.tileModel.id)
                    {
                        OnTileOver(tempTile);
                    }
                }
                else if (i == 1)
                {
                    //TO DO ARRUMAR ISSO AQUI
                    Debug.Log("PROBLEMA NO HIT DOS CARDS");
                    tempCard = hit.transform.GetComponent<Card3D>();
                    if (!currentCard && tempCard != null)
                    {
                        OnCardOver(tempCard);
                    }
                }

            }
        }

        
        //}

    }
    void OnCardOver(Card3D card)
    {
        if (currentCard)
        {
            currentCard.cardView.OnOut();
        }
        currentCard = card;
        currentCard.cardView.OnOver();
        //onCardOver.Invoke(currentCard);
        
    }
    void OnTileOver(Tile tile)
    {
        if (currentTile)
        {
            currentTile.tileView.OnOut();
        }
        currentTile = tile;
        col = currentTile.tileModel.i + "-" + currentTile.tileModel.j + "-" + currentTile.tileModel.id;

        currentTile.tileView.OnOver();
        onTileOver.Invoke(currentTile);
    }
}
