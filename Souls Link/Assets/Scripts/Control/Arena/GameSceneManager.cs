using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using Photon.Pun;

public class GameSceneManager : MonoBehaviour
{
    public SceneSpawner spawner;
    private int characterSelectedIndex;
    [Header("Cosas del Photon")]
    public PhotonView _photonView;
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private List<string> _characters = new List<string>();

    private void Awake()
    {
        characterSelectedIndex = setCharacterToSpawn();
    }

    private void Start()
    {
        onSpawnPlayerWithPhoton();
    }

    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        if (GameManager.GetInstace()._multiplayerServer == GameManager.MultiplayerServer.SOCKETWEAVER)
        {
            if (!alreadyFinishedSceneSetup)
            {
                NetworkClient.Instance.FindSpawner(2).SpawnForPlayer(characterSelectedIndex, characterSelectedIndex);
            }

            NetworkClient.Instance.LastSpawner.PlayerFinishedSceneSetup();
        }
    }

    public void onSpawnPlayerWithPhoton()
    {
        if (GameManager.GetInstace()._multiplayerServer == GameManager.MultiplayerServer.PHOTON)
        {
            PhotonNetwork.Instantiate(_characters[characterSelectedIndex], _points[characterSelectedIndex].position, Quaternion.identity, 0);
        }
    }

    private int setCharacterToSpawn()
    {
        GameManager.Characters characterSelected = GameManager.GetInstace()._myCharacter;

        switch (characterSelected)
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
