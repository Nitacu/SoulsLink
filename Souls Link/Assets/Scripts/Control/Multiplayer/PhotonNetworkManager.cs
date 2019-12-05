using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{

    public ControlLobbyUI _lobbyUI;
    public PhotonView _photonView;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        _lobbyUI.playersNumber = new ControlLobbyUI.PlayersNumberDelegate(returnPlayersInRoom);
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Conectando al sevidor...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void starGame()
    {
        SceneManager.LoadScene("Arena");
    }

    public void registerName()
    {
        PhotonNetwork.NickName = _lobbyUI.registerUser();
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        _lobbyUI.enterLobby();
    }

    public void loginRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    public void creatRoom()
    {
        PhotonNetwork.CreateRoom(_lobbyUI.nameOfTheNewRoom(), roomOptions: new RoomOptions() { MaxPlayers = 4 }, typedLobby: null);
    }

    public int returnPlayersInRoom()
    {
        int numberPlayers = 0;
        numberPlayers = PhotonNetwork.CurrentRoom.PlayerCount;

        return numberPlayers;
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al Master");
        _lobbyUI._panelRegistry.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby");

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Se creo la sala");
        _lobbyUI.showPanelCreatedRoom(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entro a la sala");

        _lobbyUI.clearPlayersList();
        _lobbyUI._panelInRoom.GetComponent<SelectCharacter>().MyID = PhotonNetwork.LocalPlayer.UserId;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            _lobbyUI.crearNewItemInPlayerList(player.NickName, player.UserId);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _lobbyUI._buttonStarGame.SetActive(true);
            FindObjectOfType<SelectCharacter>().starGame = new SelectCharacter.StarGame(starGame);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _lobbyUI.clearListRooms();

        foreach (RoomInfo room in roomList)
        {
            _lobbyUI.crearNewItemInRoomList(room.Name, room.Name);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("entro");
        _lobbyUI.crearNewItemInPlayerList(newPlayer.NickName, newPlayer.UserId);
    }
    #endregion
}
