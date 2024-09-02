using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomsListingBehavior : MonoBehaviour
{
    [SerializeField]
    Text _RoomName_txt;

    RoomInfo _info;

    public void SetUpRoom(RoomInfo data)
    {
        //Whatever data is put in the RoomInfo object will be displayed in the room listing
        _info = data;
        _RoomName_txt.text = data.Name;
    }

    public void OnClick()
    {
        LaunchBehavior.instance.JoinRoom(_info);
    }
}
