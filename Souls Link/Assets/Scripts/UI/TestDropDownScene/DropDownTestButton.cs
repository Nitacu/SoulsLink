using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DropDownTestButton : MonoBehaviour
{
    public TMP_Dropdown _dropdown;

    public void pickAnswer()
    {
        if(_dropdown.value == 0)
        {
            GameManager.GetInstace()._multiplayerServer = GameManager.MultiplayerServer.PHOTON;
            SceneManager.LoadScene("LobbyPhoton");

        }else if(_dropdown.value == 1)
        {
            GameManager.GetInstace()._multiplayerServer = GameManager.MultiplayerServer.SOCKETWEAVER;
            SceneManager.LoadScene("Lobby_Kevin");
        }
    }
}
