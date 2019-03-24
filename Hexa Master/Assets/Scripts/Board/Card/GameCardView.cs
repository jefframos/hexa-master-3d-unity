using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCardView : MonoBehaviour
{
    public Image charSprite;
    private CardStaticData cardStaticData;
    public TextMeshProUGUI attackLabel;
    public TextMeshProUGUI defenseLabel;
    public TextMeshProUGUI rangeLabel;
    public List<GameObject> starsList;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void setData(CardStaticData _cardStaticData)
    {
        cardStaticData = _cardStaticData;

        Debug.Log("Cards/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));

        attackLabel.text = cardStaticData.stats.attack.ToString();
        defenseLabel.text = cardStaticData.stats.defense.ToString();
        rangeLabel.text = cardStaticData.stats.range.ToString();

        for (int i = 0; i < starsList.Count; i++)
        {
            starsList[i].SetActive(false);
        }

        for (int i = 0; i < cardStaticData.level + 1; i++)
        {
            starsList[i].SetActive(true);
        }

        var sp = Resources.Load<Sprite>("Cards/thumbs/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));

        Debug.Log(cardStaticData.thumb_url);

        Debug.Log(sp);
        //Debug.Log(texture);

        charSprite.sprite = sp;
        //charSprite.sp Resources.Load<Sprite>("Cards/"+ cardStaticData.thumb_url);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
