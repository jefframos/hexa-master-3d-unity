using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoView : MonoBehaviour
{
    // Start is called before the first frame update
    public Image avatarImage;
    public Image bar;

    public TextMeshProUGUI teamName;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI agility;

    PlayerData playerData;
    void Start()
    {
        teamName.text = "Team Name";
    }
    internal void SetPlayerData(PlayerData _playerData)
    {
        playerData = _playerData;
        bar.color = playerData.teamColor;
        attack.text = "Attack: " + playerData.Attack;
        defense.text = "Defense: " + playerData.Defense;
        agility.text = "Agility: " + playerData.Agility;
    }
    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
