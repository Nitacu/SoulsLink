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

        if (_players.Count == 2)
        {
            FusionarPlayers();
        }
    }

    private void FusionarPlayers()
    {                
        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().DeactivateComponentsOnFusion();
        }

        StartCoroutine(createChimera());
    }

    IEnumerator createChimera()
    {
        yield return new WaitForEndOfFrame();

        Debug.Log("Fusionar");

        //Calcular punto medio

        float xPos = midPoint(_players[HOST_INDEX].transform.position.x, _players[HOST_INDEX + 1].transform.position.x);
        float yPos = midPoint(_players[HOST_INDEX].transform.position.y, _players[HOST_INDEX + 1].transform.position.y);
        Vector2 instPos = new Vector2(xPos, yPos);

        //Crear chimera

        GameObject _chimera = Instantiate(_chimeraPrefab);
        _chimera.transform.position = instPos;

        ChimeraController chimeraController = _chimera.GetComponent<ChimeraController>();
        chimeraController.setPlayersInFusion(_players);
    }

    private float midPoint(float point1, float point2)
    {
        return (point1 + point2) / 2;
    }

    public bool playerIsAttached(GameObject player)
    {
        if (_players.Contains(player)) Debug.Log(player.name + " is already on fusion");
        return _players.Contains(player);
    }
}
