using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayersInSight : MonoBehaviour
{

    public Transform _target;
    public float _turretRange = 5f;
    private List<GameObject> playerList = new List<GameObject>();
    public LayerMask playerLayer;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

    }

    void UpdateTarget()
    {
        playerList.Clear();
        //Find all enemies
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (GameObject player in players)
        {
            //check if has layer player
            if(player.layer == playerLayer)
            {
                playerList.Add(player);
            }

        }

        //Check in enemies which has the closest distance to turret to make him a target
        foreach (GameObject player in playerList)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestPlayer = player;
            }
        }

        //set target to enemy
        if (nearestPlayer != null && shortestDistance <= _turretRange)
        {
            _target = nearestPlayer.transform;
        }
        else
        {
            _target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            return;
        }

    }

    private void findPlayers()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _turretRange);
    }
}
