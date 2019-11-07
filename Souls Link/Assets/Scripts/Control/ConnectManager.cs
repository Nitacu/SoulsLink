using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using TMPro;

public class ConnectManager : MonoBehaviour
{

    [SerializeField] private TMP_InputField _inputField;

    private void Start()
    {
        NetworkClient.Lobby.OnLobbyConnectedEvent += registerPlayer;
    }

    public void registerPlayer()
    {
        string customPlayerId = _inputField.text;

        if (customPlayerId != null && customPlayerId.Length > 0)
        {
            // use the user entered playerId to check into SocketWeaver. Make sure the PlayerId is unique.
            NetworkClient.Instance.CheckIn(customPlayerId, (bool ok, string error) =>
            {
                if (!ok)
                {
                    Debug.LogError("Check-in failed: " + error);
                }
            });
        }
        else
        {
            // use a randomly generated playerId to check into SocketWeaver.
            NetworkClient.Instance.CheckIn((bool ok, string error) =>
            {
                if (!ok)
                {
                    Debug.LogError("Check-in failed: " + error);
                }
            });
        }
    }

}
