using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardView : MonoBehaviour
{

    [System.Serializable]
    public class TileData
    {
        public float width = 1;
        public float height = 0.85f;
        public float scale = 1;
    }
    public TileData tileData;
    float starterWidth = -90;
    float starterHeight = -90;
    float starterScale = -90;
    private int lin;
    private int col;
    private bool built;
    private BoardController boardController;
    public List<List<Tile>> tileList;
    public Transform boardContainer;
    public GameObject entityPrefab;
    // Start is called before the first frame update
    void Start()
    {
        built = false;
        boardController = BoardController.Instance;
    }


    internal void SetTiles(List<List<Tile>> _tileList, int _lin, int _col)
    {
        lin = _lin;
        col = _col;
        tileList = _tileList;
        built = true;
        UpdateTilesPosition();
    }

    void UpdateTilesPosition()
    {
        if (!built)
        {
            return;
        }
        if (starterWidth == tileData.width
            && starterHeight == tileData.height
             && starterScale == tileData.scale)
        {
            return;
        }
        starterWidth = tileData.width;
        starterHeight = tileData.height;


        float targetX = 0;
        float targetZ = 0;
        for (int i = 0; i < tileList.Count; i++)
        {
            List<Tile> element = tileList[i];
            for (int j = 0; j < element.Count; j++)
            {
                targetX = -col / 2 + j;
                if(col % 2 != 0)
                {
                    targetX -= tileData.width / 2;
                }
                targetZ = lin / 2 - i;
                //targetZ = boardData.lin - i;
                if (i % 2 != 0)
                {
                    targetX += 0.5f;
                }
                Tile tile = element[j];
                tile.transform.position = new Vector3(targetX * tileData.width + tileData.width / 4, tile.rnd, targetZ * tileData.height);
                tile.transform.localScale = new Vector3(tileData.scale, tileData.scale, tileData.scale);
            }
        }
    }

    internal void PlaceCard(Card3D currentCard, Tile tile, Action callback)
    {
        //NOW THEY ARE DIFFERENT TEAMS, MAKE THE MAGIC HAPPENS AND CREATE ANOTHER CLASS TO MANAGE THIS
        Debug.Log("READ THE COMMENTS HERE");
        //acting = true;
        
        //currentTile.SetCard(currentCard);

        currentCard.transform.SetParent(boardContainer, true);

        //DO AMAZING ANIMATION HERE

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
                //deckInput.SetUnblock(1f);
                EntityView ent = AddEntity(currentCard, tile);
                //PUTA GAMBIARRA ISSO AQUI
                tile.tileView.entityAttached = ent;
                //boardController.AddEntity(currentCard, tile);
                Destroy(currentCard.gameObject);
                callback();
                //acting = false;
                //}).SetEase(Ease.InBack);
            });
            currentCard.transform.DOLocalRotate(new Vector3(90f, 0, 0), time * 0.75f, RotateMode.Fast).SetEase(Ease.OutBack, 2f);
            currentCard.transform.DOScale(0.3f, time).SetEase(Ease.InBack);

        });

    }
    public EntityView AddEntity(Card3D card, Tile tile)
    {
        GameObject cardTransform = Instantiate(entityPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        cardTransform.transform.localPosition = tile.transform.localPosition;
        EntityView entity = cardTransform.GetComponent<EntityView>();
        entity.SetData(card.cardStaticData, card.cardDynamicData);

        return entity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTilesPosition();
    }
}
