using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SWNetwork;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyKevin : MonoBehaviour
{

    [SerializeField] private ControlLobbyUI _lobbyUI;

    private void Start()
    {
        NetworkClient.Lobby.OnLobbyConnectedEvent += OnLobbyConnectedEvent;
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent += Lobby_OnNewPlayerJoinRoomEvent;
        NetworkClient.Lobby.OnPlayerLeaveRoomEvent += Lobby_OnPlayerLeaveRoomEvent;
        NetworkClient.Lobby.OnNewRoomOwnerEvent += Lobby_OnNewRoomOwnerEvent;
        NetworkClient.Lobby.OnRoomReadyEvent += Lobby_OnRoomReadyEvent;
    }

    private void OnDestroy()
    {
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent -= Lobby_OnNewPlayerJoinRoomEvent;
        NetworkClient.Lobby.OnLobbyConnectedEvent -= OnLobbyConnectedEvent;
        NetworkClient.Lobby.OnPlayerLeaveRoomEvent -= Lobby_OnPlayerLeaveRoomEvent;
        NetworkClient.Lobby.OnNewRoomOwnerEvent -= Lobby_OnNewRoomOwnerEvent;
        NetworkClient.Lobby.OnRoomReadyEvent -= Lobby_OnRoomReadyEvent;
    }

    public void checkIn()
    {
        //checking
        NetworkClient.Instance.CheckIn(_lobbyUI.registerUser(), (bool successful, string error) =>
        {
            if (!successful)
            {
                Debug.LogError(error);
            }
            else
            {
                Debug.Log("se registro ");
            }
        });

    }

    public void OnLobbyConnectedEvent()
    {

        //entra al lobby
        NetworkClient.Lobby.Register(_lobbyUI.registerUser(), (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Lobby registered " + reply);

                _lobbyUI.enterLobby();

                if (reply.started)
                {
                    // si el jugador ya esta en una room y esta ha comenzado la partida
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
                Debug.Log("Lobby failed to register si si" + error);
            }
        });
    }

    public void createdNewRoom()
    {
        NetworkClient.Lobby.CreateRoom(_lobbyUI.nameOfTheNewRoom(), true, 4, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Room created " + reply);
                _lobbyUI.showPanelCreatedRoom(false);

                // refresh the room list
                GetRooms();
                _lobbyUI._panelRooms.SetActive(false);
                // refresh the player list

                GetPlayersInCurrentRoom();

                _lobbyUI._buttonStarGame.SetActive(true);
            }
            else
            {
                Debug.Log("Failed to create room " + error);
            }
        });
    }

    public void GetRooms()
    {
        NetworkClient.Lobby.GetRooms(0, 10, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Got rooms " + reply);

                // Remove rooms in the rooms list
                _lobbyUI.clearListRooms();
                //muestra los rooms que ya estan creados

                foreach (SWRoom room in reply.rooms)
                {
                    Debug.Log("nombre de la sala " + room.data);
                    _lobbyUI.crearNewItemInRoomList(room.data, room.id);
                }

            }
            else
            {
                Debug.Log("Failed to get rooms " + error);
            }
        });
    }

    public void GetPlayersInCurrentRoom()
    {
        NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Got players " + reply);

                _lobbyUI.clearPlayersList();

                foreach (SWPlayer player in reply.players)
                {
                    Debug.Log("nombre de jugador" + player.data);

                    _lobbyUI.crearNewItemInPlayerList(player.data, player.id);
                }

            }
            else
            {
                Debug.Log("Failed to get players " + error);
            }
        });
    }

    public void OnRoomSelected(string roomId)
    {
        Debug.Log("OnRoomSelected: " + roomId);
        // Join the selected room
        NetworkClient.Lobby.JoinRoom(roomId, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Joined room " + reply);
                // refresh the player list
                GetPlayersInCurrentRoom();
            }
            else
            {
                Debug.Log("Failed to Join room " + error);
            }
        });
    }

    public void LeaveRoom()
    {
        if (NetworkClient.Lobby.IsOwner)
        {
            _lobbyUI._buttonStarGame.SetActive(false);
        }

        NetworkClient.Lobby.LeaveRoom((successful, error) =>
        {
            if (successful)
            {
                Debug.Log("Left room");
                _lobbyUI._panelInRoom.SetActive(false);
                _lobbyUI._panelRooms.SetActive(true);
                GetRooms();
                _lobbyUI.clearPlayersList();
            }
            else
            {
                Debug.Log("Failed to leave room " + error);
            }
        });
    }

    void Lobby_OnNewPlayerJoinRoomEvent(SWJoinRoomEventData eventData)
    {
        Debug.Log("Player joined room");
        Debug.Log(eventData);

        if (NetworkClient.Lobby.IsOwner)
        {

            // Update the room custom data
            NetworkClient.Lobby.ChangeRoomCustomData(eventData, (bool successful, SWLobbyError error) =>
            {
                if (successful)
                {
                    Debug.Log("ChangeRoomCustomData successful");
                    _lobbyUI.crearNewItemInPlayerList(eventData.data, eventData.newPlayerId);
                }
                else
                {
                    Debug.Log("ChangeRoomCustomData failed: " + error);
                }
            });
        }
    }

    void Lobby_OnPlayerLeaveRoomEvent(SWLeaveRoomEventData eventData)
    {
        Debug.Log("Player left room: " + eventData);

        if (NetworkClient.Lobby.IsOwner)
        {
            // Update the room custom data
            NetworkClient.Lobby.ChangeRoomCustomData(eventData, (bool successful, SWLobbyError error) =>
            {
                if (successful)
                {
                    Debug.Log("ChangeRoomCustomData successful");
                    GetPlayersInCurrentRoom();
                }
                else
                {
                    Debug.Log("ChangeRoomCustomData failed: " + error);
                }
            });
        }
    }

    void Lobby_OnNewRoomOwnerEvent(SWRoomChangeOwnerEventData eventData)
    {
        Debug.Log("Room owner changed: roomId= " + eventData.roomId + " newOwnerId= " + eventData.newOwnerId);
        if (NetworkClient.Lobby.IsOwner)
        {
            _lobbyUI._buttonStarGame.SetActive(true);
        }
    }

    public void StartGame()
    {
        if (NetworkClient.Lobby.IsOwner)
        {
            NetworkClient.Lobby.StartRoom((okay, error) =>
            {
                if (okay)
                {
                    // Lobby server has sent request to SocketWeaver. The request is being processed.
                    // If socketweaver finds suitable server, Lobby server will invoke the OnRoomReadyEvent.
                    // If socketweaver cannot find suitable server, Lobby server will invoke the OnFailedToStartRoomEvent.
                    Debug.Log("Started room");
                }
                else
                {
                    Debug.Log("Failed to start room " + error);
                }
            });
        }
    }

    void Lobby_OnRoomReadyEvent(SWRoomReadyEventData eventData)
    {
        Debug.Log("Room is ready: roomId= " + eventData.roomId);
        NetworkClient.Instance.ConnectToRoom(ConnectedToRoom);
    }

    void ConnectedToRoom(bool connected)
    {
        if (connected)
        {
            Debug.Log("Connected to room");
            SceneManager.LoadScene("Arena");
        }
        else
        {
            Debug.Log("Failed to connect to room");
        }
    }
}
