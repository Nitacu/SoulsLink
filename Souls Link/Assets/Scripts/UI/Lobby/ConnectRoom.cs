using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectRoom : MonoBehaviour
{
    public string _id;
    private LobbyKevin _lobbyKevin;
    private PhotonNetworkManager _photonNetwork;

    void Start()
    {
        _lobbyKevin = FindObjectOfType<LobbyKevin>();
        _photonNetwork = FindObjectOfType<PhotonNetworkManager>();
    }

    public void conectRoom()
    {
        if (_lobbyKevin)
            _lobbyKevin.OnRoomSelected(_id);
        else
            _photonNetwork.loginRoom(_id);
    }

}
