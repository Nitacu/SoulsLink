using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SWNetwork;

public class LobbyDemo : MonoBehaviour
{
    public LobbyGUI GUI;

    /// <summary>
    /// Used to display players in different teams.
    /// </summary>
    Dictionary<string, string> playersDict_;

    /// <summary>
    /// Current room's custom data.
    /// </summary>
    RoomCustomData roomData_;

    /// <summary>
    /// Current page index of the room list. 
    /// </summary>
    int currentRoomPageIndex_ = 0;

    /// <summary>
    /// Player entered name
    /// </summary>
    string playerName_;
    /*
    private void Start()
    {
        // Subscribe to Lobby events
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent += Lobby_OnNewPlayerJoinRoomEvent;
        NetworkClient.Lobby.OnPlayerLeaveRoomEvent += Lobby_OnPlayerLeaveRoomEvent;
        NetworkClient.Lobby.OnRoomCustomDataChangeEvent += Lobby_OnRoomCustomDataChangeEvent;

        NetworkClient.Lobby.OnRoomMessageEvent += Lobby_OnRoomMessageEvent;
        NetworkClient.Lobby.OnPlayerMessageEvent += Lobby_OnPlayerMessageEvent;

        NetworkClient.Lobby.OnLobbyConncetedEvent += Lobby_OnLobbyConncetedEvent;
    }

    void onDestroy()
    {
        // Unsubscrible to Lobby events
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent -= Lobby_OnNewPlayerJoinRoomEvent;
        NetworkClient.Lobby.OnPlayerLeaveRoomEvent -= Lobby_OnPlayerLeaveRoomEvent;
        NetworkClient.Lobby.OnRoomCustomDataChangeEvent -= Lobby_OnRoomCustomDataChangeEvent;

        NetworkClient.Lobby.OnRoomMessageEvent -= Lobby_OnRoomMessageEvent;
        NetworkClient.Lobby.OnPlayerMessageEvent -= Lobby_OnPlayerMessageEvent;

        NetworkClient.Lobby.OnLobbyConncetedEvent -= Lobby_OnLobbyConncetedEvent;
    }
    */
    /*
    public void RegisterPlayer()
    {
        GUI.ShowRegisterPlayerPopup((bool ok, string playerName) =>
        {
            if (ok)
            {
                // store the playerName
                // playerName also used to register local player to the lobby server
                playerName_ = playerName;
                NetworkClient.Instance.CheckIn(playerName, (bool successful, string error) =>
                {
                    if (!successful)
                    {
                        Debug.LogError(error);
                    }
                });
            }
        });
    }

    void Lobby_OnLobbyConncetedEvent()
    {
        Debug.Log("Lobby_OnLobbyConncetedEvent");
        // Register the player using the entered player name.
        NetworkClient.Lobby.Register(playerName_, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Lobby registered " + reply);
                if (reply.started)
                {
                    // Player is in a room and the room has started.
                    // Call NetworkClient.Instance.ConnectToRoom to connect to the game servers of the room.
                }
                else if (reply.roomId != null)
                {
                    // Player is in a room.
                    GetRooms();
                    GetPlayersInCurrentRoom();
                }
                else
                {
                    // Player is not in a room.
                    GetRooms();
                }
            }
            else
            {
                Debug.Log("Lobby failed to register " + error);
            }
        });
    }

    public void CreateNewRoom()
    {
        GUI.ShowNewGamePopup((bool ok, string gameName) =>
        {
            if (ok)
            {
                roomData_ = new RoomCustomData();
                roomData_.name = gameName;
                roomData_.team1 = new TeamCustomData();
                roomData_.team2 = new TeamCustomData();
                roomData_.team1.players.Add(NetworkClient.Lobby.PlayerId);

                // use the serializable roomData object as room's custom data.
                NetworkClient.Lobby.CreateRoom(roomData_, true, 4, (successful, reply, error) =>
                {
                    if (successful)
                    {
                        Debug.Log("Room created " + reply);
                        // refresh the room list
                        GetRooms();

                        // refresh the player list
                        GetPlayersInCurrentRoom();
                    }
                    else
                    {
                        Debug.Log("Failed to create room " + error);
                    }
                });
            }
        });
    }

    public void SendRoomMessage(string message)
    {
        Debug.Log("Send room message " + message);
    }

    public void OnRoomSelected(string roomId)
    {
        Debug.Log("OnRoomSelected: " + roomId);
    }

    public void OnPlayerSelected(string playerId)
    {
        Debug.Log("OnPlayerSelected: " + playerId);
    }

    public void GetRooms()
    {
        Debug.Log("GetRooms");
    }

    public void NextPage()
    {
        currentRoomPageIndex_++;
        GetRooms();
    }

    public void PreviousPage()
    {
        currentRoomPageIndex_--;
        GetRooms();
    }

    public void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
    }

    void RefreshPlayerList()
    {
        // Use the room custom data, and the playerId and player Name dictionary to populate the player lsit
        if (playersDict_ != null)
        {
            GUI.ClearPlayerList();
            GUI.AddRowForTeam("Team 1");
            foreach (string pId in roomData_.team1.players)
            {
                String playerName = playersDict_[pId];
                GUI.AddRowForPlayer(playerName, pId, OnPlayerSelected);
            }

            GUI.AddRowForTeam("Team 2");
            foreach (string pId in roomData_.team2.players)
            {
                String playerName = playersDict_[pId];
                GUI.AddRowForPlayer(playerName, pId, OnPlayerSelected);
            }
        }
    }*/
}
