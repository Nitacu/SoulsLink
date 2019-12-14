﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonChimeraMultiplayerController : MonoBehaviour
{
    private ChimeraController _chimeraController;
    private ChimeraSkillsController _chimeraSkills;
    public PhotonView _photonView;
    public List<Player> _playersInChimera = new List<Player>();


    private const string SET_PLAYERS = "setPlayersInFusion";
    private const string SEND_MOVEMENT = "sendMovement";
    private const string RECIVE_MOVEMENT_ARROW = "reciveMovementArrows";
    private const string RECIVE_PLAYERS_IN_CHIMERA = "reciveSetPlayersInChimera";

    public bool isMine()
    {
        return _photonView.IsMine;
    }

    public void addDelegate()
    {
        _chimeraController = GetComponent<ChimeraController>();
        _chimeraSkills = GetComponent<ChimeraSkillsController>();

        _chimeraController._setPlayersInFusion = new ChimeraController.DelegateMultiplayerControllerIDs(pushSetPlayersInFusion);
        _chimeraController._isMine = new ChimeraController.DelegateMultiplayerController(isMine);
        _chimeraController._sendMovement = new ChimeraController.DelegateMultiplayerControllerIMove(pushsendMovement);
        _chimeraController._sendPlayerInChimera = new ChimeraController.DelegateMultiplayerControllerSendPlayerInChimera(sendSetPlayersInChimera);

        _chimeraSkills._isMine = new ChimeraSkillsController.DelegateMultiplayerSkillController(isMine);
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
        _photonView.RPC(RECIVE_PLAYERS_IN_CHIMERA,RpcTarget.Others,PhotonNetwork.LocalPlayer);
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
        Debug.Log("id " + id + " movemente " + movement);
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
            _photonView.RPC(RECIVE_MOVEMENT_ARROW,player, direction, playerType);
        }
    }

    [PunRPC]
    public void reciveMovementArrows(Vector2 direction, GameManager.Characters playerType)
    {
        _chimeraController.setArrows(direction, playerType);
    }

    #endregion
}
