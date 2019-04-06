using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopHUD : MonoBehaviour
{
    public TextMeshProUGUI currentTeamLabel;
    public TextMeshProUGUI team1ScoreLabel;
    public TextMeshProUGUI team2ScoreLabel;
    public void UpdateCurrentTeam(int teamID)
    {
        currentTeamLabel.text = teamID.ToString();
    }
    public void UpdateScore(int team1Score, int team2Score)
    {
        team1ScoreLabel.text = team1Score.ToString();
        team2ScoreLabel.text = team2Score.ToString();
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
