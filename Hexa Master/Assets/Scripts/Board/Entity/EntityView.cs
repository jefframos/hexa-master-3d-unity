using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class EntityView : MonoBehaviour
{
    public SpriteRenderer charSprite;
    public TextMeshPro statsLabel;
    AttackZonesCardView attackZones;
    public Renderer teamMaterial;
    private float sin;
    private bool floating;
    private float startY;
    public CardDynamicData cardDynamicData;
    public CardStaticData cardStaticData;
    public GameObject getAttackedParticle;
    public Transform blockView;
    public TextMeshPro actionLabel;
    public InteractiveObject interactive;
    internal Tile tile;
    public List<Sprite> spriteFeedbacks;
    public SpriteRenderer previewFeedback;
    // Start is called before the first frame update
    void Start()
    {
        sin = 0;
        floating = false;
        getAttackedParticle.SetActive(false);
        blockView.gameObject.SetActive(false);
        previewFeedback.gameObject.SetActive(false);
        SetInteractive(false);

    }
    internal void SetInteractive(bool b)
    {
        interactive.gameObject.SetActive(b);
    }
    public void SetData(CardStaticData _cardStaticData, CardDynamicData _cardDynamicData, Tile _tile)
    {
        tile = _tile;
        cardStaticData = _cardStaticData;
        cardDynamicData = _cardDynamicData;

        cardDynamicData.OnUpdateData.AddListener(UpdateDynamicData);
        tile.RemoveTileEffectView();
        startY = charSprite.transform.localPosition.y;
        var sp = Resources.Load<Sprite>("Cards/" + cardStaticData.folder + "/" + Path.GetFileNameWithoutExtension(cardStaticData.thumb_url));
        charSprite.sprite = sp;
        charSprite.transform.DOMoveY(charSprite.transform.localPosition.y + 1f, 0.75f).From().SetEase(Ease.OutBounce).OnComplete(() =>
        {
            EnableFloating();
        });

        statsLabel.text = (cardDynamicData.StaticAttack / 10).ToString();
        if (cardDynamicData.EffectAttack > 0)
        {
            statsLabel.text += "+" + (cardDynamicData.EffectAttack / 10).ToString();
        }
        statsLabel.text += " / ";
        statsLabel.text += (cardDynamicData.StaticDefense / 10).ToString();
        if (cardDynamicData.EffectDefense > 0)
        {
            statsLabel.text += "+" + (cardDynamicData.EffectDefense / 10).ToString();
        }


        statsLabel.text = (cardDynamicData.Defense / 10).ToString();

        attackZones = GetComponentInChildren<AttackZonesCardView>();
        attackZones.SetZones(cardDynamicData.SideList);
        attackZones.SetInGameMode();

        ApplyTeamColor();
    }
    public void UpdateDynamicData(CardDynamicData _cardDynamicData)
    {
        if(cardDynamicData.Defense > cardDynamicData.StaticDefense)
        {
            statsLabel.color = Color.green;
        }
        else
        {
            statsLabel.color = Color.white;

        }
        statsLabel.text = (cardDynamicData.Defense / 10).ToString();
    }
    public void ApplyTeamColor()
    {
        teamMaterial.material.DOColor(cardDynamicData.TeamColor, 0.5f);
        //teamMaterial.material.color = cardDynamicData.teamColor;
    }
    public void EnableFloating()
    {
        floating = true;
    }

    internal void GetAttack()
    {
        getAttackedParticle.SetActive(true);
        Invoke("RemoveParticles", 1f);

    }
    internal void CounterAttack()
    {
        actionLabel.text = "Counter";
        blockView.gameObject.SetActive(true);
        blockView.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutBack).From();
        Invoke("HideBlock", 1f);

    }
    internal void BlockAttack()
    {
        actionLabel.text = "BLOCK";
        blockView.gameObject.SetActive(true);
        blockView.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutBack).From();
        Invoke("HideBlock", 1f);

    }



    void HideBlock()
    {
        blockView.gameObject.SetActive(false);
    }

    void RemoveParticles()
    {
        getAttackedParticle.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (floating)
        {
            Vector3 targ = charSprite.transform.localPosition;
            targ.y = startY + Mathf.Sin(sin) * 0.025f;
            sin += Time.deltaTime * 1.1f;
            charSprite.transform.localPosition = targ;
        }

    }
    internal void WinFeedback()
    {
        previewFeedback.gameObject.SetActive(true);
        previewFeedback.sprite = spriteFeedbacks[0];

    }
    internal void BlockFeedback()
    {
        previewFeedback.gameObject.SetActive(true);
        previewFeedback.sprite = spriteFeedbacks[1];

    }
    internal void LoseFeedback()
    {
        previewFeedback.gameObject.SetActive(true);
        previewFeedback.sprite = spriteFeedbacks[2];
    }
    internal void HideFeedback()
    {
        previewFeedback.gameObject.SetActive(false);
    }
    internal void OnOver()
    {
        Color targColor = Color.white;
        targColor.a = 0.25f;
        charSprite.color = targColor;
    }

    internal void OnOut()
    {
        charSprite.color = Color.white;
    }
}
