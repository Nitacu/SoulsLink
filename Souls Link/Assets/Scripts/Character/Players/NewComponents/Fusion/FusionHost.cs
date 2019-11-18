using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionHost : MonoBehaviour
{
    private const int HOST_INDEX = 0;
    private List<GameObject> _players = new List<GameObject>();

    [SerializeField] private GameObject _chimeraPrefab;

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
        GameObject _chimera = Instantiate(_chimeraPrefab);
        _chimera.transform.position = _players[HOST_INDEX].transform.position;
    }
}
