using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class CharacterMultiplayerController : MonoBehaviour
{
    private NetworkID _networkID;
    private RemoteEventAgent _remoteEventAgent;

    private PlayerMovement _playerMovement;

    #region Constantes de los nombres de las funciones que se ejecutan en todas las maquinas
    private const string PLAYER_MOVEMENT = "playerMovement";
    #endregion

    private void Start()
    {
        _networkID = GetComponent<NetworkID>();
        _remoteEventAgent = GetComponent<RemoteEventAgent>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public bool isMine()
    {
        if (_networkID.IsMine)
            return true;
        else
            return false;        
    }

    //llama los metodos necesarios para mover la copia del player en las otras maquinas
    public void pushVectorMovement(Vector3 vector)
    {
        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(vector);
        _remoteEventAgent.Invoke(PLAYER_MOVEMENT, message);
    }

    //Metodo donde llega el vector de movimiento y mueve al player de las otras maquinas
    public void playerMovement(SWNetworkMessage message)
    {
        _playerMovement.InputMovement = message.PopVector3();
    }

}
