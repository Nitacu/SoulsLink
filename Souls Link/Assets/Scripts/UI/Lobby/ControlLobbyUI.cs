﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControlLobbyUI : MonoBehaviour
{
    //registro
    public GameObject _panelRegistry;
    public TMP_InputField _inputFieldRegister;
    //Muestra las salas
    public GameObject _panelRooms; // donde esta todo
    public GameObject _fatherRooms; // objeto que lo ordena como una lista
    public GameObject _buttonNameroom; // prefab
    //panel donde se crea un nueva sala
    public GameObject _panelCreatedRoom; //lo contiene todo
    public TMP_InputField _inputFieldRoomName; // donde se escribe el nombre 
    //las cosas nombres de los players en la sala
    public GameObject _fatherPlayerNames; //ordena los objetos como una lista
    public List<GameObject> _imagenPlayerName;  //prefab
    //Dentro de una sala
    public GameObject _panelInRoom;
    public GameObject _buttonStarGame;

    #region delegate
    public delegate int PlayersNumberDelegate();
    public PlayersNumberDelegate playersNumber;
    #endregion

    public bool useOldVersion = true;

    public void refreshPlayerList()
    {

    }

    public void addNewPlayerToRoom()
    {
        if (!useOldVersion)
        {
            _panelInRoom.GetComponent<SelectCharacter>().addPlayerUpdate();
        }
    }

    public void crearNewItemInPlayerList(string name, string id)
    {
        _panelRooms.SetActive(false);

        _panelInRoom.SetActive(true);

        if (useOldVersion)
        {
            foreach (GameObject player in _imagenPlayerName)
            {
                if (!player.activeSelf)
                {
                    player.SetActive(true);
                    player.GetComponentInChildren<TMP_Text>().text = name; //show name
                    player.GetComponent<PlayerSelectCharPanel>().PlayerID = id;//assign id
                    Debug.Log("Player ID setted");
                    break;
                }
            }
        }
    }

    public void clearPlayersList()
    {
        if (useOldVersion)
        {
            foreach (GameObject player in _imagenPlayerName)
            {
                player.SetActive(false);
            }
        }
        else
        {
            _panelInRoom.GetComponent<SelectCharacter>().onLeaveRoom();
        }

    }

    public void crearNewItemInRoomList(string name, string ID)
    {
        GameObject auxRoom;
        auxRoom = Instantiate(_buttonNameroom, _fatherRooms.transform);
        auxRoom.GetComponentInChildren<TMP_Text>().text = name;
        auxRoom.GetComponent<ConnectRoom>()._id = ID;

    }

    public void deletedItemRoom(string ID)
    {
        ConnectRoom[] connectRooms = FindObjectsOfType<ConnectRoom>();

        foreach (ConnectRoom room in connectRooms)
        {
            if (room._id == ID)
            {
                Destroy(room.gameObject);
                return;
            }
        }
    }

    public void clearListRooms()
    {
        List<Button> aux = _fatherRooms.GetComponentsInChildren<Button>().ToList();

        while (aux.Count > 0)
        {
            Destroy(aux[0].gameObject);
            aux.RemoveAt(0);
        }
    }

    public string nameOfTheNewRoom()
    {
        string aux = _inputFieldRoomName.text;
        _inputFieldRoomName.text = "";
        return aux;
    }

    public string registerUser()
    {
        _panelRegistry.SetActive(false);
        return _inputFieldRegister.text;
    }

    public void enterLobby()
    {
        _inputFieldRegister.text = "";
        _panelRooms.SetActive(true);
        _panelInRoom.SetActive(false);
        _panelRegistry.SetActive(false);
    }

    public void showPanelCreatedRoom(bool active)
    {
        if (active)
        {
            _inputFieldRoomName.text = "";
            _panelRooms.SetActive(false);
        }
        else
        {
            _panelRooms.SetActive(true);
        }

        _panelCreatedRoom.SetActive(active);
    }

    public int getPlayersInRoom()
    {
        return playersNumber();
    }

}
