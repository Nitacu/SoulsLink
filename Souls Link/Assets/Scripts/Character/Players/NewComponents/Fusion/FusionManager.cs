using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour
{
    [SerializeField] private float _distanceToFusion = 10;
    [SerializeField] private GameObject _chimeraPrefab;

    private GameObject[] _playersToFusion = new GameObject[4] { null, null, null, null };

    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isHost;
    #endregion

    private void Update()
    {
        int count = 0;

        foreach (var player in _playersToFusion)
        {
            if (player != null)
            {
                count++;
            }
        }

        if (count >= 2)
        {
            checkPlayers();
        }
    }

    private void checkPlayers()
    {
        List<GameObject> _playersCanFusion = new List<GameObject>();

        //Evaluar distancia de todos con todos
        for (int i = 0; i < _playersToFusion.Length; i++)
        {
            if (_playersToFusion[i] != null)
            {
                List<GameObject> _unMatchedPlayers = new List<GameObject>();

                Vector2 currentPlayerPosition = _playersToFusion[i].transform.position;

                //bool canFusionWithSomeOne = false;

                for (int j = 0; j < _playersToFusion.Length; j++)
                {
                    if (_playersToFusion[j] != null)
                    {
                        Vector2 otherPlayerPosition = _playersToFusion[j].transform.position;
                        float distance = Vector2.Distance(currentPlayerPosition, otherPlayerPosition);

                        if (distance <= _distanceToFusion)
                        {
                            //canFusionWithSomeOne = true;
                        }
                        else
                        {
                            _unMatchedPlayers.Add(_playersToFusion[j]);
                        }
                    }
                }


                //Analizar en lista de player que se pueden fusionar
                if (_unMatchedPlayers.Count == 0)//Matcheo con todos se puede fusionar
                {
                    _playersCanFusion.Add(_playersToFusion[i]);
                }
                else
                {
                    //Evaluar si los que no matchearon conmigo pueden fusionarse, si están, yo no podré

                    bool noUnMatchedPlayers = true;

                    foreach (var unMatchedPlayer in _unMatchedPlayers)
                    {
                        if (_playersCanFusion.Contains(unMatchedPlayer))
                        {
                            noUnMatchedPlayers = false;
                            break;
                        }
                    }

                    //si no hay unmatches players en la lista añadirme a fusionar
                    if (noUnMatchedPlayers)
                    {
                        _playersCanFusion.Add(_playersToFusion[i]);
                    }
                }
            }
        }

        //Ver qué jugadores se pueden fusioanar
        if (_playersCanFusion.Count >= 2)
        {
            FusionarPlayers(_playersCanFusion);
        }

    }

    private void FusionarPlayers(List<GameObject> _players)
    {
        //sacar jugadores del host
        for (int i = 0; i < _playersToFusion.Length; i++)
        {
            if (_players.Contains(_playersToFusion[i]))
            {
                _playersToFusion[i] = null;
            }
        }


        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().DeactivateComponentsOnFusion();
        }

        if (_isHost())
            StartCoroutine(createChimera(_players));
    }

    IEnumerator createChimera(List<GameObject> _players)
    {
        yield return new WaitForEndOfFrame();

        //Calcular punto medio

        float xPos = 0;
        float yPos = 0;

        foreach (var player in _players)
        {
            xPos += player.transform.position.x;
            yPos += player.transform.position.y;
        }

        xPos = xPos / _players.Count;
        yPos = yPos / _players.Count;

        Vector2 newPos = new Vector2(xPos, yPos);

        //Crear chimera

        GameObject _chimera = Instantiate(_chimeraPrefab);
        _chimera.transform.position = newPos;

        ChimeraController chimeraController = _chimera.GetComponent<ChimeraController>();
        chimeraController.setPlayersInFusion(_players);
    }

    public void addMeToFusion(GameObject player)
    {
        for (int i = 0; i < _playersToFusion.Length; i++)
        {
            if (_playersToFusion[i] == null)
            {
                _playersToFusion[i] = player;
                break;
            }
        }
    }

    public void getOutToFusion(GameObject player)
    {
        for (int i = 0; i < _playersToFusion.Length; i++)
        {
            if (_playersToFusion[i] != null)
            {
                if (player == _playersToFusion[i])
                {
                    _playersToFusion[i] = null;
                    break;
                }
            }
        }
    }

    private float midPoint(float point1, float point2)
    {
        return (point1 + point2) / 2;
    }
}