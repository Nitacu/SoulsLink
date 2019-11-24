using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class ControlSpawnEnemys : MonoBehaviour
{
    private float _lifeEnemys = 150;
    [SerializeField] private int _numberEnemys = 4;
    public SceneSpawner spawner;

    public void spawnRandomEnemy()
    {
        if (NetworkClient.Instance.IsHost)
        {
            if (_numberEnemys > 0)
            {
                int random = Random.Range(0, spawner.NumberOfSpawnPoints);

                NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(0, random);
            }
        }

    }

    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        if (NetworkClient.Instance.IsHost)
        {
            for (int i = 0; i < spawner.NumberOfSpawnPoints; i++)
            {
                _numberEnemys--;
                NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(0, i);
            }
        }
    }
}
