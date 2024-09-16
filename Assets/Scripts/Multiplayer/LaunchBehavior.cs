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

    [SerializeField]
    Transform _PlayerContentList;

    [SerializeField]
    GameObject _PlayerListPrefab;

    public static LaunchBehavior instance;

    public GameObject Start_Btn;


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

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server!!!");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuController.instance.OpenMenu("MainMenu");
        Debug.Log("Joined Lobby!!!");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
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

        //Array that shows players in the room
        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in _PlayerContentList)
        {
            Destroy(child.gameObject);
        }

        //Displays all the players in the room
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(_PlayerListPrefab, _PlayerContentList).GetComponent<PlayerListBehavior>().SetUp(players[i]);
        }

        //The start button will only show to the player which made the room
        Start_Btn.SetActive(PhotonNetwork.IsMasterClient);
    }

    //This method will be called if the room's host leaves the room, it'll make give another player host priviledges
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Start_Btn.SetActive(PhotonNetwork.IsMasterClient);
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
            if (roomList[i].RemovedFromList)
            {
                continue;
            }

            Instantiate(_RoomListPrefab, _RoomContentList).GetComponent<RoomsListingBehavior>().SetUpRoom(roomList[i]);
        }
    }

    //Method to be called when a player joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_PlayerListPrefab, _PlayerContentList).GetComponent<PlayerListBehavior>().SetUp(newPlayer);
    }
}
