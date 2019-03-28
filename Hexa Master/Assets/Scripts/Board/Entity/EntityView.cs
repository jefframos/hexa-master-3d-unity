using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
