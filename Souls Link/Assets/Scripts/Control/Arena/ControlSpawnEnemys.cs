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

            Debug.Log("as");
            int random = Random.Range(0, spawner.NumberOfSpawnPoints);
            NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(0, random);
        }

    }

    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        if (NetworkClient.Instance.IsHost)
        {
            for (int i = 0; i < spawner.NumberOfSpawnPoints; i++)
            {
                NumberEnemys--;
                NetworkClient.Instance.FindSpawner(1).SpawnForNonPlayer(0, i);
            }
        }
    }

    public int NumberEnemys { get => _numberEnemys; set => _numberEnemys = value; }
}
