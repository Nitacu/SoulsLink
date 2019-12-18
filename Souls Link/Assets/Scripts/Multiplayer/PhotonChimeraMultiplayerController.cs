using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonChimeraMultiplayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private ChimeraController _chimeraController;
    private ChimeraSkillsController _chimeraSkills;
    private PlayerHPControl _chimeraHP;
    public PhotonView _photonView;
    public List<Player> _playersInChimera = new List<Player>();


    private const string SET_PLAYERS = "setPlayersInFusion";
    private const string SEND_MOVEMENT = "sendMovement";
    private const string RECIVE_MOVEMENT_ARROW = "reciveMovementArrows";
    private const string RECIVE_FEEDBACK_SKILL = "reciveFeedBackSkills";
    private const string RECIVE_PLAYERS_IN_CHIMERA = "reciveSetPlayersInChimera";

    public bool isMine()
    {
        return _photonView.IsMine;
    }

    public bool isHost()
    {
        return PhotonNetwork.IsMasterClient;
    }

    public void addDelegate()
    {
        _chimeraController = GetComponent<ChimeraController>();
        _chimeraSkills = GetComponent<ChimeraSkillsController>();
        _chimeraHP = GetComponent<PlayerHPControl>();

        _chimeraController._setPlayersInFusion = new ChimeraController.DelegateMultiplayerControllerIDs(pushSetPlayersInFusion);
        _chimeraController._isMine = new ChimeraController.DelegateMultiplayerController(isMine);
        _chimeraController._sendMovement = new ChimeraController.DelegateMultiplayerControllerIMove(pushsendMovement);
        _chimeraController._sendPlayerInChimera = new ChimeraController.DelegateMultiplayerControllerSendPlayerInChimera(sendSetPlayersInChimera);
        _chimeraController._isHost = new ChimeraController.DelegateMultiplayerController(isHost);

        _chimeraSkills._isMine = new ChimeraSkillsController.DelegateMultiplayerSkillController(isMine);

        _chimeraHP._isMine = new PlayerHPControl.DelegateMultiplayerController(isMine);
    }

    #region players que estan dentro de la quimera
    public void pushSetPlayersInFusion(string ids)
    {
        Debug.Log("IDS" + ids);
        _photonView.RPC(SET_PLAYERS, RpcTarget.Others, ids);
    }

    [PunRPC]
    public void setPlayersInFusion(string ids)
    {
        _chimeraController.setPlayersInFusion(ids);
    }

    public void sendSetPlayersInChimera()
    {
        Debug.Log("ya todos saben que estoy dentro de la quimera" + PhotonNetwork.LocalPlayer.NickName);
        _photonView.RPC(RECIVE_PLAYERS_IN_CHIMERA, RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void reciveSetPlayersInChimera(Player player)
    {
        _playersInChimera.Add(player);
    }
    #endregion

    #region movimiento
    public void pushsendMovement(Vector2 movement, int id, GameManager.Characters type)
    {
        Debug.Log("id " + id + " movemente " + movement + "tamaño del array de playes " + _playersInChimera.Count);
        _photonView.RPC(SEND_MOVEMENT, RpcTarget.MasterClient, movement, id, type);
        sendMovementArrows(movement, type);
    }

    [PunRPC]
    public void sendMovement(Vector2 movement, int id, GameManager.Characters type)
    {
        Debug.Log("LO QUE SEA");
        _chimeraController.sendMovement(movement, id, type);
    }
    #endregion

    #region Flechas de movimiento
    public void sendMovementArrows(Vector2 direction, GameManager.Characters playerType)
    {
        foreach (Player player in _playersInChimera)
        {
            _photonView.RPC(RECIVE_MOVEMENT_ARROW, player, direction, playerType);
        }
    }

    [PunRPC]
    public void reciveMovementArrows(Vector2 direction, GameManager.Characters playerType)
    {
        _chimeraController.setArrows(direction, playerType);
    }

    #endregion

    #region FeedBack skills
    public void sendFeedBackSkills(GameManager.Characters typeCharacter, int skillIndex, float active)
    {
        foreach (Player player in _playersInChimera)
        {
            _photonView.RPC(RECIVE_FEEDBACK_SKILL, player, typeCharacter, skillIndex, active);
        }
    }

    [PunRPC]
    public void reciveFeedBackSkills(GameManager.Characters typeCharacter, int skillIndex, float active)
    {
        _chimeraSkills.setSkillFeedback(typeCharacter, skillIndex, active);
    }

    #endregion


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("ENVIADO");
            stream.SendNext(_chimeraController.Movement1);
            stream.SendNext(_chimeraController._flip);
            stream.SendNext(_chimeraHP.PlayerHealth);
        }
        else
        {
            _chimeraController.Movement1 = (Vector2)stream.ReceiveNext();
            Debug.Log("RECIBIDO " + _chimeraController.Movement1);
            _chimeraController._flip = (bool)stream.ReceiveNext();
            _chimeraHP.updateMultiplayerHealth((float)stream.ReceiveNext());
        }
    }
}
