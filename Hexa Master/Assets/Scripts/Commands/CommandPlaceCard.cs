using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlaceCard : CommandDefault
{
    internal class CommandPlaceCardData
    {
        internal Transform boardContainer;
        internal Card3D currentCard;
        internal Tile tile;//, Action callback
        internal string name = "PlaceCard";
    }
    CommandPlaceCardData data;

    public override void SetData(object obj)
    {
        param = obj;
        data = obj as CommandPlaceCardData;
    }
    public override void Play()
    {
        base.Play();

        data.currentCard.transform.SetParent(data.boardContainer, true);

        //DO AMAZING ANIMATION HERE
        Vector3 target = data.tile.transform.position;
        target.y += 1.5f;
        float time = 0.75f;

        Vector3 currentPos = data.currentCard.transform.position;
        currentPos.y += 1.5f;

        data.currentCard.transform.DOScale(2f, time / 2);
        //currentCard.transform.DOLocalRotate(new Vector3(currentCard.transform.localRotation.x, currentCard.transform.localRotation.y, 0), time / 2, RotateMode.Fast);//.SetEase(Ease.OutElastic);
        data.currentCard.transform.DOMove(currentPos, time / 2).SetEase(Ease.OutBack).OnComplete(() =>
        {
    
            data.currentCard.transform.DOMove(target, time).OnComplete(() =>
            {
                Kill();
                GameObject.Destroy(data.currentCard.gameObject);
            });
            data.currentCard.transform.DOLocalRotate(new Vector3(90f, 0, 0), time * 0.75f, RotateMode.Fast).SetEase(Ease.OutBack, 2f);
            data.currentCard.transform.DOScale(0.3f, time).SetEase(Ease.InBack);
           

        });

    }
 
}
