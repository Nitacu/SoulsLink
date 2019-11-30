using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;
using Photon.Realtime;
using Photon.Pun;

public class CharacterServerControlUsed : MonoBehaviour
{
    [Header("Socketweaver")]
    [SerializeField] protected NetworkID _networkID;
    [SerializeField] protected RemoteEventAgent _eventAgent;
    [SerializeField] protected RealtimeAgent _realtimeAgent;
    [SerializeField] protected SyncPropertyAgent _propertyAgent;
    [Header("Photon")]
    [SerializeField] protected PhotonTransformView _transformView;
    [SerializeField] protected PhotonView _photonView;
    [SerializeField] protected PhotonAnimatorView _animatorView;

    protected virtual void OnEnable()
    {
        switch (GameManager.GetInstace()._multiplayerServer)
        {
            case GameManager.MultiplayerServer.PHOTON:
                if (_eventAgent)
                    Destroy(_eventAgent);

                if (_realtimeAgent)
                    Destroy(_realtimeAgent);

                if (_propertyAgent)
                    Destroy(_propertyAgent);

                if (_networkID)
                    Destroy(_networkID);

                activeMultiplayerController();

                break;

            case GameManager.MultiplayerServer.SOCKETWEAVER:
                if (_transformView)
                    Destroy(_transformView);

                if (_animatorView)
                    Destroy(_animatorView);

                if (_photonView)
                    Destroy(_photonView);

                activeMultiplayerController();

                break;
        }
    }

    public virtual void activeMultiplayerController()
    {
        switch (GameManager.GetInstace()._multiplayerServer)
        {
            case GameManager.MultiplayerServer.PHOTON:
                Destroy(GetComponent<CharacterMultiplayerController>());
                GetComponent<PhotonCharacterMultiplayerController>().enabled = true;
                break;

            case GameManager.MultiplayerServer.SOCKETWEAVER:
                Destroy(GetComponent<PhotonCharacterMultiplayerController>());
                GetComponent<CharacterMultiplayerController>().enabled = true;
                break;
        }
    }
}
