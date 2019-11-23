using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class ControlSpawnEnemys : MonoBehaviour
{
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();
    private float _lifeEnemys = 150;

    public void spawnRandomEnemy()
    {
        /*
        if (GetComponent<NetworkID>().OwnerCustomPlayerId == GetComponent<NetworkID>().OwnerRemotePlayerId)
        {
            int random = Random.Range(0, _spawnPoints.Count);

            NetworkClient.Instance.LastSpawner.SpawnForNonPlayer(0, random).GetComponent<SimpleEnemyController>().health = _lifeEnemys;
            _lifeEnemys += 20;

        }
        */
    }

    public void spawnNewEnemy()
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            NetworkClient.Instance.LastSpawner.SpawnForNonPlayer(0, i);
        }
    }
}
