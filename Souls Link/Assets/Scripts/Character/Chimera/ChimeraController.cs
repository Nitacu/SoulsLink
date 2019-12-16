using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraController : PlayerMovement
{   

    [SerializeField] private List<GameObject> _players = new List<GameObject>(); //cuidado al usar esta lista antes de que el servidor la actualice 
    [SerializeField] private List<GameObject> _arrows = new List<GameObject>();

    [SerializeField] private Vector2 _movement;
    public Vector2 Movement
    {
        get { return Movement1; }
    }

    public Vector2 Movement1 { get => _movement; set => _movement = value; }

    [SerializeField] private Vector2[] _inputsMovements;

    bool[] _unFusionCheck;

    #region Delegate
    public delegate bool DelegateMultiplayerController();
    public DelegateMultiplayerController _isMine;
    public delegate void DelegateMultiplayerControllerSendPlayerInChimera();
    public DelegateMultiplayerControllerSendPlayerInChimera _sendPlayerInChimera;
    public delegate void DelegateMultiplayerControllerIDs(string ids);
    public DelegateMultiplayerControllerIDs _setPlayersInFusion;
    public delegate void DelegateMultiplayerControllerIMove(Vector2 movement, int id, GameManager.Characters type);
    public DelegateMultiplayerControllerIMove _sendMovement;
    #endregion    

    private void OnEnable()
    {
        GetComponent<PhotonChimeraMultiplayerController>().addDelegate();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        foreach (var arrow in _arrows)
        {
            arrow.SetActive(false);
        }

        foreach (var player in _players)
        {
            setArrows(Vector2.zero, player.GetComponent<FusionTrigger>()._characterType);
        }
    }


    #region OVERRIDE_METHODS
    protected override void Awake()
    {
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Update()
    {
        calculateNewMovement();
        move();
    }
    #endregion

    private void move()
    {
        if (!isDashing)
        {

            _rb.velocity = Movement1 * Time.deltaTime * _speed;

            setAnimation();
        }
        else
        {
            if (GetComponent<Dash>() != null)
            {
                if (!GetComponent<Dash>().isSimpleDash)
                {
                    GetComponent<Dash>().playerDash(GetComponent<Dash>().Aiming.AimDirection); //Dash asesino (el que dashea bastante como un rayo)
                }
                else
                {
                    GetComponent<CometDash>().playerDash(GetComponent<CometDash>().Aiming.AimDirection);
                }
            }
            else if (GetComponent<CometDash>() != null)
            {

                GetComponent<CometDash>().playerDash(GetComponent<CometDash>().Aiming.AimDirection); //Dash basico
            }
        }
    }

    private void setAnimation()
    {
        //RotarSprite
        if (Movement1.x > 0)
        {
            _renderer.flipX = false;
            _flip = false;
        }
        else if (Movement1.x < 0)
        {
            _renderer.flipX = true;
            _flip = true;
        }
    }

    public void sendMovement(Vector2 movement, int id, GameManager.Characters playerType)
    {
        Debug.Log("id " + id + " movemente " + movement);
        _inputsMovements[id] = movement;
    }

    private void calculateNewMovement()
    {
        Vector2 newinputMovement = Vector2.zero;
        foreach (Vector2 input in _inputsMovements)
        {
            newinputMovement += input;
        }

        Movement1 = newinputMovement;
    }

    public void setArrows(Vector2 direction, GameManager.Characters playerType)
    {
        if (_arrows.Count > 0)
        {
            foreach (var arrow in _arrows)
            {
                ChimeraArrow chimeraArrow = arrow.GetComponent<ChimeraArrow>();

                if (chimeraArrow.CharacterType == playerType)
                {
                    if (direction.magnitude > 0)
                    {
                        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        arrow.transform.localRotation = Quaternion.Euler(0, 0, rot);
                        arrow.SetActive(true);
                    }
                    else
                    {
                        arrow.SetActive(false);
                    }
                }
            }
        }
    }

    public void setPlayersInFusion(string playersIds)
    {
        Debug.Log("PlayersIds " + playersIds);
        _players = getPlayersByID(playersIds);
        setPlayersChild();
        updatePlayersInChimera();
    }

    public void updatePlayersInChimera()
    {
        foreach (var player in _players)
        {
            setArrows(Vector2.zero, player.GetComponent<FusionTrigger>()._characterType);
            Debug.Log("primera parte " + player.GetComponent<PhotonCharacterMultiplayerController>().isMine() + "segunda parte" + !player.GetComponent<PhotonCharacterMultiplayerController>().isHost());
            if (player.GetComponent<PhotonCharacterMultiplayerController>().isMine())
            {
                Debug.Log("ESTOY DENTRO DE LA QUIMERA");
                //llamar la funcion para decirle a todos cual maquina esta dentro de esa quimera
                _sendPlayerInChimera();
            }
        }
    }

    private List<GameObject> getPlayersByID(string playerIds)
    {
        string[] ids = playerIds.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> playersWithIds = new List<GameObject>();

        foreach (var id in ids)
        {
            foreach (var player in players)
            {
                if (player.GetComponent<FusionTrigger>()._myID().Equals(id))
                {
                    playersWithIds.Add(player);
                    Debug.Log("PLayer encontrado " + player.name + " ID " + id);
                    break;
                }
            }
        }

        return playersWithIds;
    }

    private void setPlayersChild()
    {
        _inputsMovements = new Vector2[_players.Count];

        _unFusionCheck = new bool[_players.Count];
        resetChekingUnfusion();

        int idCount = 0;

        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().setOnFusion(gameObject, idCount);

            idCount++;
        }
    }

    public void addNewPlayer(GameObject newPlayer)
    {
        _players.Add(newPlayer);

        _inputsMovements = new Vector2[_players.Count];
        _unFusionCheck = new bool[_players.Count];

        resetChekingUnfusion();

        newPlayer.GetComponent<FusionTrigger>().IsOnFusion = true;
        newPlayer.GetComponent<FusionTrigger>().CurrentChimeraParent = this;
        newPlayer.GetComponent<FusionTrigger>().OnFusionID = _players.Count;
        newPlayer.transform.SetParent(gameObject.transform);
        newPlayer.GetComponent<FusionTrigger>().assingSkillsTochimera(gameObject);

    }

    //UnFusion methods

    public void sendUnFusion(bool check, int id)
    {
        _unFusionCheck[id] = check;

        //Ver que todos se queiren desfusionar
        bool allPlayersWantUnfusion = true;
        foreach (bool checkUnfusion in _unFusionCheck)
        {
            if (!checkUnfusion)
            {
                allPlayersWantUnfusion = false;
            }
        }

        if (allPlayersWantUnfusion)
        {
            //desfusionar
            unFusion();
        }
    }

    private void unFusion()
    {
        Debug.Log("DesFusionar");

        foreach (var player in _players)
        {
            player.GetComponent<FusionTrigger>().IsOnFusion = false;
            player.GetComponent<FusionTrigger>().CurrentChimeraParent = null;
            //player.GetComponent<FusionTrigger>().OnFusionID = idCount;
            player.GetComponent<FusionTrigger>().ActiveComponentsOnFusion();

            player.transform.parent = null;

        }

        Destroy(gameObject);
    }

    private void resetChekingUnfusion()
    {
        for (int i = 0; i < _unFusionCheck.Length; i++)
        {
            _unFusionCheck[i] = false;
        }
    }

}
