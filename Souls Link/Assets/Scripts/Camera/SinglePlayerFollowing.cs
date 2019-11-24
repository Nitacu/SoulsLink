using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerFollowing : MonoBehaviour
{
    [SerializeField]private GameObject target;

    private void Start()
    {
        StartCoroutine(findPlayersAgain(0));       
    }

    IEnumerator findPlayersAgain(float timeToStart)
    {
        yield return new WaitForSeconds(timeToStart);

        CharacterMultiplayerController[] _players = FindObjectsOfType<CharacterMultiplayerController>();

        if (_players.Length <= 0)
        {
            StartCoroutine(findPlayersAgain(0.2f));
        }
        else
        {
            assignTarget(_players);
        }
    }

    private void assignTarget(CharacterMultiplayerController[] playerFinded)
    {
        bool playerFound = false;

        foreach (var player in playerFinded)
        {
            if (player.isMine())
            {
                target = player.gameObject;
                playerFound = true;
                Debug.Log("Player found: " + target.name);
            }
        }

        if (!playerFound)
        {
            Debug.Log("Player not found - Try Again");
            StartCoroutine(findPlayersAgain(0.2f));
        }
    }

    private void Update()
    {
        //Follow Player
        if (target != null)
        {
            Debug.Log("Move Camera");
            gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, gameObject.transform.position.z);
        }

    }
}
