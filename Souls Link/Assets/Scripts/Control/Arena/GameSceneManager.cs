using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class GameSceneManager : MonoBehaviour
{
    #region CAMERAS
    [SerializeField] private GameObject _camera1;
    [SerializeField] private GameObject _camera2;

    [SerializeField] private SplitCameraController _splitCamera;
    #endregion

    private int characterSelectedIndex;
    [Header("Cosas del Photon")]
    public PhotonView _photonView;
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private List<string> _characters = new List<string>();

    private void Awake()
    {
        //characterSelectedIndex = setCharacterToSpawn();
    }

    private void Start()
    {
        onSpawnPlayerWithPhoton();

        setCoopCamera(true);
    }

    public void onSpawnPlayerWithPhoton()
    {
        List<GameObject> _players = new List<GameObject>();


        ////NEW
        var characterSetTup = GameManager.GetInstace()._charactersSetupList;

        foreach (var character in characterSetTup)
        {
            if (character.characterType != GameManager.Characters.NONE)
            {
                int index = setCharacterToSpawn(character.characterType);
                GameObject player = PhotonNetwork.Instantiate(_characters[index], _points[index].position, Quaternion.identity, 0);

                InputDevice devices = InputSystem.GetDevice(character.device);
                player.GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentControlScheme(character.scheme, devices);
                _players.Add(player);
            }
        }

        /////OLD  
        /*
        foreach (var character in GameManager.GetInstace()._charactersList)
        {
            if (character != GameManager.Characters.NONE)
            {
                int index = setCharacterToSpawn(character);
                GameObject player = PhotonNetwork.Instantiate(_characters[index], _points[index].position, Quaternion.identity, 0);
                _players.Add(player);
            }
        }
        */

        _players[0].GetComponent<SetHUDController>().setLeftHUD();

        if (_players.Count >= 2)
        {
            _splitCamera.player1 = _players[0].transform;
            _splitCamera.player2 = _players[1].transform;

            _players[1].GetComponent<SetHUDController>().setRightHUD();
        }

    }

    public void setCoopCamera(bool active)
    {
        //saber cuantos players hay

        var characterSetTup = GameManager.GetInstace()._charactersSetupList;

        int count = 0;
        foreach (var item in characterSetTup)
        {
            if (item.characterType != GameManager.Characters.NONE)
            {
                count++;
            }
        }

        if (count >= 2)
        {
            _splitCamera.enabled = active;

            _camera1.SetActive(active);
            _camera2.GetComponent<SinglePlayerFollowing>().enabled = !active;

            if (!active)
            {
                _splitCamera.screenDivider.SetActive(false);
            }
        }
    }

    private int setCharacterToSpawn(GameManager.Characters _charSet)
    {
        switch (_charSet)
        {
            case GameManager.Characters.TANK:
                return 0;
            case GameManager.Characters.MAGE:
                return 1;
            case GameManager.Characters.DRUID:
                return 2;
            case GameManager.Characters.ASSASIN:
                return 3;
            default:
                return 0;
        }
    }
}
