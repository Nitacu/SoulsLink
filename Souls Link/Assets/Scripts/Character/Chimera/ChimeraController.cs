﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraController : PlayerMovement
{
    [SerializeField] private float _speed = 100;
    public float Speed
    {
        get { return _speed; }
    }

    [SerializeField] private List<GameObject> _players = new List<GameObject>();
    [SerializeField] private List<GameObject> _arrows = new List<GameObject>();

    private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private Vector2 _movement;
    public Vector2 Movement
    {
        get { return _movement; }
    }
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

            if (player.GetComponent<PhotonCharacterMultiplayerController>().isMine() &&
                !player.GetComponent<PhotonCharacterMultiplayerController>().isHost())
            {
                Debug.Log("ESTOY DENTRO DE LA QUIMERA");
                //llamar la funcion para decirle a todos cual maquina esta dentro de esa quimera
                _sendPlayerInChimera();
            }
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
        _rb.velocity = _movement * Time.deltaTime * _speed;

        setAnimation();
    }

    private void setAnimation()
    {
        //RotarSprite
        if (_movement.x > 0)
        {
            _renderer.flipX = false;
        }
        else if (_movement.x < 0)
        {
            _renderer.flipX = true;
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

        _movement = newinputMovement;
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
