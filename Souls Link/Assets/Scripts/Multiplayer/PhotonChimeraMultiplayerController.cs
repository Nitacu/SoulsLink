﻿using System.Collections;
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
    private const string RECEIVE_SENDUNFUSION = "receiveSendUnFusion";
    private const string RECEIVE_UNFUSION = "receiveUnFusion";

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
        PhotonNetwork.Destroy(gameObject);
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
        _chimeraController._destroySelf = new ChimeraController.DelegateMultiplayerControllerSendPlayerInChimera(destroySelf);
        _chimeraController._sendUnFusion = new ChimeraController.DelegateMultiplayerControllerSendUnFusion(pushSendUnFusion);
        _chimeraController._unFusion = new ChimeraController.DelegateMultiplayerControllerSendPlayerInChimera(pushUnFusion);

        _chimeraSkills._isMine = new ChimeraSkillsController.DelegateMultiplayerSkillController(isMine);
        _chimeraSkills._isHost = new PlayerSkills.DelegateMultiplayerController(isHost);
        _chimeraSkills._directionShoot = new ChimeraSkillsController.DelegateMultiplayerSkillControllerDirectionShoot(sendDirectionShoot);

        _chimeraHP._isMine = new PlayerHPControl.DelegateMultiplayerController(isMine);
        _chimeraHP._destroySelf = new PlayerHPControl.DelegateMultiplayerControllerDestroy(destroySelf);
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

    #region Unfusion

    public void pushSendUnFusion(bool state, int playerID, bool forced)
    {
        _photonView.RPC(RECEIVE_SENDUNFUSION, RpcTarget.MasterClient, state, playerID, forced);
    }

    [PunRPC]
    public void receiveSendUnFusion(bool state, int playerID, bool forced)
    {
        _chimeraController.sendUnFusion(state, playerID, forced);
    }

    public void pushUnFusion()
    {
        _photonView.RPC(RECEIVE_UNFUSION, RpcTarget.Others);
    }

    [PunRPC]
    public void receiveUnFusion()
    {
        _chimeraController.unFusion();
    }

    #endregion

    #region direccion de disparo

    //les dice a las otras maquinas en que direcion deben disparar
    public void sendDirectionShoot(Vector2 direction)
    {
        _photonView.RPC("reciveDirectionShoot", RpcTarget.Others, direction);
    }

    [PunRPC]
    public void reciveDirectionShoot(Vector2 direction)
    {
        Debug.Log("JA IA");
        _chimeraSkills.HostAllow = true;
        _chimeraController.Movement1 = direction;
        _chimeraSkills.launchSkills();
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(_chimeraController.Movement1);
            stream.SendNext(_chimeraController._flip);
            stream.SendNext(_chimeraHP.PlayerHealth);
        }
        else
        {
            _chimeraController.Movement1 = (Vector2)stream.ReceiveNext();
            _chimeraController._flip = (bool)stream.ReceiveNext();
            _chimeraHP.updateMultiplayerHealth((float)stream.ReceiveNext());
        }
    }
}
