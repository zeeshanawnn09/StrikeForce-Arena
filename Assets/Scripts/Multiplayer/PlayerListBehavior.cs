using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListBehavior : MonoBehaviourPunCallbacks
{
    public Text PlayerName_txt;

    Player _player;

    public void SetUp(Player player)
    {
        _player = player;
        PlayerName_txt.text = player.NickName;
    }

    //Method to be called when a player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    //Method to be called when the local player leaves the room
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
