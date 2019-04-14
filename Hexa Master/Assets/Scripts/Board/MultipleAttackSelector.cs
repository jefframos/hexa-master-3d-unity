using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleAttackSelector : MonoBehaviour
{
    public class SelectorData
    {
        internal EntityView entity;
        internal EntitySelectorView selectorView;
        internal InteractiveObject interactive;
        internal EnemiesAttackData attackData;
        internal int id;
    }
    public class MultipleAttackEvent : UnityEvent<List<EnemiesAttackData>> { };
    public MultipleAttackEvent onMultiplesReady = new MultipleAttackEvent();
    public GameObject SelectorPrefab;
    List<SelectorData> selectors;
    List<SelectorData> selectedOrder;
    static int INTERACTIVE_ACC = 0;
    private int order = 1;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void EntitySelected(InteractiveObject interactive)
    {
        EntityView entityView = interactive.GetComponentInParent<EntityView>();
        for (int i = 0; i < selectors.Count; i++)
        {
            SelectorData selectorData = selectors[i];
            selectorData.selectorView.label.text = "";


            if (selectorData.interactive.id == interactive.id)
            {
                if (selectedOrder.Contains(selectorData))
                {
                    selectedOrder.Remove(selectorData);
                }
                else
                {
                    selectedOrder.Add(selectorData);
                }

            }
        }
        List<EnemiesAttackData> attackData = new List<EnemiesAttackData>();

        for (int i = 0; i < selectedOrder.Count; i++)
        {
            SelectorData selectorData = selectedOrder[i];
            selectorData.selectorView.label.text = (i + 1).ToString();
            attackData.Add(selectorData.attackData);
        }
        if (selectedOrder.Count >= selectors.Count)
        {
            onMultiplesReady.Invoke(attackData);
            for (int i = 0; i < selectors.Count; i++)
            {
                SelectorData selectorData = selectors[i];
                selectorData.entity.SetInteractive(false);
                Destroy(selectorData.selectorView.gameObject);
            }
        }
    }
    public void SetEntities(List<EnemiesAttackData> attackList)
    {

        order = 1;
        selectedOrder = new List<SelectorData>();
        selectors = new List<SelectorData>();
        for (int i = 0; i < attackList.Count; i++)
        {
            EntityView entity = attackList[i].tile.entityAttached;
            entity.SetInteractive(true);
            entity.interactive.onClick.RemoveAllListeners();
            entity.interactive.onClick.AddListener(EntitySelected);

            GameObject selectorTransform = Instantiate(SelectorPrefab, new Vector3(0, 0, 0), Quaternion.identity, entity.transform);
            selectorTransform.transform.localPosition = new Vector3(0, 1.5f, 0);
            Vector3 angles = selectorTransform.transform.eulerAngles;
            angles.x = 30;
            selectorTransform.transform.eulerAngles = angles;
            
            entity.interactive.id = INTERACTIVE_ACC;
            INTERACTIVE_ACC++;
            selectors.Add(new SelectorData
            {
                interactive = entity.interactive,
                entity = entity,
                selectorView = selectorTransform.GetComponent<EntitySelectorView>(),
                attackData = attackList[i]
            });

        }

        //StartSelectors();
    }
}
