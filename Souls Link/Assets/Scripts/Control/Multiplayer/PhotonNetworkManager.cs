using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    private bool _nickName;
    public ControlLobbyUI _lobbyUI;
    public PhotonView _photonView;
    protected LoadBalancingClient _balancingClient;
    protected ConnectionHandler ch;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _lobbyUI.playersNumber = new ControlLobbyUI.PlayersNumberDelegate(returnPlayersInRoom);
    }

    public virtual void Start()
    {

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Conectando al sevidor...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void leaveRoom()
    {
        List<SelectingCharacter> selecting = FindObjectsOfType<SelectingCharacter>().ToList();

        foreach (var delete in selecting)
        {
            delete.CharactersPanel.GetComponentInParent<PlayerSelectCharPanel>()._photonView.RPC("DeactivateMySelection", RpcTarget.All);
        }

        FindObjectOfType<SelectCharacter>()._filledSlots.Clear();
        //las cosas para arrancar la partida
        _lobbyUI._buttonStarGame.SetActive(true);
        FindObjectOfType<SelectCharacter>().starGame = null;

        PhotonNetwork.LeaveRoom();
        _lobbyUI.enterLobby();
    }
    
    public void starGame()
    {
        SceneManager.LoadScene("PracticeRange");
    }

    public void registerName()
    {
        PhotonNetwork.NickName = _lobbyUI.registerUser();
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        _nickName = true;
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

    public bool existingRoom(RoomInfo room)
    {
        ConnectRoom[] connectRooms = FindObjectsOfType<ConnectRoom>();

        if (connectRooms.Length > 0)
        {
            foreach (ConnectRoom connect in connectRooms)
            {
                Debug.Log(room.Name + "==" + connect._id);
                if (room.Name == connect._id)
                    return true;
            }
        }

        return false;
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        if (!_nickName)
            _lobbyUI._panelRegistry.SetActive(true);
        else
            PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby");
        _lobbyUI.clearListRooms();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Se creo la sala");
        _lobbyUI.showPanelCreatedRoom(false);
    }

    public override void OnJoinedRoom()
    {
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
        Debug.Log("Update roomlist: " + roomList.Count + " salas");

        foreach (RoomInfo room in roomList)
        {
            Debug.Log("La sala " + room.Name + " tiene " + room.PlayerCount + " jugadores");

            if (room.PlayerCount >= 1)
            {
                if (!existingRoom(room))
                    _lobbyUI.crearNewItemInRoomList(room.Name, room.Name);
            }
            else if (room.PlayerCount == 0)
                _lobbyUI.deletedItemRoom(room.Name);

        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("entro");
        if (PhotonNetwork.LocalPlayer != newPlayer)
        {
            _lobbyUI.addNewPlayerToRoom();
        }

    }

    public override void OnLeftRoom()
    {
        // llama un metodo en el selecting para eliminarlo
        List<SelectingCharacter> selecting = FindObjectsOfType<SelectingCharacter>().ToList();

        foreach (var delete in selecting)
        {
            Destroy(delete.gameObject);
        }

        _lobbyUI.clearPlayersList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _lobbyUI._buttonStarGame.SetActive(true);
            FindObjectOfType<SelectCharacter>().starGame = new SelectCharacter.StarGame(starGame);
        }
    }
    #endregion
}
