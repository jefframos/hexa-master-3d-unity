using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAddEntity : CommandDefault
{
  
    internal class CommandEntityData
    {
        //internal Card3D currentCard;
        internal CardStaticData cardStaticData;
        internal CardDynamicData cardDynamicData;
        internal Tile tile;
        internal BoardView boardView;
        internal GameObject entityPrefab;
    }
    CommandEntityData data;
    float timer = 1f;
    public override void SetData(object obj)
    {
        param = obj;
        data = obj as CommandEntityData;        
    }

    public override void Play()
    {
        base.Play();


        EntityView ent = AddEntity(data.tile);
        data.tile.entityAttached = ent;

        timer = 0.5f;

    }
    EntityView AddEntity(Tile tile)
    {
        GameObject cardTransform = Object.Instantiate(data.entityPrefab, new Vector3(0, 0, 0), Quaternion.identity, data.boardView.transform);
        cardTransform.transform.localPosition = tile.transform.localPosition;
        EntityView entity = cardTransform.GetComponent<EntityView>();
        entity.SetData(data.cardStaticData, data.cardDynamicData);

        return entity;
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Kill();
        }
        base.Update();
    }
}
