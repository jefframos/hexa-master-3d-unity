using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public BoardInput boardInput;
    public DeckInput deckInput;
    public DeckView deckView;
    BoardController boardController;
    public NeighborsArroundModel currentNeighborsList;
    public Tile currentTile;
    Card3D currentCard;
    private bool acting = false;
    public List<GameObject> decksTransform;
    private int currentTeam = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
        boardController = BoardController.Instance;
        boardInput.onTileOver.AddListener(OnTileOver);
        boardInput.onTileOut.AddListener(OnTileOut);
        boardInput.onTileSelected.AddListener(SelectTile);

        //Invoke("UpdateCurrentTeam", 1f);
        //UpdateCurrentTeam();
    }

    public void SetCurrentCard(Card3D card)
    {
        currentCard = card;
    }
    public void SelectTile(Tile tile)
    {
        currentTile = tile;
        if (currentCard)
        {
            currentCard.cardDynamicData.teamID = 1 + currentTeam * 3;
            //NOW THEY ARE DIFFERENT TEAMS, MAKE THE MAGIC HAPPENS AND CREATE ANOTHER CLASS TO MANAGE THIS
            Debug.Log("READ THE COMMENTS HERE");
            acting = true;
            deckInput.SetBlock();
            deckView.RemoveCurrentCard();
            currentTile.SetCard(currentCard);

            currentCard.transform.SetParent(boardController.transform, true);

            //DO AMAZING ANIMATION HERE
            //foreach (Transform child in currentCard.GetComponentsInChildren<Transform>(true))
            //{
            //    child.gameObject.layer = LayerMask.NameToLayer("BoardLayerFront");  // add any layer you want. 
            //}

            //currentCard.gameObject.layer = LayerMask.NameToLayer("BoardLayerFront");//boardController.gameObject.layer;
            Vector3 target = tile.transform.position;
            target.y += 1.5f;
            float time = 0.75f;

            Vector3 currentPos = currentCard.transform.position;
            currentPos.y += 1.5f;

            currentCard.transform.DOScale(2f, time / 2);
            //currentCard.transform.DOLocalRotate(new Vector3(currentCard.transform.localRotation.x, currentCard.transform.localRotation.y, 0), time / 2, RotateMode.Fast);//.SetEase(Ease.OutElastic);
            currentCard.transform.DOMove(currentPos, time / 2).SetEase(Ease.OutBack).OnComplete(() =>
            {
                currentCard.transform.DOMove(target, time).OnComplete(() =>
                {
                    //currentCard.transform.DOMove(tile.transform.position, 0.35f).OnComplete(() =>
                    //{
                        deckInput.SetUnblock(1f);
                        EntityView ent = boardController.AddEntity(currentCard, tile);
                        //PUTA GAMBIARRA ISSO AQUI
                        tile.tileView.entityAttached = ent;                       
                    //boardController.AddEntity(currentCard, tile);
                        Destroy(currentCard.gameObject);
                        acting = false;
                    //}).SetEase(Ease.InBack);
                });
                currentCard.transform.DOLocalRotate(new Vector3(90f, 0, 0), time * 0.75f, RotateMode.Fast).SetEase(Ease.OutBack, 2f);
                currentCard.transform.DOScale(0.3f, time).SetEase(Ease.InBack);

            });


            currentTeam++;
            currentTeam %= decksTransform.Count;
            //UpdateCurrentTeam();
        }

    }
    void UpdateCurrentTeam()
    {
        for (int i = 0; i < decksTransform.Count; i++)
        {
            if(i == currentTeam)
            {
                decksTransform[i].SetActive(true);
            }
            else
            {
                decksTransform[i].SetActive(false);
            }
        }
    }
    //void Remove
    void OnTileOut(Tile tile)
    {
        ClearAllNeighbors();
    }
    void OnTileOver(Tile tile)
    {
        //if (currentNeighborsList)
        //{
        ClearAllNeighbors();
        //}
        if (!tile.hasCard)
        {
            currentTile = tile;
            currentNeighborsList = boardController.GetNeighbours(tile.tileModel, 2);
            HighlightAllNeighbors();
        }        

        tile.tileView.OnOver();

        //Debug.Log("TILE OVER "+tile.tileModel.i+" - "+ tile.tileModel.j);
        //Debug.Log(currentNeighborsList);
    }
    void HighlightAllNeighbors()
    {
        currentNeighborsList.CapOnFirstBlock();
        if (currentCard == null ||acting)
        {
            return;
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopLeft))
        {
            HighlightList(currentNeighborsList.topLeft, "TL");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.TopRight))
        {
            HighlightList(currentNeighborsList.topRight, "TR");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Left))
        {
            HighlightList(currentNeighborsList.left, "L");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.Right))
        {
            HighlightList(currentNeighborsList.right, "R");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomLeft))
        {
            HighlightList(currentNeighborsList.bottomLeft, "BL");
        }
        if (currentCard.cardDynamicData.sideList.Contains(SideType.BottomRight))
        {
            HighlightList(currentNeighborsList.bottomRight, "BR");
        }
    }
    void HighlightList(List<NeighborModel> list, string debug = "")
    {
        //for (int i = 0; i < list.Count; i++)
        for (int i = 0; i < list.Count; i++)
        {
            if (i < currentCard.cardStaticData.stats.range)
            {
                NeighborModel neighbor = list[i];
                if (neighbor.tile)
                {
                    neighbor.tile.tileView.OnHighlight();
                    neighbor.tile.tileView.debug.text = debug;
                }
            }

        }
    }

    void ClearAllNeighbors()
    {
        ClearList(currentNeighborsList.topLeft);
        ClearList(currentNeighborsList.topRight);
        ClearList(currentNeighborsList.left);
        ClearList(currentNeighborsList.right);
        ClearList(currentNeighborsList.bottomLeft);
        ClearList(currentNeighborsList.bottomRight);
    }
    void ClearList(List<NeighborModel> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            NeighborModel neighbor = list[i];
            if (neighbor.tile)
            {
                neighbor.tile.tileView.OnClear();

            }
        }
    }

}
