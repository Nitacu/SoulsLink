using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class GameSceneManager : MonoBehaviour
{
    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        if (!alreadyFinishedSceneSetup)
        {
            NetworkClient.Instance.LastSpawner.SpawnForPlayer(0,new Vector3(0,0,0),Quaternion.identity);

            NetworkClient.Instance.LastSpawner.PlayerFinishedSceneSetup();

        }
    }
}
