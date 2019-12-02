using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameSceneManager : MonoBehaviour
{
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

    public void onSpawnPlayerWithPhoton()
    {
        PhotonNetwork.Instantiate(_characters[characterSelectedIndex], _points[characterSelectedIndex].position, Quaternion.identity, 0);
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
