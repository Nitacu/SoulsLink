using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SWNetwork;
using UnityEngine.UI;

public class LobbyKevin : MonoBehaviour
{
    public TMP_InputField _inputField;

    public Button _buttonRegister;
    public Button _buttonCreatedRoom;

    private void Start()
    {
        NetworkClient.Lobby.OnLobbyConnectedEvent += OnLobbyConnectedEvent;
    }

    private void OnDestroy()
    {
        NetworkClient.Lobby.OnLobbyConnectedEvent -= OnLobbyConnectedEvent;
    }

    public void checkIn()
    {
        //checking
        NetworkClient.Instance.CheckIn(_inputField.text, (bool successful, string error) =>
        {
            if (!successful)
            {
                Debug.LogError(error);
            }
            else
            {
                Debug.Log("se registro " + _inputField.text);
            }
        });
   
    }

    public void OnLobbyConnectedEvent()
    {
        
        //entra al lobby
        NetworkClient.Lobby.Register(_inputField.text, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Lobby registered " + reply);

                _inputField.text = "";
                _buttonRegister.gameObject.SetActive(false);
                _buttonCreatedRoom.gameObject.SetActive(true);

                if (reply.started)
                {
                    // si el jugador ya esta en una room y esta ha comenzado la partida
                    // Call NetworkClient.Instance.ConnectToRoom to connect to the game servers of the room.
                }
                else if (reply.roomId != null)
                {
                    // Player is in a room.
                    GetRooms();
                    //GetPlayersInCurrentRoom();
                }
                else
                {
                    // Player is not in a room.
                    //GetRooms();
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
        NetworkClient.Lobby.CreateRoom(_inputField.text, true, 4, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Room created " + reply);

                _inputField.gameObject.SetActive(false);
                _buttonRegister.gameObject.SetActive(false);
                _buttonCreatedRoom.gameObject.SetActive(false);

                // refresh the room list
                GetRooms();

                // refresh the player list
                //GetPlayersInCurrentRoom();
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

                //muestra los rooms que ya estan creados

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

                // store the playerIds and player names in a dictionary.
                // The dictionary is later used to populate the player list.
            }
            else
            {
                Debug.Log("Failed to get players " + error);
            }
        });
    }

}
