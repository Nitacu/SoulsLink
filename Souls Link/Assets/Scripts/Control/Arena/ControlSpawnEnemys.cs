using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using Photon.Pun;

public class ControlSpawnEnemys : MonoBehaviour
{
    [SerializeField] private int _numberEnemys;
    public SceneSpawner spawner;
    [Header("Cosas del Photon")]
    public PhotonView _photonView;
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private List<string> _enemys = new List<string>();

    private void Start()
    {
        onSpawnPlayerWithPhoton();
    }

    //para crear uno nuevo a medida que los van matando
    public void spawnRandomEnemy()
    {
        if (GameManager.GetInstace()._multiplayerServer == GameManager.MultiplayerServer.SOCKETWEAVER)
        {
            if (NetworkClient.Instance.IsHost)
            {
                int random = Random.Range(0, spawner.NumberOfSpawnPoints);
                int randomEnemy = Random.Range(0, spawner.NumberOfNonPlayerPrefabs);
                NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(randomEnemy, random);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int random = Random.Range(0, _points.Count);
                int randomEnemy = Random.Range(0, _enemys.Count);
                PhotonNetwork.Instantiate(_enemys[randomEnemy], _points[random].position, Quaternion.identity, 0);

            }
        }
    }

    //cuando inica el juego
    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        if (GameManager.GetInstace()._multiplayerServer == GameManager.MultiplayerServer.SOCKETWEAVER)
        {
            if (NetworkClient.Instance.IsHost)
            {
                for (int i = 0; i < _numberEnemys; i++)
                {
                    int random = Random.Range(0, spawner.NumberOfNonPlayerPrefabs);
                    NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(random, i);
                }
            }
        }

    }

    public void onSpawnPlayerWithPhoton()
    {
        if (GameManager.GetInstace()._multiplayerServer == GameManager.MultiplayerServer.PHOTON)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _numberEnemys; i++)
                {
                    int random = Random.Range(0, _enemys.Count);
                    PhotonNetwork.Instantiate(_enemys[random], _points[i].position, Quaternion.identity, 0);
                }
            }
        }
    }

    public int NumberEnemys { get => _numberEnemys; set => _numberEnemys = value; }
}
