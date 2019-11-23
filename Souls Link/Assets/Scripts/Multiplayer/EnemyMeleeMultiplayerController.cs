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

    private const string ATTACK = "Attack";
    private const string HEALTH = "Health";

    private void Start()
    {
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _syncPropertyAgent = GetComponent<SyncPropertyAgent>();
        _enemyController = GetComponent<SimpleEnemyController>();
    }

    private void Update()
    {
        if (!NetworkClient.Instance.IsHost)
        {
            GetComponent<BehaviorTree>().enabled = false;
        }
        else
        {
            GetComponent<BehaviorTree>().enabled = true;
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

    #region Vida
    //inicializa la vida
    public void onHealthSyncPropertyReady()
    {
        int version = _syncPropertyAgent.GetPropertyWithName(HEALTH).version;
        float health = _syncPropertyAgent.GetPropertyWithName(HEALTH).GetFloatValue();

        if (version == 0)
        {
            // colocar la vida en el maximo
            _syncPropertyAgent.Modify(HEALTH, _enemyController.health);
            health = _enemyController.health;
        }

        // carga la vida
        _enemyController.health = health;
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
