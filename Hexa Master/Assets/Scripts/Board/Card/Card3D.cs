using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card3D : MonoBehaviour
{
    public GameCardView3D cardView;
    public CardStaticData cardStaticData;
    public CardDynamicData cardDynamicData;
    public uint cardID;
    private int orderAdd = 0;
    //public List<SideType> sides;
    //public List<SideType> oppositeSides;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetData(CardStaticData _cardStaticData, int teamID)
    {
        cardView = GetComponent<GameCardView3D>();

        cardDynamicData = new CardDynamicData();

        cardStaticData = _cardStaticData;
        cardDynamicData.SetData(cardStaticData);

        cardDynamicData.teamID = teamID;
        cardDynamicData.teamColor = GameConfig.Instance.GetTeamColor(teamID);
        //GameConfig.GetTeamColor(teamID);
        cardView.SetData(cardStaticData, cardDynamicData);


        //sides = cardDynamicData.sideList;
        //oppositeSides = cardDynamicData.oppositeSideList;
    }
    public void SetOrder(int order)
    {
        
        Vector3 orderPos = transform.localPosition;
        orderPos.z = -order * 0.2f;

        order += orderAdd;
        cardView.SetOrder(order);

        transform.localPosition = orderPos;
    }
    public void OnOut()
    {
        orderAdd = 0;
        cardView.OnOut();
    }
    public void OnOver()
    {
        orderAdd = 6;
        cardView.OnOver();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
