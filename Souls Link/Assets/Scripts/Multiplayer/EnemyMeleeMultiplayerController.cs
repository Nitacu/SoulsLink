using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyMeleeMultiplayerController : MonoBehaviour
{
    [SerializeField] private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;
    private SyncPropertyAgent _syncPropertyAgent;
    private SimpleEnemyController _enemyController;
    private ControlSpawnEnemys _controlSpawnEnemys;
    [SerializeField]private SpriteRenderer _spriteRenderer;

    private const string ATTACK = "Attack";
    private const string HEALTH = "Health";
    private const string FLIP = "Flip";

    private void Start()
    {
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _syncPropertyAgent = GetComponent<SyncPropertyAgent>();
        _enemyController = GetComponent<SimpleEnemyController>();
        _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();
    }

    private void Update()
    {
        if (NetworkClient.Instance.IsHost)
        {
            GetComponent<BehaviorTree>().enabled = false;
        }
        else
        {
            GetComponent<BehaviorTree>().enabled = true;
            changeFlip(_spriteRenderer.flipX);
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
        NetworkClient.Destroy(gameObject);
    }

    #region Flip
    //inicializa la vida
    public void onFlipSyncPropertyReady()
    {
        bool flip = _syncPropertyAgent.GetPropertyWithName(FLIP).GetBoolValue();

        if (isMine())
        {
            int version = _syncPropertyAgent.GetPropertyWithName(FLIP).version;

            if (version == 0)
            {
                // colocar la vida en el maximo
                _syncPropertyAgent.Modify(FLIP, _spriteRenderer.flipX);
                flip = _spriteRenderer.flipX;
            }
        }

        // carga la vida
        _spriteRenderer.flipX = flip;
    }

    //cuando detecta un cambio de vida en el servidor
    public void onFlipSyncPropertyChanged()
    {
        bool flip = _syncPropertyAgent.GetPropertyWithName(FLIP).GetBoolValue();

        _spriteRenderer.flipX = flip;
    }

    //envia el cambio de vida al servidor
    public void changeFlip(bool flip)
    {
        _syncPropertyAgent.Modify(FLIP, flip);
    }

    #endregion


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
        _enemyController.Anim.Play(Animator.StringToHash("Attack"));
    }
    #endregion 
}
