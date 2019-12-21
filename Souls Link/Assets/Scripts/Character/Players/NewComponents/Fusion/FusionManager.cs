using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class FusionManager : MonoBehaviour
{
    [SerializeField] private float _distanceToFusion = 3;

    public GameObject[] _playersToFusion = new GameObject[4] { null, null, null, null };

    #region ChimerasPrefabs
    [SerializeField] private GameObject _chimeraTwoPlayersBase;

    [SerializeField] private List<GameObject> _chimeras = new List<GameObject>();

    #endregion


    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isHost;
    public delegate GameObject DelegateMultiplayerControllerCreatedChimera();
    public DelegateMultiplayerControllerCreatedChimera _createdChimera;
    #endregion

    private void Update()
    {

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

                for (int j = 0; j < _playersToFusion.Length; j++)
                {
                    if (_playersToFusion[j] != null)
                    {
                        Vector2 otherPlayerPosition = _playersToFusion[j].transform.position;
                        float distance = Vector2.Distance(currentPlayerPosition, otherPlayerPosition);

                        //Guarda con quienes no se puede fusionar

                        if (distance > _distanceToFusion)
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

        //Sacar players repetidos
        List<GameManager.Characters> _playersTypes = new List<GameManager.Characters>();
        List<GameObject> _playersNonRepeated = new List<GameObject>();
        foreach (var item in _playersCanFusion)
        {
            if (!_playersTypes.Contains(item.GetComponent<FusionTrigger>()._characterType))
            {
                _playersNonRepeated.Add(item);
                _playersTypes.Add(item.GetComponent<FusionTrigger>()._characterType);
            }
        }

        //Sacar players que no se pueden fusionar
        List<GameObject> _playersAvailableToFusion = new List<GameObject>();
        foreach (var player in _playersNonRepeated)
        {
            if (player.GetComponent<FusionTrigger>().availableToFusion())
            {
                _playersAvailableToFusion.Add(player);
            }
        }

        //Ver qué jugadores se pueden fusioanar
        if (_playersAvailableToFusion.Count >= 2)
        {
            if (playersWithDifferentParent(_playersAvailableToFusion))
            {
                FusionarPlayers(_playersAvailableToFusion);
            }
        }

    }

    private bool playersWithDifferentParent(List<GameObject> _players)
    {
        //Si alguno que no tiene padre
        foreach (var player in _players)
        {
            if ((!player.GetComponent<FusionTrigger>().IsOnFusion) && player.GetComponent<FusionTrigger>().CurrentChimeraParent == null)
            {
                return true;
            }
        }

        List<GameObject> _parents = new List<GameObject>();
        //que tengan diferente padre
        foreach (var player in _players)
        {
            FusionTrigger trigger = player.GetComponent<FusionTrigger>();

            if (!_parents.Contains(trigger.CurrentChimeraParent.gameObject))
            {
                _parents.Add(trigger.CurrentChimeraParent.gameObject);
            }
        }

        if (_parents.Count > 1)
        {
            return true;
        }

        return false;
    }

    private void FusionarPlayers(List<GameObject> _players)
    {
        Debug.Log("FUSIONAR PLAYER, PREGUNTA SI ES HOST");

        if (_isHost())
            StartCoroutine(createChimera(_players));
    }

    IEnumerator createChimera(List<GameObject> _players)
    {
        yield return new WaitForEndOfFrame();

        if (_isHost())
        {
            Debug.Log("SOY HOST - CREAR CHIMERA");

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

            //Saber qué chimera crear
            GameObject chimeraTocreate = selectCorrectChimera(_players);
            string chimeraName = chimeraTocreate.name;

            //Crear chimera   
            GameObject _chimera = PhotonNetwork.Instantiate(chimeraName, newPos, Quaternion.identity);
            _chimera.transform.position = newPos;

            foreach (var player in _players)
            {
                player.GetComponent<FusionTrigger>().IsOnFusion = true;
            }

            ChimeraController chimeraController = _chimera.GetComponent<ChimeraController>();

            //crear cadena de ids
            string ids = _players[0].GetComponent<FusionTrigger>()._myID();

            for (int i = 1; i < _players.Count; i++)
            {
                string newId = "#" + _players[i].GetComponent<FusionTrigger>()._myID();
                ids += newId;
            }

            //Se debe llamar en los demás también
            chimeraController.setPlayersInFusion(ids);//local

            StartCoroutine(waitForSetPlayer(chimeraController, ids));//para todas las maquinas
        }
    }

    IEnumerator waitForSetPlayer(ChimeraController chimeraController, string ids)
    {
        yield return new WaitForEndOfFrame();
        chimeraController.GetComponent<PhotonChimeraMultiplayerController>().addDelegate();
        chimeraController._setPlayersInFusion(ids);
    }

    public GameObject selectCorrectChimera(List<GameObject> players)
    {
        List<GameManager.Characters> playerCharacterType = new List<GameManager.Characters>();
        foreach (var player in players)
        {
            playerCharacterType.Add(player.GetComponent<FusionTrigger>()._characterType);
        }

        GameObject chimeraToCreate = null;

        foreach (var chimera in _chimeras)
        {
            if (CompareList(chimera.GetComponent<ChimeraTypes>()._types, playerCharacterType))
            {
                chimeraToCreate = chimera;
                return chimeraToCreate;
            }
        }

        return _chimeraTwoPlayersBase;
    }

    public bool CompareList(List<GameManager.Characters> list1, List<GameManager.Characters> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }

        foreach (var item in list1)
        {
            if (!list2.Contains(item))
            {
                return false;
            }
        }

        foreach (var item in list2)
        {
            if (!list1.Contains(item))
            {
                return false;
            }
        }

        return true;
    }

    public void addMeToFusion(GameObject player)
    {
        if (!_playersToFusion.ToList().Contains(player))
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

        verifyPLayerSizeToFusion();

    }

    private void verifyPLayerSizeToFusion()
    {
        int count = 0;

        //recorre arreglo players para saber cuántos hay en el frame
        foreach (var player in _playersToFusion)
        {
            if (player != null)
            {
                count++;
            }
        }

        //si hay dos o más procede a ver si se pueden fusionar
        if (count >= 2)
        {
            checkPlayers();
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