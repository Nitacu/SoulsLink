﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Pun;
using Photon.Realtime;

public class PhotonEnemyMultiplayerController : MonoBehaviourPunCallbacks
{
    private ControlSpawnEnemys _controlSpawnEnemys;
    private SimpleEnemyController _enemyController;

    [SerializeField] private PhotonView _photonView;

    private const string ATTACK = "getAttack";
    private const string RANGE_ATTACK = "getRangeAttack";
    private const string HEALTH = "onHealthSyncPropertyChanged";
    private const string FLIP = "getFlip";

    private void Start()
    {
        _enemyController = GetComponent<SimpleEnemyController>();
        _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();

        addDelegate();
    }

    private void Update()
    {
        if (GetComponent<PolyNav.PolyNavAgent>() != null && GetComponent<BehaviorTree>() != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<PolyNav.PolyNavAgent>().enabled = true;
                GetComponent<BehaviorTree>().enabled = true;
            }
            else
            {
                GetComponent<PolyNav.PolyNavAgent>().enabled = false;
                GetComponent<BehaviorTree>().enabled = false;
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<DummyController>().enabled = true;
            }
            else
            {
                GetComponent<DummyController>().enabled = false;
            }
        }
    }


    public bool isMine()
    {
        return _photonView.IsMine;
    }

    public bool isHost()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public void destroySelf()
    {
        _controlSpawnEnemys.spawnRandomEnemy();
        PhotonNetwork.Destroy(gameObject);
    }

    public void addDelegate()
    {
        _enemyController._isHost = new SimpleEnemyController.DelegateEnemyMultiplayerController(isHost);
        _enemyController._isMine = new SimpleEnemyController.DelegateEnemyMultiplayerController(isMine);
        _enemyController._setAttack = new SimpleEnemyController.DelegateEnemyMultiplayerControllerAttack(setAttack);
        _enemyController._setRangeAttack = new SimpleEnemyController.DelegateEnemyMultiplayerControllerRangeAttack(setRangeAttack);
        _enemyController._destroySelf = new SimpleEnemyController.DelegateEnemyMultiplayerControllerDestroy(destroySelf);
        _enemyController._changeHealth = new SimpleEnemyController.DelegateEnemyMultiplayerControllerHealth(changeHealth);
        _enemyController._setFlip = new SimpleEnemyController.DelegateEnemyMultiplayerControllerDestroyFlip(setFlip);
    }

    #region Ataque
    //MELLE
    //llama a las demas maquinas lo de atacar
    public void setAttack()
    {
        _photonView.RPC(ATTACK, RpcTarget.Others);
    }

    //recibe la informacion de que esta atacando
    [PunRPC]
    public void getAttack()
    {
        _enemyController.exeAttack();
        _enemyController.Anim.Play(Animator.StringToHash("Attack"));
    }

    //RANGE
    //llama a las demas maquinas lo de atacar
    public void setRangeAttack(Vector2 direction)
    {
        _photonView.RPC(RANGE_ATTACK, RpcTarget.Others, direction);
    }

    //recibe la informacion de que esta atacando
    [PunRPC]
    public void getRangeAttack(Vector2 direction)
    {
        _enemyController.createdBullet(direction);
    }
    #endregion

    #region Vida

    //cuando detecta un cambio de vida en el servidor
    [PunRPC]
    public void onHealthSyncPropertyChanged(float health)
    {
        _enemyController.health = health;

        if (health <= 0)
        {
            _enemyController.StartCoroutine(_enemyController.die());
        }
        else
        {
            _enemyController.StartCoroutine(_enemyController.changeColor(0.5f));
        }
    }

    //envia el cambio de vida al servidor
    public void changeHealth(float health)
    {
        _photonView.RPC(HEALTH, RpcTarget.Others, health);
    }

    #endregion

    #region flip
    public void setFlip(bool flip)
    {
        _photonView.RPC(FLIP, RpcTarget.OthersBuffered, flip);
    }

    [PunRPC]
    public void getFlip(bool flip)
    {
        _enemyController._flip = flip;
    }
    #endregion

    #region dummys

    public void setNextPosition(int index)
    {
        _photonView.RPC("getNextPosition", RpcTarget.Others,index);
    }

    [PunRPC]
    public void getNextPosition(int index)
    {
        GetComponent<DummyController>().changeLocation(index);
    }

    public void setKill()
    {
        _photonView.RPC("getKill", RpcTarget.Others);
    }

    [PunRPC]
    public void getKill()
    {
        GetComponent<DummyController>().killDummy();
    }


    #endregion
}
