using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class GameSceneManager : MonoBehaviour
{
    public SceneSpawner spawner;

    public void onSpawnerReady(bool alreadyFinishedSceneSetup)
    {
        int characterSelectedIndex = setCharacterToSpawn();

        if (!alreadyFinishedSceneSetup)
        {
            NetworkClient.Instance.LastSpawner.SpawnForPlayer(characterSelectedIndex, new Vector3(0, 0, 0), Quaternion.identity);

            if (NetworkClient.Instance.IsHost)
            {
                for (int i = 0; i < spawner.NumberOfSpawnPoints; i++)
                    NetworkClient.Instance.LastSpawner.SpawnForNonPlayer(0, i);
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
