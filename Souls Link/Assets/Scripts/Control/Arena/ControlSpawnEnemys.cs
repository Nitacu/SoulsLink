using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class ControlSpawnEnemys : MonoBehaviour
{
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] private GameObject _enemy;
    private float _lifeEnemys = 50;

    public void spawnRandomEnemy()
    {
        if (GetComponent<NetworkID>().OwnerCustomPlayerId == GetComponent<NetworkID>().OwnerRemotePlayerId)
        {
            int random = Random.Range(0, _spawnPoints.Count);

            NetworkClient.Instance.LastSpawner.SpawnForNonPlayer(0, _spawnPoints[random].position, Quaternion.identity).GetComponent<SimpleEnemyController>().health = _lifeEnemys;
            _lifeEnemys += 20;

        }
    }


    public void spawnNewEnemy()
    {
        if (GetComponent<NetworkID>().OwnerCustomPlayerId == GetComponent<NetworkID>().OwnerRemotePlayerId)
        {
            foreach (Transform spawn in _spawnPoints)
            {
                NetworkClient.Instance.LastSpawner.SpawnForNonPlayer(0, spawn.position, Quaternion.identity);
                _lifeEnemys += 20;
            }
        }
    }
}
