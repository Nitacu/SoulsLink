using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    #region NAME SCENES
    public const string LOBBY_PHOTON_ONLINE = "LobbyPhoton";
    public const string LOBBY_PHOTON_LAN = "LobbyPhotonLAN";
    #endregion

    public enum Scenes
    {
        LOBBY_PHOTON_ONLINE,
        LOBBY_PHOTON_LAN
        
    }

    [SerializeField] private Scenes sceneToChange;

    public void changeScene()
    {
        switch (sceneToChange)
        {
            case Scenes.LOBBY_PHOTON_ONLINE:
                SceneManager.LoadScene(LOBBY_PHOTON_ONLINE);
                break;
            case Scenes.LOBBY_PHOTON_LAN:
                SceneManager.LoadScene(LOBBY_PHOTON_LAN);
                break;
        }
    }
}
