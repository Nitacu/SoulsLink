using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectRoom : MonoBehaviour
{
    public string _id;
    private LobbyKevin _lobbyKevin;

    void Start()
    {
        _lobbyKevin = FindObjectOfType<LobbyKevin>();
    }

    public void conectRoom()
    {
        _lobbyKevin.OnRoomSelected(_id);
    }

}
