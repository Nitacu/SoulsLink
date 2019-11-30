using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Pun;
using Photon.Realtime;

public class PhotonEnemyMultiplayerController : MonoBehaviour
{
    private ControlSpawnEnemys _controlSpawnEnemys;
    private SimpleEnemyController _enemyController;

    [SerializeField] private PhotonView _photonView;

    private const string ATTACK = "getAttack";
    private const string RANGE_ATTACK = "getRangeAttack";
    private const string HEALTH = "Health";
    private const string FLIP = "Flip";

    private void Start()
    {
        _enemyController = GetComponent<SimpleEnemyController>();
        _controlSpawnEnemys = FindObjectOfType<ControlSpawnEnemys>();

        addDelegate();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
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
    }

    #region Ataque
    //MELLE
    //llama a las demas maquinas lo de atacar
    public void setAttack()
    {
        _photonView.RPC(ATTACK,RpcTarget.Others);
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
        _photonView.RPC(RANGE_ATTACK, RpcTarget.Others,direction);
    }

    //recibe la informacion de que esta atacando
    [PunRPC]
    public void getRangeAttack(Vector2 direction)
    {
        _enemyController.createdBullet(direction);
    }
    #endregion 
}
