using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

public class LaunchBehavior : MonoBehaviourPunCallbacks
{
    [SerializeField]
    InputField _roomName_IF;

    [SerializeField]
    Text _roomName_txt;

    [SerializeField]
    Text _error_txt;

    [SerializeField]
    Transform _RoomContentList;

    [SerializeField]
    GameObject _RoomListPrefab;

    public static LaunchBehavior instance;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server!!!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuController.instance.OpenMenu("MainMenu");
        Debug.Log("Joined Lobby!!!");
    }

    public void CreateRoom()
    {
        //if the room name is empty, return
        if (string.IsNullOrEmpty(_roomName_IF.text))
        {
            return;
        }

        //Create a room with the name of the input field
        PhotonNetwork.CreateRoom(_roomName_IF.text);
        MenuController.instance.OpenMenu("LoadingMenu");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuController.instance.OpenMenu("LoadingMenu");
    }
    public override void OnJoinedRoom()
    {
        MenuController.instance.OpenMenu("JoinRoomMenu");
        _roomName_txt.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string ErrorMsg)
    {
        _error_txt.text = "Cannot Create Room :(  " + ErrorMsg;
        MenuController.instance.OpenMenu("ErrorMenu");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuController.instance.OpenMenu("LoadingMenu");
    }

    public override void OnLeftRoom()
    {
        MenuController.instance.OpenMenu("MainMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform t in _RoomContentList)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(_RoomListPrefab, _RoomContentList).GetComponent<RoomsListingBehavior>().SetUpRoom(roomList[i]);
        }
    }
}
