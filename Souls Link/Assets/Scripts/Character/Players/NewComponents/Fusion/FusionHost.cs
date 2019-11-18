using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionHost : MonoBehaviour
{
    private const int HOST_INDEX = 0;
    private List<GameObject> _players = new List<GameObject>();

    public void addPlayerToFusion(GameObject player)
    {
        _players.Add(player);

        if (_players.Count >= 2)
        {
            FusionarPlayers();
        }
    }

    private void FusionarPlayers()
    {
        Debug.Log("Fusionar Players");
    }
}
