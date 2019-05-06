using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerInfoPrefab;
    List<PlayerInfoView> playerInfoList;
    
    void Start()
    {
        playerInfoList = new List<PlayerInfoView>();       
    }

    internal void BuildGameHud(List<PlayerData> playerDataList)
    {
        for (int i = 0; i < playerInfoList.Count; i++)
        {
            Destroy(playerInfoList[i].gameObject);
        }
        playerInfoList = new List<PlayerInfoView>();

        for (int i = 0; i < playerDataList.Count; i++)
        {
            GameObject playerInfo = Instantiate(playerInfoPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            PlayerInfoView playerInfoView = playerInfo.GetComponent<PlayerInfoView>();
            playerInfoList.Add(playerInfoView);
            playerInfoView.SetPlayerData(playerDataList[i]);
        }
    }

    internal void UpdateScore(ScoreData score)
    {
        //Debug.Log(score);
        for (int i = 0; i < playerInfoList.Count; i++)
        {
            playerInfoList[i].UpdateCurrentStatus(score);

        }
    }
    //// Update is called once per frame
    //void Update()
    //{

    //}
}
