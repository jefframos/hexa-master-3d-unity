using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
public class EntityView : MonoBehaviour
{
    public SpriteRenderer charSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(CardStaticData cardStaticData)
    {
        var sp = Resources.Load<Sprite>("Cards/thumbs/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));
        charSprite.sprite = sp;
        charSprite.transform.DOMoveY(charSprite.transform.position.y + 1f, 0.75f).From().SetEase(Ease.OutBounce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
