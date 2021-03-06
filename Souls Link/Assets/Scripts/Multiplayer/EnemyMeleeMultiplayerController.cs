﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyMeleeMultiplayerController : MonoBehaviour
{/*
    [SerializeField] private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;
    private SyncPropertyAgent _syncPropertyAgent;
    private SimpleEnemyController _enemyController;
    private ControlSpawnEnemys _controlSpawnEnemys;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private const string ATTACK = "Attack";
    private const string RANGE_ATTACK = "RangeAttack";
    private const string HEALTH = "Health";
    private const string FLIP = "Flip";

    private void Start()
    {
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _syncPropertyAgent = GetComponent<SyncPropertyAgent>();
        _enemyController = GetComponent<SimpleEnemyController>();
        _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();
        addDelegate();
    }

    private void Update()
    {
        if (NetworkClient.Instance.IsHost)
        {
            GetComponent<PolyNavAgent>().enabled = true;
            GetComponent<BehaviorTree>().enabled = true;
        }
        else
        {
            GetComponent<PolyNavAgent>().enabled = false;
            GetComponent<BehaviorTree>().enabled = false;
            //changeFlip(_spriteRenderer.flipX);
        }
    }

    public bool isMine()
    {
        return _networkID.IsMine;
    }

    public bool isHost()
    {
        return NetworkClient.Instance.IsHost;
    }

    public void destroySelf()
    {
        _controlSpawnEnemys.spawnRandomEnemy();
        NetworkClient.Destroy(gameObject);
    }

    public void addDelegate()
    {
        _enemyController._isHost = new SimpleEnemyController.DelegateEnemyMultiplayerController(isHost);
        _enemyController._isMine = new SimpleEnemyController.DelegateEnemyMultiplayerController(isMine);
        _enemyController._setAttack = new SimpleEnemyController.DelegateEnemyMultiplayerControllerAttack(setAttack);
        _enemyController._setRangeAttack = new SimpleEnemyController.DelegateEnemyMultiplayerControllerRangeAttack(setRangeAttack);
        _enemyController._destroySelf = new SimpleEnemyController.DelegateEnemyMultiplayerControllerDestroy(destroySelf);
        _enemyController._changeHealth = new SimpleEnemyController.DelegateEnemyMultiplayerControllerHealth(changeHealth);
    }
  
    #region Vida
    //inicializa la vida
    public void onHealthSyncPropertyReady()
    {
        int version = _syncPropertyAgent.GetPropertyWithName(HEALTH).version;
        if (version == 0)
        {
            // colocar la vida en el maximo
            _syncPropertyAgent.Modify(HEALTH, _enemyController.health);
        }
    }

    //cuando detecta un cambio de vida en el servidor
    public void onHealthSyncPropertyChanged()
    {
        float health = _syncPropertyAgent.GetPropertyWithName(HEALTH).GetFloatValue();
        _enemyController.health = health;
        if (health <= 0 && _syncPropertyAgent.GetPropertyWithName(HEALTH).version > 1)
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
        _syncPropertyAgent.Modify(HEALTH, health);
    }

    #endregion

    #region Ataque
    //llama a las demas maquinas lo de atacar
    public void setAttack()
    {
        _remoteEventAgent.Invoke(ATTACK);
    }

    //recibe la informacion de que esta atacando
    public void getAttack()
    {
        _enemyController.exeAttack();
        _enemyController.Anim.Play(Animator.StringToHash("Attack"));
    }

    //llama a las demas maquinas lo de atacar
    public void setRangeAttack(Vector2 direction)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(direction);
        _remoteEventAgent.Invoke(RANGE_ATTACK, message);
    }

    //recibe la informacion de que esta atacando
    public void getRangeAttack(SWNetworkMessage message)
    {
        Vector2 direction = message.PopVector3();
        _enemyController.createdBullet(direction);
    }
    #endregion */
}
