using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonNetworkManagerLan : PhotonNetworkManager
{
    public string _serverAddress;
    public int _port;

    public override void Start()
    {
        

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Conectando al sevidor...");
            PhotonNetwork.ConnectToMaster(_serverAddress, _port, "1");
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Conectado al lobby");
        PhotonNetwork.AuthValues = new AuthenticationValues(PhotonNetwork.NickName);
        _lobbyUI.clearListRooms();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("NO SE CONECTA " + returnCode + message);
    }

}
