using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class GameSceneManager : MonoBehaviour
{
    SceneSpawner spawner;
    private GameObject player;

    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        int characterSelectedIndex = setCharacterToSpawn();
        
        if (player == null)
        {
            player = NetworkClient.Instance.LastSpawner.SpawnForPlayer(characterSelectedIndex, new Vector3(0,0,0),Quaternion.identity);

            if (!alreadyFinishedSceneSetup && NetworkClient.Instance.IsHost)
            {
                GetComponent<ControlSpawnEnemys>().spawnNewEnemy();
            }
        } 
        
        NetworkClient.Instance.LastSpawner.PlayerFinishedSceneSetup();
    }

    private int setCharacterToSpawn()
    {        
        GameManager.Characters characterSelected = GameManager.GetInstace()._myCharacter;        

        switch (characterSelected)
        {
            case GameManager.Characters.TANK:
                return 0;
                break;
            case GameManager.Characters.MAGE:
                return 1;
                break;
            case GameManager.Characters.DRUID:
                return 2;
                break;
            case GameManager.Characters.ASSASIN:
                return 3;
                break;
            default:
                return 0;
                break;
        }
    }
}
