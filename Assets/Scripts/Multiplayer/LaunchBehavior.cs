using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LaunchBehavior : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server!!!");
        OnJoinedLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuController.instance.OpenMenu("MainMenu");
        Debug.Log("Joined Lobby!!!");
    }
}
