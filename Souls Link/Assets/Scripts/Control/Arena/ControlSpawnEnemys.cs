using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class ControlSpawnEnemys : MonoBehaviour
{
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();
    private float _lifeEnemys = 150;
    [SerializeField] private float _numberEnemys = 4;

    public void spawnRandomEnemy()
    {
        if (NetworkClient.Instance.IsHost)
        {
            if (FindObjectsOfType<EnemyMeleeMultiplayerController>().Length < _numberEnemys)
            {
                int random = Random.Range(0, _spawnPoints.Count);

                NetworkClient.Instance.LastSpawner.SpawnForNonPlayer(0, random);
            }
        }

    }
}
