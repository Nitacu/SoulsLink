using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class ControlSpawnEnemys : MonoBehaviour
{
    private float _lifeEnemys = 150;
    [SerializeField] private int _numberEnemys = 6;
    public SceneSpawner spawner;

    public void spawnRandomEnemy()
    {
        if (NetworkClient.Instance.IsHost)
        {
            int random = Random.Range(0, spawner.NumberOfSpawnPoints);
            int randomEnemy = Random.Range(0, spawner.NumberOfNonPlayerPrefabs);
            NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(randomEnemy, random);
        }

    }

    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        if (NetworkClient.Instance.IsHost)
        {
            for (int i = 0; i < spawner.NumberOfSpawnPoints; i++)
            {
                int random = Random.Range(0, spawner.NumberOfNonPlayerPrefabs);
                NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(random, i);
            }
        }
    }

    public int NumberEnemys { get => _numberEnemys; set => _numberEnemys = value; }
}
