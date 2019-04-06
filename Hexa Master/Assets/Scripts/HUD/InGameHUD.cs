﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameHUD : MonoBehaviour
{
    public TopHUD topHud;

    public TextMeshProUGUI DEBUG;

    public void UpdateCurrentRound(int currentTeam, int team1Score, int team2Score)
    {
        topHud.UpdateCurrentTeam(currentTeam);
        topHud.UpdateScore(team1Score, team2Score);
    }
    // Start is called before the first frame update
    void Start()
    {
        topHud.UpdateCurrentTeam(0);
        topHud.UpdateScore(0, 0);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
