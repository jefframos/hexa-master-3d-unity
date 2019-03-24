using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    GameCardView cardView;
    public CardStaticData cardStaticData;
    public CardDynamicData cardDynamicData;

    //public List<SideType> sides;
    //public List<SideType> oppositeSides;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetData(CardStaticData _cardStaticData)
    {
        cardView = GetComponent<GameCardView>();
        cardDynamicData = new CardDynamicData();

        cardStaticData = _cardStaticData;
        cardView.SetData(cardStaticData);

        cardDynamicData.SetData(cardStaticData);

        //sides = cardDynamicData.sideList;
        //oppositeSides = cardDynamicData.oppositeSideList;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
