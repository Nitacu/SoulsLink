using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerFollowing : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private bool playerDead = false;
    public bool PlayerDead
    {
        get { return playerDead; }
        set { playerDead = value; }
    }

    private void Start()
    {
        //if player in pc == 1
        StartCoroutine(findPlayersAgain(0));
        //else
        //Turn player2camera off, Turn player1Camera on, turn screen divider on, set players on split screen script, 
        //turn split screen script on, disable SinglePlayerFollowingScript

    }

    IEnumerator findPlayersAgain(float timeToStart)
    {
        yield return new WaitForSeconds(timeToStart);

        PlayerMovement[] _players = FindObjectsOfType<PlayerMovement>();

        if (_players.Length <= 0)
        {
            StartCoroutine(findPlayersAgain(0.2f));
        }
        else
        {
            assignTarget(_players);
        }
    }

    private void assignTarget(PlayerMovement[] playerFinded)
    {
        bool playerFound = false;

        foreach (var player in playerFinded)
        {
            if (player._isMine())
            {
                target = player.gameObject;
                playerFound = true;
            }
        }

        if (!playerFound)
        {
            StartCoroutine(findPlayersAgain(0.2f));
        }
    }

    private void Update()
    {
        if (playerDead)
        {
            return;
        }

        //Follow Player
        if (target != null)
        {
            gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, gameObject.transform.position.z);
        }
        else
        {
            Debug.Log("Find player");
            StartCoroutine(findPlayersAgain(0));
        }

    }
}
