using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfoView : MonoBehaviour
{
    // Start is called before the first frame update
    public Image avatarImage;
    public Image bar;
    public Image entityIcon;

    public TextMeshProUGUI teamName;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI agility;
    public TextMeshProUGUI zonesLabel;
    public TextMeshProUGUI entitiesLabel;

    AspectRatioFitter aspectRatioFitter;

    PlayerData playerData;

    public int TeamID { get => playerData.teamID; }

    void Start()
    {
        //teamName.text = "Team Name";
        aspectRatioFitter = GetComponent<AspectRatioFitter>();

       // Invoke("enableFillter", 0.01f);
    }
    void enableFillter()
    {
        aspectRatioFitter.enabled = true;

    }
    internal void SetPlayerData(PlayerData _playerData)
    {
        playerData = _playerData;
        playerData.playerInfoView = this;
        bar.fillAmount = 0.5f;
        string spritePath = "elf";
        switch (playerData.deckType)
        {
            case DeckType.NONE:
                break;
            case DeckType.ALLIANCE:
                break;
            case DeckType.HORDE:
                break;
            case DeckType.HUMAN:
                teamName.text = "Team Hero";
                spritePath = "human";
                break;
            case DeckType.DWARF:
                teamName.text = "Team Beard";
                spritePath = "dwarf";
                break;
            case DeckType.ELF:
                teamName.text = "Team Blond";
                spritePath = "elf";
                break;
            case DeckType.ORC:
                teamName.text = "Team Angry";
                spritePath = "orc";
                break;
            default:
                break;
        }
        var sp = Resources.Load<Sprite>("HUD/covers/"+ spritePath);
        avatarImage.sprite = sp;
         // Debug.Log(playerData.deckType);
        bar.color = playerData.teamColor;
        entityIcon.color = playerData.teamColor;
        attack.text =playerData.Attack.ToString();
        defense.text = playerData.Defense.ToString();
        agility.text = playerData.Agility.ToString();

        //attack.text = "Attack: " + playerData.Attack;
        //defense.text = "Defense: " + playerData.Defense;
        //agility.text = "Agility: " + playerData.Agility;
    }

    internal void UpdateCurrentStatus(ScoreData score)
    {
      
        int zones = playerData.zonesWinning;
        //for (int i = 0; i < score.allZones.Count; i++)
        //{
        //    if (score.allZones[i][TeamID - 1] > 0)
        //    {
        //        zones++;
        //    }
        //}
        //// score.allZones[TeamID];
        //int entities = score.allPlayers[TeamID - 1];
        bar.DOFillAmount(zones / 3f, 0.5f).SetEase(Ease.OutBack);
        zonesLabel.text = zones.ToString();
        entitiesLabel.text = playerData.totalOnBoard.ToString();

        //entitiesLabel.text = entities.ToString();
        //Debug.Log(zones + " - " + entities);
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
