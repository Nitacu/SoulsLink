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
            NetworkClient.Instance.FindSpawner(2).SpawnForPlayer(characterSelectedIndex, characterSelectedIndex);
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
            case GameManager.Characters.MAGE:
                return 1;
            case GameManager.Characters.DRUID:
                return 2;
            case GameManager.Characters.ASSASIN:
                return 3;
            default:
                return 0;
        }
    }
}
