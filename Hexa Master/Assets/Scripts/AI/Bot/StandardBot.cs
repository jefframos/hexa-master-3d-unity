using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DeckInput))]
[RequireComponent(typeof(DeckView))]

public class StandardBot : MonoBehaviour
{
    DeckInput deckInput;
    DeckView deckView;
    BoardController boardController;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        deckInput = GetComponent<DeckInput>();
        deckInput.SetBlock();
        deckView = GetComponent<DeckView>();
        deckView.isBot = true;
        deckView.bot = this;
        boardController = BoardController.Instance;
        gameManager = GameManager.Instance;
    }
    internal void ChooseMove()
    {
        // deckView.HandDeck[0];
        // boardController.GetTile(2,2).
        //deckInput.onCardOver.Invoke(deckView.HandDeck[0]);
        deckInput.SetBlock();
        Card3D randomCard = deckView.HandDeck[Random.Range(0, deckView.HandDeck.Count)];
        gameManager.SetCurrentCard(randomCard);
        deckView.CardSelect(randomCard);

        gameManager.SelectTile(boardController.GetRandomEmpryTile());
        //deckView.RemoveCurrentCard();

    }
    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
