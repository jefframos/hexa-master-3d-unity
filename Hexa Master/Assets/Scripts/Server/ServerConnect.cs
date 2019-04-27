using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerConnect : MonoBehaviour
{
    public string versionName = "0.1";

    private bool isConnected = false;

    public void Connect()
    {
        if (isConnected)
        {
            return;
        }
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Connect");
    }
    
    private void OnPlayerConnected(NetworkIdentity player)
    {
        Debug.Log("OnPlayerConnected");
    }
    private void OnConnectedToServer()
    {
        Debug.Log("OnConnected");
        isConnected = true;
        PhotonNetwork.JoinLobby(Photon.Realtime.TypedLobby.Default);
    }
    private void OnFailedToConnectToMasterServer(NetworkReachability error)
    {
        Debug.Log(error);
    }
    private void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    private void OnDisconnectedFromServer(NetworkIdentity info)
    {
        Debug.Log("OnDisconnectedFromServer");
    }
}
