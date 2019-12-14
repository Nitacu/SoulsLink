using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonChimeraMultiplayerController : MonoBehaviour
{
    private ChimeraController _chimeraController;
    public PhotonView _photonView;

    private const string SET_PLAYERS = "setPlayersInFusion";
    private const string SEND_MOVEMENT = "sendMovement";

    public bool isMine()
    {
        return _photonView.IsMine;
    }

    public void addDelegate()
    {
        _chimeraController = GetComponent<ChimeraController>();
        _chimeraController._setPlayersInFusion = new ChimeraController.DelegateMultiplayerControllerIDs(pushSetPlayersInFusion);
        _chimeraController._isMine = new ChimeraController.DelegateMultiplayerController(isMine);
        _chimeraController._sendMovement = new ChimeraController.DelegateMultiplayerControllerIMove(pushsendMovement);
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
    #endregion

    #region movimiento
    public void pushsendMovement(Vector2 movement, int id, GameManager.Characters type)
    {
        Debug.Log("id " + id + " movemente " + movement);
        _photonView.RPC(SEND_MOVEMENT, RpcTarget.MasterClient, movement, id, type);
    }

    [PunRPC]
    public void sendMovement(Vector2 movement, int id, GameManager.Characters type)
    {
        Debug.Log("LO QUE SEA");
        _chimeraController.sendMovement(movement, id, type);
    }
    #endregion
}
