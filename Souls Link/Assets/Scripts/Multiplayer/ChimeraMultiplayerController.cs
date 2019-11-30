﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class ChimeraMultiplayerController : MonoBehaviour
{
    private ChimeraController _chimeraController;
    [SerializeField] private RemoteEventAgent _remoteEventAgent;
    [SerializeField] private NetworkID _networkID;

    private const string SET_PLAYERS = "setPlayers";
    private const string SEND_MOVEMENT = "sendMovement";

    private void Start()
    {
        _chimeraController = GetComponent<ChimeraController>();
        addDelegate();
    }

    public bool isMine()
    {
        return _networkID.IsMine;
    }

    public void addDelegate()
    {
        _chimeraController._setPlayersInFusion = new ChimeraController.DelegateMultiplayerControllerIDs(pushSetPlayersInFusion);
        _chimeraController._isMine = new ChimeraController.DelegateMultiplayerController(isMine);
        _chimeraController._sendMovement = new ChimeraController.DelegateMultiplayerControllerIMove(pushsendMovement);
    }

    #region players que estan dentro de la quimera
    public void pushSetPlayersInFusion(string ids)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.PushUTF8LongString(ids);
        _remoteEventAgent.Invoke(SET_PLAYERS, message);
    }

    public void setPlayersInFusion(SWNetworkMessage message)
    {   
        _chimeraController.setPlayersInFusion(message.PopUTF8LongString());
    }
    #endregion

    public void pushsendMovement(Vector2 movement, int id)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(movement);
        message.Push(id);
        _remoteEventAgent.Invoke(SEND_MOVEMENT, message);
    }

    public void sendMovement(SWNetworkMessage message)
    {
        Vector2 movement = message.PopVector3();
        int id = message.PopUInt16();
        _chimeraController.sendMovement(movement, id);
    }
}
